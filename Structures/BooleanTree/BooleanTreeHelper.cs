using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures.BooleanTree {
  public class BooleanTreeHelper {

    public static Conjunction Conjunction(BooleanExpr expr1,  BooleanExpr expr2) {
      return new Conjunction(expr1, expr2);
    }

    public static Disjunction Disjunction(BooleanExpr expr1, BooleanExpr expr2) {
      return new Disjunction(expr1, expr2);
    }

    public static Negation Negation(BooleanExpr expr1) {
      return new Negation(expr1);
    }

    public static BVar BVar(string name) {
      return new BVar(name);
    }

    public static True True() {
      return new True();
    }

    public static False False() {
      return new False();
    }
  }
}