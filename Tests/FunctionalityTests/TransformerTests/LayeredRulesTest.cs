using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structures.ArithmeticTree;
using static Structures.ArithmeticTree.TreeHelper;
using LiTra.Transformation;

namespace Tests.FunctionalityTests.TransformerTests {
  [TestClass]
  public class LayeredRulesTest {

    [TestMethod]
    public void LayeredRulesTest_GeneralizeInput() {
      var expr = Addition(Const(1), Const(4));
      Assert.AreEqual("1 + 4", expr.ToString());

      var transformer = new Transformer()
        .Mutator<Const>(c => c.Value += 1)
        .Outputter<Expr, Addition>((input, output) => { output.Expr1 = input; output.Expr2 = Const(0); });
      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("((2 + 0) + (5 + 0)) + 0", result.ToString());
    }

    [TestMethod]
    public void LayeredRulesTest_GeneralizeOutput() {
      var expr = Addition(Const(1), Const(4));
      Assert.AreEqual("1 + 4", expr.ToString());

      var transformer = new Transformer()
        .Outputter<Const, Multiplication>((input, output) => { output.Expr1 = input; output.Expr2 = Const(2); }) //Double values
        .Outputter<Const, BinaryExpr>((input, output) => output.Expr2 = Const(4)); //Quadruple them instead
      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("(1 * 4) + (4 * 4)", result.ToString());
    }

    [TestMethod]
    public void LayeredRulesTest_GeneralizeInputAndOutput() {
      var expr = Subtraction(Addition(Const(1), Const(4)), Multiplication(Const(1), Const(5)));
      Assert.AreEqual("(1 + 4) - (1 * 5)", expr.ToString());

      var transformer = new Transformer()
        .Outputter<Addition, Multiplication>(input => Multiplication(input.Expr1, input.Expr2))
        .Outputter<BinaryExpr, Division>(input => Division(input.Expr1, input.Expr2))
        .Outputter<BinaryExpr, BinaryExpr>((input, output) => output.Expr2 = Const(10));
      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("(1 * 10) / 10", result.ToString());
    }

    [TestMethod]
    public void LayeredRulesTest_IgnoreIncompatibleRule() {
      var expr = Addition(Const(1), Const(4));
      Assert.AreEqual("1 + 4", expr.ToString());

      var transformer = new Transformer()
        .Outputter<Const, Multiplication>((input, output) => { output.Expr1 = input; output.Expr2 = Const(2); }) //Double values
        .Outputter<Const, Addition>((input, output) => { output.Expr1 = input; output.Expr2 = Const(0); }); //Should not be called
      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("(1 * 2) + (4 * 2)", result.ToString());
    }    
  }
}
