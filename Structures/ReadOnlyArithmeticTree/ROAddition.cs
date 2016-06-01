using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures.ReadOnlyArithmeticTree {
  public class ROAddition : ROBinaryExpr {

    public ROAddition(ROExpr expr1, ROExpr expr2) : base(expr1, expr2) { }

    public override string ToString() {
      return $"{Expr1.ToNestedString()} + {Expr2.ToNestedString()}";
    }
  }
}
