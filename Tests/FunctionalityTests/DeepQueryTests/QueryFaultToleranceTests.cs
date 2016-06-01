using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structures.ArithmeticTree;
using static Structures.ArithmeticTree.TreeHelper;
using static Structures.ArithmeticListTree.TreeHelper;
using LiTra.DeepQuery;
using Structures.ArithmeticListTree;
using Structures.ReadOnlyArithmeticTree;
using static Structures.ReadOnlyArithmeticTree.TreeHelper;
using static Structures.ReadOnlyArithmeticListTree.TreeHelper;
using Structures.ReadOnlyArithmeticListTree;

namespace Tests.FunctionalityTests.QueryTests {
  [TestClass]
  public class QueryFaultToleranceTests {

    [TestMethod]
    public void QueryFaultToleranceTests_NullRoot() {
      Expr root = null;
      var result = root.DeepQuery<Var>();

      Assert.AreEqual(0, result.Values.Count());
    }

    [TestMethod]
    public void QueryFaultToleranceTests_NullProperty() {
      Expr root = Division(null, Addition(Var("x"), Var("y")));
      var result = root.DeepQuery<Var>();
      //The null value should simply be ignored
      Assert.AreEqual(2, result.Values.Count());
    }

    [TestMethod]
    public void QueryFaultToleranceTests_NullField() {
      Expr root = Division(Addition(Var("x"), Var("y")), null);
      var result = root.DeepQuery<Var>();
      //The null value should simply be ignored
      Assert.AreEqual(2, result.Values.Count());
    }

    [TestMethod]
    public void QueryFaultToleranceTests_NullListElement() {
      LExpr root = LDivision(LAddition(LVar("x"), LVar("y")), null);
      var result = root.DeepQuery<LVar>();
      //The null value should simply be ignored
      Assert.AreEqual(2, result.Values.Count());
    }

    [TestMethod]
    public void QueryFaultToleranceTests_DoesNotTransformReadonlyProperty() {
      ROExpr root = RODivision(ROVar("y"), ROAddition(ROVar("x"), ROConst(1)));
      Assert.AreEqual("y / (x + 1)", root.ToString());

      var result = root.DeepQuery<ROVar>();
      Assert.AreEqual(2, result.Values.Count());
      result.Transform(v => ROVar("z"));

      //Since the variables are stored in readonly properties they should not be transformer
      Assert.AreEqual("y / (x + 1)", root.ToString());
    }

    [TestMethod]
    public void QueryFaultToleranceTests_DoesNotTransformReadonlyField() {
      ROExpr root = RODivision(ROAddition(ROConst(1), ROVar("x")), ROVar("y"));
      Assert.AreEqual("(1 + x) / y", root.ToString());

      var result = root.DeepQuery<ROVar>();
      Assert.AreEqual(2, result.Values.Count());
      result.Transform(v => ROVar("z"));

      //Since the variables are stored in readonly fields they should not be transformer
      Assert.AreEqual("(1 + x) / y", root.ToString());
    }

    [TestMethod]
    public void QueryFaultToleranceTests_DoesNotTransformReadonlyList() {
      ROLExpr root = ROLDivision(ROLAddition(ROLVar("x"), ROLVar("x")), ROLVar("y"));
      Assert.AreEqual("(x + x) / y", root.ToString());

      var result = root.DeepQuery<ROLVar>();
      Assert.AreEqual(3, result.Values.Count());
      result.Transform(v => ROVar("z"));

      //Since the variables are stored in readonly collections they should not be transformer
      Assert.AreEqual("(x + x) / y", root.ToString());
    }
  }
}
