using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures.BooleanTree {
  public class Negation : BooleanExpr {
    public BooleanExpr Expr { get; set; }

    public Negation() { }
    public Negation(BooleanExpr expr) {
      this.Expr = expr;
    }

    internal override string ToNestedString() {
      return ToString();
    }

    public override string ToString() {
      return $"¬{Expr.ToNestedString()}";
    }
  }
}