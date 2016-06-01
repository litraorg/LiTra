using LiTra.DeepQuery.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LiTra.DeepQuery {
  public class DeepQueryResult<TSource> {
    private Dictionary<TSource, List<Context<TSource>>> _contexts;   
    public IEnumerable<TSource> Values { get; private set; }

    internal DeepQueryResult() {
      Values = new HashSet<TSource>();
      _contexts = new Dictionary<TSource, List<Context<TSource>>>();
    }

    internal void MergeWith(DeepQueryResult<TSource> queryResult) {
      ((HashSet<TSource>)Values).UnionWith(queryResult.Values);
      foreach(var key in queryResult._contexts.Keys) {
        List<Context<TSource>> existingContexts;
        if (!_contexts.TryGetValue(key, out existingContexts)) {
          existingContexts = new List<Context<TSource>>();
          _contexts.Add(key, existingContexts);
        }
        existingContexts.AddRange(queryResult._contexts[key]);
      }
    }

    internal void AddValueWithoutContext(TSource value) {
      ((HashSet<TSource>)Values).Add(value);
    }

    internal void AddPropertyContext(object parent, PropertyInfo parentProperty, TSource value) {
      ((HashSet<TSource>)Values).Add(value);
      if (parentProperty.CanWrite) {        
        var context = new PropertyContext<TSource>(parent, parentProperty, value);
        getContexts(value).Add(context);
      }
    }

    internal void AddFieldContext(object parent, FieldInfo parentField, TSource value) {
      ((HashSet<TSource>)Values).Add(value);
      if (!parentField.IsInitOnly) {
        var context = new FieldContext<TSource>(parent, parentField, value);
        getContexts(value).Add(context);
      }
    }

    internal void AddListContext(IList list, int index, TSource value) {
      ((HashSet<TSource>)Values).Add(value);      
      var context = new ListContext<TSource>(list, index, value);
      getContexts(value).Add(context);
    }

    private List<Context<TSource>> getContexts(TSource value) {
      List<Context<TSource>> existingContexts;
      if (!_contexts.TryGetValue(value, out existingContexts)) {
        existingContexts = new List<Context<TSource>>();
        _contexts.Add(value, existingContexts);
      }
      return existingContexts;
    }

    public void Mutate(Action<TSource> mutator) {
      foreach (var context in _contexts.Values.SelectMany(i => i)) {
        context.Mutate(mutator);
      }
    }

    internal void Mutate(IEnumerable<TSource> source, Action<TSource> mutator) {
      foreach (var s in source) {
        List<Context<TSource>> list;
        if (_contexts.TryGetValue(s, out list)) {
          foreach (var context in list) {
            context.Mutate(mutator);
          }
        }
      }
    }

    public void Transform<TResult>(Func<TSource, TResult> transformer) {
      foreach (var context in _contexts.Values.SelectMany(i => i)) {
        context.Transform(transformer);
      }
    }

    internal void Transform<TResult>(IEnumerable<TSource> source, Func<TSource, TResult> transformer) {
      foreach (var s in source) {
        List<Context<TSource>> list;
        if (_contexts.TryGetValue(s, out list)) {
          foreach (var context in list) {
            context.Transform(transformer);
          }
        }
      }
    }
  }
}
