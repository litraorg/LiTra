using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using LiTra.Transformation.Rules;

namespace LiTra.Transformation {
  public class Transformer {
    private TransformCache cache;
    private RuleManager ruleManager;
    private bool hasAppliedRule;
    internal TransformationStrategy strategy;

    public Transformer() {
      cache = new TransformCache();
      ruleManager = new RuleManager(cache);
    }

    public Transformer Mutator<I>(Predicate<I> condition, Action<I> body) {
      ruleManager.SaveMutator(condition, body);
      return this;
    }

    public Transformer Mutator<I>(Action<I> body) {
      ruleManager.SaveMutator(null, body);
      return this;
    }

    public Transformer Outputter<I, O>(Action<I, O> body) {
      ruleManager.SaveOutputter(null, null, body);
      return this;
    }

    public Transformer Outputter<I, O>(Predicate<I> condition, Action<I, O> body) {
      ruleManager.SaveOutputter(condition, null, body);
      return this;
    }

    public Transformer Outputter<I>(Action<I, I> body) {
      ruleManager.SaveOutputter(null, null, body);
      return this;
    }

    public Transformer Outputter<I>(Predicate<I> condition, Action<I, I> body) {
      ruleManager.SaveOutputter(condition, null, body);
      return this;
    }

    public Transformer Outputter<I, O>(Func<I, O> initializer, Action<I, O> body = null) {
      ruleManager.SaveOutputter(null, initializer, body);
      return this;
    }

    public Transformer Outputter<I, O>(Predicate<I> condition, Func<I, O> initializer, Action<I, O> body = null) {
      ruleManager.SaveOutputter(condition, initializer, body);
      return this;
    }

    public Transformer Outputter<I>(Func<I, I> initializer, Action<I, I> body = null) {
      ruleManager.SaveOutputter(null, initializer, body);
      return this;
    }

    public Transformer Outputter<I>(Predicate<I> condition, Func<I, I> initializer, Action<I, I> body = null) {
      ruleManager.SaveOutputter(condition, initializer, body);
      return this;
    }

    private object TransformNode(object input, TransformationStrategy strategy, Type outputType, HashSet<object> visited) {
      object cachedValue;
      if (cache.TryGetValueOrInitialize(input, outputType, out cachedValue)) return cachedValue;

      visited.Add(input);
      var result = ApplyRulesAccordingToStrategy(input, strategy, outputType, visited);
      visited.Remove(input);

      return result;
    }

    private object ApplyRulesAccordingToStrategy(object input, TransformationStrategy strategy, Type outputType, HashSet<object> visited) {
      object result;
      switch (strategy) {
        case TransformationStrategy.BOTTOM_UP:
          TransformSubstructures(input, strategy, visited);
          result = ruleManager.ApplyRules(input, outputType, ref hasAppliedRule);
          break;
        case TransformationStrategy.TOP_DOWN:
          result = ruleManager.ApplyRules(input, outputType, ref hasAppliedRule);
          TransformSubstructures(result, strategy, visited);
          break;
        case TransformationStrategy.NONE:
          result = ruleManager.ApplyRules(input, outputType, ref hasAppliedRule);
          break;
        default:
          throw new ArgumentException("Unknown transformation strategy used.");
      }
      return result;
    }

    private void TransformSubstructures(object input, TransformationStrategy strategy, HashSet<object> visited) {
      var type = input.GetType();

      if (input is IList) TransformListElements((IList)input, strategy, visited, type);
      TransformProperties(input, strategy, visited, type);
      TransformFields(input, strategy, visited, type);
    }

    private void TransformListElements(IList list, TransformationStrategy strategy, HashSet<object> visited, Type inputType) {
      for (int i = 0; i < list.Count; i++) {
        var child = list[i];
        if (ReferenceEquals(child, null) || (visited.Contains(child) && strategy.HasFlag(TransformationStrategy.BOTTOM_UP))) continue;
        var outputType = inputType.IsGenericType ? inputType.GetGenericArguments()[0] : typeof(object);
        var transformedChild = TransformNode(child, strategy, outputType, visited);
        SetListElement(list, i, child, transformedChild);
      }
    }

    private void SetListElement(IList list, int index, object originalValue, object newValue) {
      var originalType = originalValue.GetType();
      if (!ReferenceEquals(originalValue, newValue)
        && (!originalType.IsValueType || !originalType.Equals(newValue.GetType()) || !originalValue.Equals(newValue))) {
        try {
          list[index] = newValue;
        } catch (NotSupportedException) { } //Is thrown if the list is unmodifiable
      }
    }

    private void TransformProperties(object input, TransformationStrategy strategy, HashSet<object> visited, Type inputType) {
      var properties = inputType.GetProperties();
      foreach (var property in properties) {
        if (property.GetIndexParameters().Length != 0) continue;
        var child = property.GetValue(input);
        if (ReferenceEquals(child, null) || (visited.Contains(child) && strategy.HasFlag(TransformationStrategy.BOTTOM_UP))) continue;
        var transformedChild = TransformNode(child, strategy, property.PropertyType, visited);
        SetProperty(property, input, child, transformedChild);
      }
    }

    private void SetProperty(PropertyInfo property, object parent, object originalValue, object newValue) {
      var originalType = originalValue.GetType();
      if (!ReferenceEquals(originalValue, newValue) && property.CanWrite
        && (!originalType.IsValueType || !originalType.Equals(newValue.GetType()) || !originalValue.Equals(newValue))) {
        property.SetValue(parent, newValue);
      }
    }

    private void TransformFields(object input, TransformationStrategy strategy, HashSet<object> visited, Type inputType) {
      var fields = inputType.GetFields(BindingFlags.Instance | BindingFlags.Public);
      foreach (var field in fields) {
        var child = field.GetValue(input);
        if (ReferenceEquals(child, null) || (visited.Contains(child) && strategy.HasFlag(TransformationStrategy.BOTTOM_UP))) continue;
        var transformedChild = TransformNode(child, strategy, field.FieldType, visited);
        SetField(field, input, child, transformedChild);
      }
    }

    private void SetField(FieldInfo field, object parent, object originalValue, object newValue) {
      var originalType = originalValue.GetType();
      if (!ReferenceEquals(originalValue, newValue) && !field.IsInitOnly
        && (!originalType.IsValueType || !originalType.Equals(newValue.GetType()) || !originalValue.Equals(newValue))) {
        field.SetValue(parent, newValue);
      }
    }

    internal O Resolve<O>(object input, TransformationStrategy strategy = TransformationStrategy.NONE) {
      if (ReferenceEquals(input, null)) return default(O);

      object result = null;
      Type outputType = typeof(O);
      HashSet<object> visited = new HashSet<object>();
      if (strategy.HasFlag(TransformationStrategy.REPEAT)) {
        do {
          cache.Clear();
          hasAppliedRule = false;
          result = TransformNode(result ?? input, strategy & ~TransformationStrategy.REPEAT, outputType, visited);
          visited.Clear();
        } while (hasAppliedRule);
      } else {
        result = TransformNode(input, strategy, outputType, visited);
      }

      if (result is O) {
        return (O)result;
      } else {
        return default(O);
      }
    }

    public O Transform<O>(object input, TransformationStrategy strategy = TransformationStrategy.NONE) {
      TransformationExtensions.RegisterTransformer(this);
      this.strategy = strategy;
      O result;
      try {
        result = Resolve<O>(input, strategy);
      } finally {
        TransformationExtensions.DeregisterTransformer();
        cache.Clear();
      }
      return result;
    }

    public IEnumerable<O> TransformEach<O>(IEnumerable input, TransformationStrategy strategy = TransformationStrategy.NONE) {
      foreach (var element in input) {
        var result = Transform<O>(element, strategy);
        if (!ReferenceEquals(result, null))
          yield return result;
      }
    }
  }
}