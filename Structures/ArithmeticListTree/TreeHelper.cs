using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures.ArithmeticListTree {
  public class TreeHelper {
    public static LConst LConst(int value) {
      return new LConst(value);
    }
    
    public static LVar LVar(string name) {
      return new LVar(name);
    }

    public static LAddition LAddition(LExpr expr1, LExpr expr2) {
      return new LAddition(expr1, expr2);
    }

    public static LSubtraction LSubtraction(LExpr expr1, LExpr expr2) {
      return new LSubtraction(expr1, expr2);
    }

    public static LMultiplication LMultiplication(LExpr expr1, LExpr expr2) {
      return new LMultiplication(expr1, expr2);
    }

    public static LDivision LDivision(LExpr expr1, LExpr expr2) {
      return new LDivision(expr1, expr2);
    }
  }
}
