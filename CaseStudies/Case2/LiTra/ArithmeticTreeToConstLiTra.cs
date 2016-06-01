using Structures.ArithmeticTree;
using LiTra.Transformation;

namespace CaseStudies.Case2.LiTra {
  public class ArithmeticTreeToConstLiTra {
    public static Const Transform(Expr expr) {
      var transformer = new Transformer()
        .Outputter<Addition, Const>((input, output) =>
          output.Value = (input.Expr1 as Const).Value + (input.Expr2 as Const).Value
        )
        .Outputter<Subtraction, Const>((input, output) =>
          output.Value = (input.Expr1 as Const).Value - (input.Expr2 as Const).Value
        )
        .Outputter<Multiplication, Const>((input, output) =>
          output.Value = (input.Expr1 as Const).Value * (input.Expr2 as Const).Value
        )
        .Outputter<Division, Const>((input, output) =>
          output.Value = (input.Expr1 as Const).Value / (input.Expr2 as Const).Value
        );
      return transformer.Transform<Const>(expr, TransformationStrategy.BOTTOM_UP);
    }
  }
}