using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures.ArithmeticTree {
  public class TreeHelper {
    public static Const Const(int value) {
      return new Const(value);
    }

    public static AppendableVar AppendableVar(string name) {
      return new AppendableVar(new StringBuilder(name));
    }

    public static Var Var(string name) {
      return new Var(name);
    }

    public static Addition Addition(Expr expr1, Expr expr2) {
      return new Addition(expr1, expr2);
    }

    public static Subtraction Subtraction(Expr expr1, Expr expr2) {
      return new Subtraction(expr1, expr2);
    }

    public static Multiplication Multiplication(Expr expr1, Expr expr2) {
      return new Multiplication(expr1, expr2);
    }

    public static Division Division(Expr expr1, Expr expr2) {
      return new Division(expr1, expr2);
    }
  }
}
