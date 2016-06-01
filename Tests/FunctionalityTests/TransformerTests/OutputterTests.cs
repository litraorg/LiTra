using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structures.ArithmeticTree;
using static Structures.ArithmeticTree.TreeHelper;
using LiTra.Transformation;
using Structures.ArithmeticListTree;

namespace Tests.FunctionalityTests.TransformerTests {
  [TestClass]
  public class OutputterTests {

    [TestMethod]
    public void OutputterTests_Body() {
      var y = Var("y");
      var x = Var("x");
      var expr = Division(Subtraction(x, Const(4)), Addition(x, y));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      var transformer = new Transformer();      
      transformer.Outputter<Var>((input, output) => output.Name = "z");
      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("(z - 4) / (z + z)", result.ToString());
      Assert.AreEqual("(z - 4) / (z + z)", expr.ToString());
      Assert.AreEqual("x", x.ToString());
      Assert.AreEqual("y", y.ToString());
      Assert.AreSame(expr, result);
    }

    [TestMethod]
    public void OutputterTests_ConditionAndBody() {
      var x = Var("x");
      var y = Var("y");
      var expr = Division(Subtraction(x, Const(4)), Addition(x, y));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      var transformer = new Transformer();
      transformer.Outputter<Var>(input => input.Name == "x", (input, output) => output.Name = "z");
      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("(z - 4) / (z + y)", result.ToString());
      Assert.AreEqual("(z - 4) / (z + y)", expr.ToString());
      Assert.AreEqual("x", x.ToString());
      Assert.AreEqual("y", y.ToString());
      Assert.AreSame(expr, result);
    }

    [TestMethod]
    public void OutputterTests_Initializer() {
      var y = Var("y");
      var x = Var("x");
      var expr = Division(Subtraction(x, Const(4)), Addition(x, y));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      var z = Var("z");
      var transformer = new Transformer();
      transformer.Outputter<Var>(input => z);
      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("(z - 4) / (z + z)", result.ToString());
      Assert.AreEqual("(z - 4) / (z + z)", expr.ToString());
      Assert.AreEqual("x", x.ToString());
      Assert.AreEqual("y", y.ToString());
      Assert.AreSame(z, ((dynamic)expr).Expr1.Expr1);
      Assert.AreSame(expr, result);
    }

    [TestMethod]
    public void OutputterTests_InitializerAndBody() {
      var y = Var("y");
      var x = Var("x");
      var expr = Division(Subtraction(x, Const(4)), Addition(x, y));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      var newVar = new Var();
      var transformer = new Transformer();
      transformer.Outputter<Var>(input => newVar, (input, output) => output.Name = "z");
      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("(z - 4) / (z + z)", result.ToString());
      Assert.AreEqual("(z - 4) / (z + z)", expr.ToString());
      Assert.AreEqual("x", x.ToString());
      Assert.AreEqual("y", y.ToString());
      Assert.AreEqual("z", newVar.ToString());
      Assert.AreSame(expr, result);
    }

    [TestMethod]
    public void OutputterTests_ConditionAndInitializer() {
      var y = Var("y");
      var x = Var("x");
      var expr = Division(Subtraction(x, Const(4)), Addition(x, y));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      var z = Var("z");
      var transformer = new Transformer();
      transformer.Outputter<Var>(input => input.Name == "x", input => z);
      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("(z - 4) / (z + y)", result.ToString());
      Assert.AreEqual("(z - 4) / (z + y)", expr.ToString());
      Assert.AreEqual("x", x.ToString());
      Assert.AreSame(z, ((dynamic)expr).Expr1.Expr1);
      Assert.AreEqual("y", y.ToString());
      Assert.AreSame(expr, result);
    }

    [TestMethod]
    public void OutputterTests_ConditionInitializerAndBody() {
      var y = Var("y");
      var x = Var("x");
      var expr = Division(Subtraction(x, Const(4)), Addition(x, y));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      var newVar = new Var();
      var transformer = new Transformer();
      transformer.Outputter<Var>(input => input.Name == "x", input => newVar, (input, output) => output.Name = "z");
      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("(z - 4) / (z + y)", result.ToString());
      Assert.AreEqual("(z - 4) / (z + y)", expr.ToString());
      Assert.AreEqual("x", x.ToString());
      Assert.AreEqual("y", y.ToString());
      Assert.AreEqual("z", newVar.ToString());
      Assert.AreSame(expr, result);
    }

    [TestMethod]
    public void OutputterTests_OneGenericType() {
      var expr = Division(Subtraction(Var("x"), Const(4)), Addition(Var("x"), Var("y")));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());
      
      var transformer = new Transformer();
      transformer.Outputter<Var>((input, output) => output.Name = input.Name + "'");
      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("(x' - 4) / (x' + y')", result.ToString());
      Assert.AreEqual("(x' - 4) / (x' + y')", expr.ToString());
      Assert.AreSame(expr, result);
    }

    [TestMethod]
    public void OutputterTests_TwoGenericTypes() {
      var expr = Division(Subtraction(Var("x"), Const(4)), Addition(Var("x"), Var("y")));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      var transformer = new Transformer();
      transformer.Outputter<Var, Const>(input => input.Name == "x", (input, output) => output.Value = 1);
      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("(1 - 4) / (1 + y)", result.ToString());
      Assert.AreEqual("(1 - 4) / (1 + y)", expr.ToString());
      Assert.AreSame(expr, result);
    }

    [TestMethod]
    public void OutputterTests_OutOfPlaceTransformation() {
      var expr = Division(Subtraction(Const(7), Const(3)), Addition(Const(1), Const(1)));
      Assert.AreEqual("(7 - 3) / (1 + 1)", expr.ToString());

      var transformer = new Transformer();
      transformer.Outputter<Addition, LAddition>((input, output) => output.Expressions = new List<LExpr> { input.Expr1.As<LExpr>(), input.Expr2.As<LExpr>() });
      transformer.Outputter<Subtraction, LSubtraction>((input, output) => output.Expressions = new List<LExpr> { input.Expr1.As<LExpr>(), input.Expr2.As<LExpr>() });
      transformer.Outputter<Multiplication, LMultiplication>((input, output) => output.Expressions = new List<LExpr> { input.Expr1.As<LExpr>(), input.Expr2.As<LExpr>() });
      transformer.Outputter<Division, LDivision>((input, output) => output.Expressions = new List<LExpr> { input.Expr1.As<LExpr>(), input.Expr2.As<LExpr>() });      
      transformer.Outputter<Var, LVar>((input, output) => output.Name = input.Name);
      transformer.Outputter<Const, LConst>((input, output) => output.Value = input.Value);

      var result = transformer.Transform<LExpr>(expr, TransformationStrategy.NONE);

      Assert.AreEqual("(7 - 3) / (1 + 1)", result.ToString());
      Assert.AreEqual("(7 - 3) / (1 + 1)", expr.ToString());
      Assert.IsInstanceOfType(result, typeof(LExpr));
    }

    [TestMethod]
    public void OutputterTests_InPlaceTransformation() {
      var expr = Multiplication(Addition(Var("x"), Const(0)), Const(2));
      Assert.AreEqual("(x + 0) * 2", expr.ToString());

      var transformer = new Transformer()
      .Outputter<Addition, Expr>(
        input => input.Expr1 is Const && ((Const)input.Expr1).Value == 0,
        input => input.Expr2
      )
      .Outputter<Addition, Expr>(
        input => input.Expr2 is Const && ((Const)input.Expr2).Value == 0,
        input => input.Expr1
      );

      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreEqual("x * 2", result.ToString());
      Assert.AreSame(expr, result);
    }
  }
}
