using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiTra.Transformation.Rules {
  internal abstract class Rule {
    protected object condition;
    protected object body;

    internal Type InputType { get; set; }

    internal Rule(object body) {
      this.body = body;
    }

    internal Rule(object condition, object body) {
      this.condition = condition;
      this.body = body;
    }

    internal bool IsApplicable(object input) {
      return ReferenceEquals(condition, null) || (bool)InvokeLambda(condition, input);
    }
    
    /// Helper method for invoking unspecified delegates/lambdas
    protected virtual object InvokeLambda(object lambda, params object[] parameters) {
      var lambdaType = lambda.GetType();      
      return lambdaType.GetMethod("Invoke").Invoke(lambda, parameters);
    }
  }
}
