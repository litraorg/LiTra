using System.Collections.Generic;
using System.Linq;

namespace Structures.ArithmeticListTree {
  public class LAddition : LBinaryExpr {

    public LAddition() {
      Expressions = new List<LExpr>();
    }

    public LAddition(LExpr expr1, LExpr expr2) {
      Expressions = new List<LExpr> { expr1, expr2 };
    }

    public override string ToString() {
      return string.Join(" + ", Expressions.Select(e => e.ToNestedString()));
    }
    
  }
}
