using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures.ReadOnlyArithmeticTree {
  public class TreeHelper {
    public static ROConst ROConst(int value) {
      return new ROConst(value);
    }   

    public static ROVar ROVar(string name) {
      return new ROVar(name);
    }

    public static ROAddition ROAddition(ROExpr expr1, ROExpr expr2) {
      return new ROAddition(expr1, expr2);
    }

    public static ROSubtraction ROSubtraction(ROExpr expr1, ROExpr expr2) {
      return new ROSubtraction(expr1, expr2);
    }

    public static ROMultiplication ROMultiplication(ROExpr expr1, ROExpr expr2) {
      return new ROMultiplication(expr1, expr2);
    }

    public static RODivision RODivision(ROExpr expr1, ROExpr expr2) {
      return new RODivision(expr1, expr2);
    }
  }
}
