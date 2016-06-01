using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures.ReadOnlyArithmeticTree {
  public abstract class ROBinaryExpr : ROExpr{
    private readonly ROExpr expr1;
    public ROExpr Expr1 { get { return expr1; } }
    public readonly ROExpr Expr2;

    protected ROBinaryExpr(ROExpr expr1, ROExpr expr2) {
      this.expr1 = expr1;
      this.Expr2 = expr2;
    }
  }
}
