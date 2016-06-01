using LiTra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiTra.Transformation.Rules {
  internal class Outputter : Rule {
    private object initializer;

    internal Type OutputType { get; set; }

    internal Outputter(object condition, object initializer, object body) : base(condition, body) {
      this.initializer = initializer;
    }

    internal object InitializeOutput(object input, Type outputType) {
      if (!ReferenceEquals(initializer, null)) return InvokeLambda(initializer, input);

      var emptyConstructor = outputType.GetConstructor(Type.EmptyTypes);
      if (ReferenceEquals(emptyConstructor, null)) {
        throw new NoEmptyConstructorException($"Cannot instantiate output object of type {outputType}, as that type has no public empty constructor. For custom instantiation, use an InstantiatingRule.");
      }

      return emptyConstructor.Invoke(Array.Empty<object>());      
    }

    internal object Invoke(object input, object output) {
      if (ReferenceEquals(body, null)) return output;
      return InvokeLambda(body, input, output);
    }
  }
}
