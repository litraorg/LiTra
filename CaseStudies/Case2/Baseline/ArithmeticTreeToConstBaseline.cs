using Structures.ArithmeticTree;

namespace CaseStudies.Case2.Baseline {
  public class ArithmeticTreeToConstBaseline {
    public static Const Transform(Expr input) {
      return Eval((dynamic)input);
    }

    private static Const Eval(Addition input) {
      return new Const(Transform(input.Expr1).Value + Transform(input.Expr2).Value);
    }

    private static Const Eval(Subtraction input) {
      return new Const(Transform(input.Expr1).Value - Transform(input.Expr2).Value);
    }

    private static Const Eval(Multiplication input) {
      return new Const(Transform(input.Expr1).Value * Transform(input.Expr2).Value);
    }

    private static Const Eval(Division input) {
      return new Const(Transform(input.Expr1).Value / Transform(input.Expr2).Value);
    }

    private static Const Eval(Const input) {
      return input;
    }
  }
}