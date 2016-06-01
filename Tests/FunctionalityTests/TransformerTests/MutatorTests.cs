using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structures.ArithmeticTree;
using static Structures.ArithmeticTree.TreeHelper;
using LiTra.Transformation;

namespace Tests.FunctionalityTests.TransformerTests {
  [TestClass]
  public class MutatorTests {

    [TestMethod]
    public void MutatorTests_Body() {
      var expr = Division(Subtraction(Var("x"), Const(4)), Addition(Var("x"), Var("y")));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      var transformer = new Transformer();      
      transformer.Mutator<Var>(v => v.Name = "z");
      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("(z - 4) / (z + z)", result.ToString());
      Assert.AreEqual("(z - 4) / (z + z)", expr.ToString());
    }

    [TestMethod]
    public void MutatorTests_ConditionAndBody() {
      var expr = Division(Subtraction(Var("x"), Const(4)), Addition(Var("x"), Var("y")));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      var transformer = new Transformer();     
      transformer.Mutator<Var>(v => v.Name == "x", v => v.Name = "z");
      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("(z - 4) / (z + y)", result.ToString());
      Assert.AreEqual("(z - 4) / (z + y)", expr.ToString());
    }

    [TestMethod]
    public void MutatorTests_InPlaceTransformation() {
      var expr = Division(Subtraction(Var("x"), Const(4)), Addition(Const(2), Var("y")));
      Assert.AreEqual("(x - 4) / (2 + y)", expr.ToString());

      var transformer = new Transformer();
      transformer.Mutator<Const>(v => v.Value++);
      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("(x - 5) / (3 + y)", result.ToString());
      Assert.AreEqual("(x - 5) / (3 + y)", expr.ToString());
    }
  }
}
