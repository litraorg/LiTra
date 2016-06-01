using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiTra.Transformation.Rules {
  internal class Mutator : Rule {   
    internal Mutator(object body) : base(body) { }
    internal Mutator(object condition, object lambda) : base(condition, lambda) { }

    internal void Invoke(object input) {
      if (!ReferenceEquals(body, null)) {
        InvokeLambda(body, input);
      }
    }
  }
}
