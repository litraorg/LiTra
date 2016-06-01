using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures.BooleanTree {
  public class True : BooleanExpr {
    internal override string ToNestedString() {
      return ToString();
    }

    public override string ToString() {
      return "True";
    }
  }
}