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
  public class RuleOrderTests {
    [TestMethod]
    public void RuleOrderTests_MutatorsAppliedBeforeOutputters() {
      var expr = Addition(Const(1), Const(4));
      Assert.AreEqual("1 + 4", expr.ToString());

      var transformer = new Transformer()
        .Mutator<Const>(input => input.Value *= 2)
        .Outputter<Const, Addition>((input, output) => { output.Expr1 = input; output.Expr2 = Const(0); })
        .Mutator<Const>(input => input.Value -= 2);
      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("(0 + 0) + (6 + 0)", result.ToString());
    }

    [TestMethod]
    public void RuleOrderTests_OutputtersAppliedInDefinitionOrder() {
      var expr = Addition(Const(1), Const(4));
      Assert.AreEqual("1 + 4", expr.ToString());

      var transformer = new Transformer()
        .Outputter<Const>((input, output) => output.Value = input.Value + 3)
        .Outputter<Const>((input, output) => output.Value *= 2);

      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("8 + 14", result.ToString());
    }

    [TestMethod]
    public void RuleOrderTests_MutatorsAppliedInDefinitionOrder() {
      var expr = Addition(Const(1), Const(4));
      Assert.AreEqual("1 + 4", expr.ToString());

      var transformer = new Transformer()
        .Mutator<Const>(input => input.Value -= 3)
        .Mutator<Const>(input => input.Value *= 2);

      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("-4 + 2", result.ToString());
    }
  }
}
