using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures.ReadOnlyArithmeticTree {
  public class ROConst : ROExpr {
    public readonly int Value;
    
    public ROConst(int value) {
      Value = value;
    }

    internal override string ToNestedString() {
      return ToString();
    }

    public override string ToString() {
      return $"{Value}";
    }
  }
}
