using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures.ReadOnlyArithmeticListTree {
  public class TreeHelper {
    public static ROLConst ROLConst(int value) {
      return new ROLConst(value);
    }
    
    public static ROLVar ROLVar(string name) {
      return new ROLVar(name);
    }

    public static ROLAddition ROLAddition(ROLExpr expr1, ROLExpr expr2) {
      return new ROLAddition(expr1, expr2);
    }

    public static LSubtraction ROLSubtraction(ROLExpr expr1, ROLExpr expr2) {
      return new LSubtraction(expr1, expr2);
    }

    public static ROLMultiplication ROLMultiplication(ROLExpr expr1, ROLExpr expr2) {
      return new ROLMultiplication(expr1, expr2);
    }

    public static ROLDivision ROLDivision(ROLExpr expr1, ROLExpr expr2) {
      return new ROLDivision(expr1, expr2);
    }
  }
}
