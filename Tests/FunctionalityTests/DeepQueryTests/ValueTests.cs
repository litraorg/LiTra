using Microsoft.VisualStudio.TestTools.UnitTesting;
using LiTra.DeepQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structures.ArithmeticTree;
using Structures.ArithmeticListTree;
using static Structures.ArithmeticTree.TreeHelper;
using static Structures.ArithmeticListTree.TreeHelper;

namespace Tests.FunctionalityTests.QueryTests {
  [TestClass]
  public class ValueTests {

    [TestMethod]
    public void ValueTests_CorrectValuesPropertiesAndFields() {
      var y1 = Var("y");
      var x1 = Var("x");
      var x2 = Var("x");

      var expr = Division(Subtraction(x1, Const(4)), Addition(x2, y1));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      var vars = expr.DeepQuery<Var>().Values;
      Assert.AreEqual(3, vars.Count());
      Assert.IsTrue(vars.Contains(y1));
      Assert.IsTrue(vars.Contains(x1));
      Assert.IsTrue(vars.Contains(x2));

      //no side-effects
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());
    }

    [TestMethod]
    public void ValueTests_CorrectValuesPropertiesAndFieldsDeep() {
      var expr1 = Subtraction(Var("x"), Const(4));
      var expr2 = Addition(Var("x"), Var("y"));
      var expr3 = Division(expr1, expr2);
      
      Assert.AreEqual("(x - 4) / (x + y)", expr3.ToString());

      var binExprs = expr3.DeepQuery<BinaryExpr>().Values;
      Assert.AreEqual(3, binExprs.Count());
      Assert.IsTrue(binExprs.Contains(expr1));
      Assert.IsTrue(binExprs.Contains(expr2));
      Assert.IsTrue(binExprs.Contains(expr3));

      //no side-effects
      Assert.AreEqual("(x - 4) / (x + y)", expr3.ToString());
    }

    [TestMethod]
    public void ValueTests_CorrectValuesLists() {
      var y1 = LVar("y");
      var x1 = LVar("x");
      var x2 = LVar("x");

      var expr = LDivision(LSubtraction(x1, LConst(4)), LAddition(x2, y1));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      var vars = expr.DeepQuery<LVar>().Values;
      Assert.AreEqual(3, vars.Count());
      Assert.IsTrue(vars.Contains(y1));
      Assert.IsTrue(vars.Contains(x1));
      Assert.IsTrue(vars.Contains(x2));

      //no side-effects
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());
    }

    [TestMethod]
    public void ValueTests_CorrectValuesListsDeep() {
      var expr1 = LSubtraction(LVar("x"), LConst(4));
      var expr2 = LAddition(LVar("x"), LVar("y"));
      var expr3 = LDivision(expr1, expr2);

      Assert.AreEqual("(x - 4) / (x + y)", expr3.ToString());

      var binExprs = expr3.DeepQuery<LBinaryExpr>().Values;
      Assert.AreEqual(3, binExprs.Count());
      Assert.IsTrue(binExprs.Contains(expr1));
      Assert.IsTrue(binExprs.Contains(expr2));
      Assert.IsTrue(binExprs.Contains(expr3));

      //no side-effects
      Assert.AreEqual("(x - 4) / (x + y)", expr3.ToString());
    }

    [TestMethod]
    public void ValueTests_CachePersistance() {
      var expr1 = Subtraction(Var("x"), Const(4));      
      var expr2 = Multiplication(Const(5), Const(7));
      var expr3 = Addition(expr2, Var("y"));
      var expr4 = Division(expr1, expr3);

      Assert.AreEqual("(x - 4) / ((5 * 7) + y)", expr4.ToString());

      var binExprs = expr4.DeepQuery<BinaryExpr>().Values;
      Assert.AreEqual(4, binExprs.Count());
      Assert.IsTrue(binExprs.Contains(expr1));
      Assert.IsTrue(binExprs.Contains(expr2));
      Assert.IsTrue(binExprs.Contains(expr3));
      Assert.IsTrue(binExprs.Contains(expr4));      

      //Start new query on modified input
      expr3.Expr1 = Const(10);

      Assert.AreEqual("(x - 4) / (10 + y)", expr4.ToString());

      var newQuery = expr4.DeepQuery<BinaryExpr>().Values;
      Assert.AreEqual(4, newQuery.Count());
      Assert.IsTrue(newQuery.Contains(expr1));
      Assert.IsTrue(newQuery.Contains(expr2));
      Assert.IsTrue(newQuery.Contains(expr3));
      Assert.IsTrue(newQuery.Contains(expr4));
    }

    [TestMethod]
    public void ValueTests_CacheClear() {
      var expr1 = Subtraction(Var("x"), Const(4));
      var expr2 = Multiplication(Const(5), Const(7));
      var expr3 = Addition(expr2, Var("y"));
      var expr4 = Division(expr1, expr3);

      Assert.AreEqual("(x - 4) / ((5 * 7) + y)", expr4.ToString());

      var binExprs = expr4.DeepQuery<BinaryExpr>().Values;
      Assert.AreEqual(4, binExprs.Count());
      Assert.IsTrue(binExprs.Contains(expr1));
      Assert.IsTrue(binExprs.Contains(expr2));
      Assert.IsTrue(binExprs.Contains(expr3));
      Assert.IsTrue(binExprs.Contains(expr4));

      //Start new query on modified input, but clear cache beforehand
      expr3.Expr1 = Const(10);

      Assert.AreEqual("(x - 4) / (10 + y)", expr4.ToString());

      Cache.ClearCache();
      var newQuery = expr4.DeepQuery<BinaryExpr>().Values;
      Assert.AreEqual(3, newQuery.Count());
      Assert.IsTrue(newQuery.Contains(expr1));
      Assert.IsTrue(newQuery.Contains(expr3));
      Assert.IsTrue(newQuery.Contains(expr4));
    }
  }
}
