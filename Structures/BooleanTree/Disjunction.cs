using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures.BooleanTree {
  public class Disjunction : BooleanExpr {
    public BooleanExpr Expr1 { get; set; }
    public BooleanExpr Expr2 { get; set; }

    public Disjunction() { }
    public Disjunction(BooleanExpr expr1, BooleanExpr expr2) {
      this.Expr1 = expr1;
      this.Expr2 = expr2;
    }

    public override string ToString() {
      return $"{Expr1.ToNestedString()} ∨ {Expr2.ToNestedString()}";
    }
  }
}