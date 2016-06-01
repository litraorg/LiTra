using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiTra.Transformation.Rules {
  internal class RuleManager {
    private TransformCache _cache;
    private List<Mutator> _mutators;
    private List<Outputter> _outputters;

    internal RuleManager(TransformCache cache) {
      this._cache = cache;
      _mutators = new List<Mutator>();
      _outputters = new List<Outputter>();
    }

    internal void SaveMutator<I>(Predicate<I> condition, Action<I> body) {
      _mutators.Add(new Mutator(condition, body) { InputType = typeof(I) });
    }

    internal void SaveOutputter<I, O>(Predicate<I> condition, Func<I, O> initializer, Action<I, O> body) {
      _outputters.Add(new Outputter(condition, initializer, body) { InputType = typeof(I), OutputType = typeof(O) });
    }

    private IEnumerable<Mutator> SelectMutators(object input) {
      for (int i = 0; i < _mutators.Count; i++) {
        var rule = _mutators[i];
        if (IsApplicableToInput(input, rule)) {
          yield return rule;
        }
      }
    }

    private IEnumerable<Outputter> SelectOutputters(object input, Type outputType) {
      Outputter firstRule;
      int firstRuleIndex = SelectFirstOutputter(input, outputType, out firstRule);
      if (firstRuleIndex < 0) yield break;
      yield return firstRule;

      for (int j = firstRuleIndex + 1; j < _outputters.Count; j++) {
        var otherRule = _outputters[j];
        if (IsApplicableAndCanUseExistingOutput(input, otherRule, firstRule.OutputType)) {
          yield return otherRule;
        }
      }
    }

    private bool IsApplicableToInput(object input, Rule rule) {
      return rule.InputType.IsInstanceOfType(input) && rule.IsApplicable(input);
    }

    private bool IsApplicableAndCanUseExistingOutput(object input, Outputter rule, Type outputType) {
      return IsApplicableToInput(input, rule) && rule.OutputType.IsAssignableFrom(outputType);
    }

    private bool IsApplicableAndAssignableToOutput(object input, Outputter rule, Type outputType) {
      return IsApplicableToInput(input, rule) && outputType.IsAssignableFrom(rule.OutputType);
    }

    private int SelectFirstOutputter(object input, Type outputType, out Outputter firstRule) {
      for (int i = 0; i < _outputters.Count; i++) {
        var rule = _outputters[i];
        if (IsApplicableAndAssignableToOutput(input, rule, outputType)) {
          firstRule = rule;
          return i;
        }
      }
      firstRule = null;
      return -1;
    }

    internal object ApplyRules(object input, Type outputType, ref bool ruleApplied) {
      var mutators = SelectMutators(input);
      var outputters = SelectOutputters(input, outputType);
      //Tell the caller if a rule is applied
      ruleApplied = ruleApplied || mutators.Count() > 0 || outputters.Count() > 0;
      //Instantiate an output object for the first outputter
      var output = InstantiateOutput(outputters, input, outputType);
      //Cache result of applying the rules before they are invoked.
      if (ReferenceEquals(output, null)) {
        _cache.AddValue(input, outputType, input);
      } else {
        _cache.AddValue(input, output.GetType(), output);
      }
      
      InvokeMutators(mutators, input);
      InvokeOutputters(outputters, input, output);
      
      return output ?? input;
    }

    private object InstantiateOutput(IEnumerable<Outputter> outputters, object input, Type outputType) {
      if (outputters.Count() == 0) return null;
      var firstOutputter = outputters.First();
      return firstOutputter.InitializeOutput(input, firstOutputter.OutputType);
    }    

    private void InvokeMutators(IEnumerable<Mutator> mutators, object input) {
      foreach (var mutator in mutators) {
        mutator.Invoke(input);
      }
    }

    private void InvokeOutputters(IEnumerable<Outputter> outputters, object input, object output) {
      foreach (var outputter in outputters) {
        outputter.Invoke(input, output);
      }
    }
  }
}
