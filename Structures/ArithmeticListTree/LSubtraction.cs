using System.Collections.Generic;
using System.Linq;

namespace Structures.ArithmeticListTree {
  public class LSubtraction : LBinaryExpr {

    public LSubtraction() {
      Expressions = new List<LExpr>();
    }

    public LSubtraction(LExpr expr1, LExpr expr2) {
      Expressions = new List<LExpr> { expr1, expr2 };
    }

    public override string ToString() {
      return string.Join(" - ", Expressions.Select(e => e.ToNestedString()));
    }
  }
}
