using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Transformations;
using Structures.ArithmeticTree;
using CaseStudies.Case2.Baseline;
using CaseStudies.Case2.NMF;
using CaseStudies.Case2.LiTra;
using static Structures.ArithmeticTree.TreeHelper;

namespace Tests.EvaluationTests {
  [TestClass]
  public class Case2Tests {
    [TestMethod]
    public void Case2Test() {
      var expr = Division(Subtraction(Const(7), Const(3)), Addition(Const(1), Const(1)));
      Assert.AreEqual("(7 - 3) / (1 + 1)", expr.ToString());

      var baselineResult = ArithmeticTreeToConstBaseline.Transform(expr);
      Assert.AreNotSame(expr, baselineResult);
      Assert.AreEqual("2", baselineResult.ToString());

      var nmfResult = TransformationEngine.Transform<Expr, Expr>(expr, new ArithmeticTreeToConstNMF());
      Assert.AreNotSame(expr, nmfResult);
      Assert.AreEqual("2", nmfResult.ToString());

      var ourResult = ArithmeticTreeToConstLiTra.Transform(expr);
      Assert.AreNotSame(expr, ourResult);
      Assert.AreEqual("2", ourResult.ToString());
    }
  }
}