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
  public class MutateTests {
    [TestMethod]
    public void MutateTests_MutateValuesFieldsAndProperties() {
      var y1 = Var("y");
      var x1 = Var("x");
      var x2 = Var("x");

      var expr = Division(Subtraction(x1, Const(4)), Addition(x2, y1));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      //query
      var vars = expr.DeepQuery<Var>();
      vars.Mutate(v => v.Name = "z");

      Assert.AreEqual("(z - 4) / (z + z)", expr.ToString());
    }

    [TestMethod]
    public void MutateTests_MutateValuesLists() {
      var y1 = LVar("y");
      var x1 = LVar("x");
      var x2 = LVar("x");

      var expr = LDivision(LSubtraction(x1, LConst(4)), LAddition(x2, y1));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      //query
      var vars = expr.DeepQuery<LVar>();
      vars.Mutate(v => v.Name = "z");

      Assert.AreEqual("(z - 4) / (z + z)", expr.ToString());
    }

    [TestMethod]
    public void MutateTests_MutateValuesFieldsAndPropertiesLINQ() {
      var y1 = Var("y");
      var x1 = Var("x");
      var x2 = Var("x");

      var expr = Division(Subtraction(x1, Const(4)), Addition(x2, y1));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      //query
      var vars = expr.DeepQuery<Var>();
      vars.Values.Where(v => v.Name == "x").Mutate(vars, x => x.Name = "z");

      Assert.AreEqual("(z - 4) / (z + y)", expr.ToString());
    }

    [TestMethod]
    public void MutateTests_MutateValuesListsLINQ() {
      var y1 = LVar("y");
      var x1 = LVar("x");
      var x2 = LVar("x");

      var expr = LDivision(LSubtraction(x1, LConst(4)), LAddition(x2, y1));
      Assert.AreEqual("(x - 4) / (x + y)", expr.ToString());

      //query
      var vars = expr.DeepQuery<LVar>();
      vars.Values.Where(v => v.Name == "x").Mutate(vars, x => x.Name = "z");

      Assert.AreEqual("(z - 4) / (z + y)", expr.ToString());
    }
  }
}
