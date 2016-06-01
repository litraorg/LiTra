using Structures.ArithmeticTree;
using NMF.Transformations;
using NMF.Transformations.Core;

namespace CaseStudies.Case2.NMF {
  public class ArithmeticTreeToConstNMF : ReflectiveTransformation {
    public class ExprToExpr : AbstractTransformationRule<Expr, Expr> { }

    public class ConstToConst : TransformationRule<Const, Const> {
      public override void Transform(Const input, Const output, ITransformationContext context) {
        output.Value = input.Value;
      }

      public override void RegisterDependencies() {
        MarkInstantiatingFor(Rule<ExprToExpr>());
      }
    }

    public class AdditionToConst : TransformationRule<Addition, Const> {
      public override void Transform(Addition input, Const output, ITransformationContext context) {
        output.Value = context.Trace.Resolve<Expr, Const>(input.Expr1).Value + context.Trace.Resolve<Expr, Const>(input.Expr2).Value;
      }

      public override void RegisterDependencies() {
        RequireMany(
          rule: Rule<ExprToExpr>(),
          selector: input => new Expr[] { input.Expr1, input.Expr2 }
        );
        MarkInstantiatingFor(Rule<ExprToExpr>());
      }
    }

    public class SubtractionToConst : TransformationRule<Subtraction, Const> {
      public override void Transform(Subtraction input, Const output, ITransformationContext context) {
        output.Value = context.Trace.Resolve<Expr, Const>(input.Expr1).Value - context.Trace.Resolve<Expr, Const>(input.Expr2).Value;
      }

      public override void RegisterDependencies() {
        RequireMany(
          rule: Rule<ExprToExpr>(),
          selector: input => new Expr[] { input.Expr1, input.Expr2 }
        );
        MarkInstantiatingFor(Rule<ExprToExpr>());
      }
    }

    public class MultiplicationToConst : TransformationRule<Multiplication, Const> {
      public override void Transform(Multiplication input, Const output, ITransformationContext context) {
        output.Value = context.Trace.Resolve<Expr, Const>(input.Expr1).Value * context.Trace.Resolve<Expr, Const>(input.Expr2).Value;
      }

      public override void RegisterDependencies() {
        RequireMany(
          rule: Rule<ExprToExpr>(),
          selector: input => new Expr[] { input.Expr1, input.Expr2 }
        );
        MarkInstantiatingFor(Rule<ExprToExpr>());
      }
    }

    public class DivisionToConst : TransformationRule<Division, Const> {
      public override void Transform(Division input, Const output, ITransformationContext context) {
        output.Value = context.Trace.Resolve<Expr, Const>(input.Expr1).Value / context.Trace.Resolve<Expr, Const>(input.Expr2).Value;
      }

      public override void RegisterDependencies() {
        RequireMany(
          rule: Rule<ExprToExpr>(),
          selector: input => new Expr[] { input.Expr1, input.Expr2 }
        );
        MarkInstantiatingFor(Rule<ExprToExpr>());
      }
    }
  }
}