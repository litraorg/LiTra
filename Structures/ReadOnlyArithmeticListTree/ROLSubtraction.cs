using System.Collections.Generic;
using System.Linq;

namespace Structures.ReadOnlyArithmeticListTree {
  public class LSubtraction : ROLBinaryExpr {

    public LSubtraction() {
      Expressions = new List<ROLExpr>().AsReadOnly();
    }

    public LSubtraction(ROLExpr expr1, ROLExpr expr2) {
      Expressions = new List<ROLExpr> { expr1, expr2 }.AsReadOnly();
    }

    public override string ToString() {
      return string.Join(" - ", Expressions.Select(e => e.ToNestedString()));
    }
  }
}
