using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures.BooleanTree {
  public abstract class BooleanExpr {

    internal virtual string ToNestedString() {
      return $"({this})";
    }

    public abstract override string ToString();
  }
}