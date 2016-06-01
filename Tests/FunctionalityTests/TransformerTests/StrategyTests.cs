using Microsoft.VisualStudio.TestTools.UnitTesting;
using LiTra.Transformation;
using Structures.ArithmeticTree;
using Structures.BooleanTree;
using static Structures.ArithmeticTree.TreeHelper;
using static Structures.BooleanTree.BooleanTreeHelper;

namespace Tests.FunctionalityTests.TransformerTests {
  [TestClass]
  public class StrategyTests {

    [TestMethod]
    public void StrategyTests_TopDown() {
      var expr = Negation(Disjunction(Conjunction(BVar("p"), BVar("q")), Negation(BVar("r"))));
      Assert.AreEqual("¬((p ∧ q) ∨ ¬r)", expr.ToString());

      var transformer = new Transformer();
      AddNNFRules(transformer);

      var result = transformer.Transform<BooleanExpr>(expr, TransformationStrategy.TOP_DOWN);
      Assert.AreEqual("(¬p ∨ ¬q) ∧ r", result.ToString());
    }

    [TestMethod]
    public void StrategyTests_BottomUp() {
      var expr = Division(Subtraction(Const(7), Const(3)), Addition(Const(1), Const(1)));
      Assert.AreEqual("(7 - 3) / (1 + 1)", expr.ToString());

      var transformer = new Transformer();
      AddEvalRules(transformer);

      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);
      Assert.AreEqual("2", result.ToString());
    }

    [TestMethod]
    public void StrategyTests_None() {
      var expr = Division(Subtraction(Const(7), Const(3)), Division(Const(1), Const(1)));
      Assert.AreEqual("(7 - 3) / (1 / 1)", expr.ToString());

      var transformer = new Transformer();
      transformer.Outputter<BinaryExpr, Addition>((input, output) => { output.Expr1 = input.Expr1; output.Expr2 = input.Expr2; });

      var result = transformer.Transform<Expr>(expr, TransformationStrategy.NONE);
      Assert.AreEqual("(7 - 3) + (1 / 1)", result.ToString());
    }

    [TestMethod]
    public void StrategyTests_Repeat() {
      var expr = Division(Const(1), Division(Const(1), Const(1)));
      Assert.AreEqual("1 / (1 / 1)", expr.ToString());

      var transformer = new Transformer();
      transformer
        .Mutator<BinaryExpr>(
          input => input.Expr1 is Const && ((Const)input.Expr1).Value < 3,
          input => ((Const)input.Expr1).Value++
        ).Mutator<BinaryExpr>(
          input => input.Expr2 is Const && ((Const)input.Expr2).Value < 3,
          input => ((Const)input.Expr2).Value++
        );

      var result = transformer.Transform<Expr>(expr, TransformationStrategy.REPEAT);
      Assert.AreEqual("3 / (1 / 1)", result.ToString());
    }

    [TestMethod]
    public void StrategyTests_TopDownRepeat() {
      var expr = Negation(Negation(Negation(Negation(BVar("p")))));
      Assert.AreEqual("¬¬¬¬p", expr.ToString());

      var transformer = new Transformer();
      AddNNFRules(transformer);

      var singleRunResult = transformer.Transform<BooleanExpr>(expr, TransformationStrategy.TOP_DOWN);
      var repeatResult = transformer.Transform<BooleanExpr>(expr, TransformationStrategy.TOP_DOWN | TransformationStrategy.REPEAT);
      Assert.AreNotEqual("p", singleRunResult.ToString());
      Assert.AreEqual("p", repeatResult.ToString());
    }

    [TestMethod]
    public void StrategyTests_BottomUpRepeat() {
      var expr = Negation(Disjunction(Conjunction(BVar("p"), BVar("q")), Negation(BVar("r"))));
      Assert.AreEqual("¬((p ∧ q) ∨ ¬r)", expr.ToString());

      var transformer = new Transformer();
      AddNNFRules(transformer);

      var singleRunResult = transformer.Transform<BooleanExpr>(expr, TransformationStrategy.BOTTOM_UP);
      var repeatResult = transformer.Transform<BooleanExpr>(expr, TransformationStrategy.BOTTOM_UP | TransformationStrategy.REPEAT);
      Assert.AreNotEqual("(¬p ∨ ¬q) ∧ r", singleRunResult.ToString());
      Assert.AreEqual("(¬p ∨ ¬q) ∧ r", repeatResult.ToString());
    }

    [TestMethod]
    public void StrategyTests_NoneRepeat() {
      var expr = Division(Const(1), Division(Const(1), Const(1)));
      Assert.AreEqual("1 / (1 / 1)", expr.ToString());

      var transformer = new Transformer();
      transformer
        .Mutator<BinaryExpr>(
          input => input.Expr1 is Const && ((Const)input.Expr1).Value < 3,
          input => ((Const)input.Expr1).Value++
        ).Mutator<BinaryExpr>(
          input => input.Expr2 is Const && ((Const)input.Expr2).Value < 3,
          input => ((Const)input.Expr2).Value++
        );

      var result = transformer.Transform<Expr>(expr, TransformationStrategy.NONE | TransformationStrategy.REPEAT);
      Assert.AreEqual("3 / (1 / 1)", result.ToString());
    }

    private void AddNNFRules(Transformer transformer) {
      transformer
      .Outputter<Negation, BooleanExpr>(
        n => n.Expr is Negation,
        n => ((Negation)n.Expr).Expr
      ).Outputter<Negation, Disjunction>(
        n => n.Expr is Conjunction,
        (input, output) => {
          var conj = input.Expr as Conjunction;
          output.Expr1 = Negation(conj.Expr1);
          output.Expr2 = Negation(conj.Expr2);
        }
      ).Outputter<Negation, Conjunction>(
        n => n.Expr is Disjunction,
        (input, output) => {
          var disjunction = input.Expr as Disjunction;
          output.Expr1 = Negation(disjunction.Expr1);
          output.Expr2 = Negation(disjunction.Expr2);
        }
      );
    }

    private void AddEvalRules(Transformer transformer) {
      transformer
        .Outputter<Addition, Const>(
          input => input.Expr1 is Const && input.Expr2 is Const,
          (input, output) => output.Value = ((Const)input.Expr1).Value + ((Const)input.Expr2).Value
        )
        .Outputter<Multiplication, Const>(
          input => input.Expr1 is Const && input.Expr2 is Const,
          (input, output) => output.Value = ((Const)input.Expr1).Value * ((Const)input.Expr2).Value
        )
        .Outputter<Division, Const>(
          input => input.Expr1 is Const && input.Expr2 is Const,
          (input, output) => output.Value = ((Const)input.Expr1).Value / ((Const)input.Expr2).Value
        )
        .Outputter<Subtraction, Const>(
          input => input.Expr1 is Const && input.Expr2 is Const,
          (input, output) => output.Value = ((Const)input.Expr1).Value - ((Const)input.Expr2).Value
        );
    }

  }
}
