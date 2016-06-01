using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structures.ArithmeticTree;
using static Structures.ArithmeticTree.TreeHelper;
using LiTra.Transformation;
using Structures.ArithmeticListTree;
using Structures.ReadOnlyArithmeticTree;
using static Structures.ReadOnlyArithmeticTree.TreeHelper;
using static Structures.ReadOnlyArithmeticListTree.TreeHelper;
using Structures.ReadOnlyArithmeticListTree;

namespace Tests.FunctionalityTests.TransformerTests {
  [TestClass]
  public class TransformerFaultToleranceTests {

    [TestMethod]
    public void TransformerFaultToleranceTests_DesiredOutputTypeNotApplicable() {
      var expr = Division(Subtraction(Var("x"), Const(4)), Addition(Var("x"), Var("y")));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      var transformer = new Transformer();
      transformer.Outputter<Var>((input, output) => output.Name = "z");
      //Attempts to transform to LExpr, which should return in a null output as the rules do not support this
      var result = transformer.Transform<LExpr>(expr, TransformationStrategy.BOTTOM_UP);

      Assert.AreSame(null, result);
    }

    [TestMethod]
    [ExpectedException(typeof(LiTra.Exceptions.NoEmptyConstructorException))]
    public void TransformerFaultToleranceTests_NoEmptyConstructor() {
      var expr = RODivision(ROSubtraction(ROVar("x"), ROConst(4)), ROAddition(ROVar("x"), ROVar("y")));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      var transformer = new Transformer();
      //ROVar has no empty constructor and therefore an initializer is required to use it in a outputter rule
      transformer.Outputter<ROVar>((input, output) => output.Name = "z");
      var result = transformer.Transform<ROExpr>(expr, TransformationStrategy.BOTTOM_UP);      
    }

    [TestMethod]
    public void TransformerFaultToleranceTests_ReadOnlyFieldAndProperyNotModified() {
      var expr = RODivision(ROSubtraction(ROVar("x"), ROConst(4)), ROAddition(ROVar("x"), ROVar("y")));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      var transformer = new Transformer();
      //ROVar has no empty constructor and therefor an initializer is required to use it in a outputter rule
      transformer.Outputter<ROVar>(input => ROVar("z"));
      var result = transformer.Transform<ROExpr>(expr, TransformationStrategy.BOTTOM_UP);

      //The fields and properties containing subexpression are readonly and therefore should not be modified
      Assert.AreEqual("(x - 4) / (x + y)", result.ToString());
    }

    [TestMethod]
    public void TransformerFaultToleranceTests_ReadOnlyListNotModified() {
      var expr = ROLDivision(ROLSubtraction(ROLVar("x"), ROLConst(4)), ROLAddition(ROLVar("x"), ROLVar("y")));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      var transformer = new Transformer();
      //ROVar has no empty constructor and therefor an initializer is required to use it in a outputter rule
      transformer.Outputter<ROLVar>(input => ROLVar("z"));
      var result = transformer.Transform<ROLExpr>(expr, TransformationStrategy.BOTTOM_UP);

      //The fields and properties containing subexpression are readonly and therefore should not be modified
      Assert.AreEqual("(x - 4) / (x + y)", result.ToString());
    }

    [TestMethod]
    public void TransformerFaultToleranceTests_NullInput() {
      var transformer = new Transformer();
      transformer.Outputter<ROVar>(input => ROVar("z"));
      var result = transformer.Transform<ROExpr>(null, TransformationStrategy.BOTTOM_UP);

      //Given null as input null should always be returned
      Assert.AreSame(null, result);
    }

    [TestMethod]
    public void TransformerFaultToleranceTests_NullInInputStructure() {
      var expr = Division(null, Addition(Var("x"), Var("y")));
      Assert.AreEqual(" / (x + y)", expr.ToString());

      var transformer = new Transformer();
      transformer.Outputter<Var>((input, output) => output.Name = "z");
      var result = transformer.Transform<Expr>(expr, TransformationStrategy.BOTTOM_UP);

      //Given null as input null should always be returned
      Assert.AreEqual(" / (z + z)", result.ToString());
    }

    [TestMethod]
    [ExpectedException(typeof(LiTra.Exceptions.ResolveOutsideTransformationRuleException))]
    public void TransformerFaultToleranceTests_AsOutOfTransformation() {
      var expr = Addition(Var("x"), Var("y"));
      expr.As<Var>();
    }

    [TestMethod]
    [ExpectedException(typeof(LiTra.Exceptions.ResolveOutsideTransformationRuleException))]
    public void TransformerFaultToleranceTests_EachAsOutOfTransformation() {
      var exprs = new List<Expr> { Const(2), Var("y") };
      exprs.EachAs<Var>();
    }

    [TestMethod]
    [ExpectedException(typeof(LiTra.Exceptions.ResolveOutsideTransformationRuleException))]
    public void TransformerFaultToleranceTests_EachAsWithActionOutOfTransformation() {
      var exprs = new List<Expr> { Var("x"), Var("y") };
      exprs.EachAs<Var>(v => v.Name = "z");
    }

    [TestMethod]
    [ExpectedException(typeof(LiTra.Exceptions.ResolveOutsideTransformationRuleException))]
    public void TransformerFaultToleranceTests_AddEachAsOutOfTransformation() {
      var exprs = new List<Expr> { Var("x"), Var("y") };
      var vars = new List<Var>();
      vars.AddEachAs<Var>(exprs);
    }

    [TestMethod]
    [ExpectedException(typeof(LiTra.Exceptions.ResolveOutsideTransformationRuleException))]
    public void TransformerFaultToleranceTests_AddAsOutOfTransformation() {
      Expr expr = Var("x");
      var vars = new List<Var>();
      vars.AddAs<Var>(expr);
    }
  }
}
