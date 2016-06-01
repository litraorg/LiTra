using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Structures.ArithmeticTree;
using static Structures.ArithmeticTree.TreeHelper;
using Structures.ArithmeticListTree;
using static Structures.ArithmeticListTree.TreeHelper;
using LiTra.Transformation;
using System;
using System.Reflection;

namespace Tests.FunctionalityTests.TransformerTests {
  [TestClass]
  public class ExtensionTests {
    [TestMethod]
    public void ExtensionTests_As() {
      var expr = Addition(Const(1), Const(4));
      Assert.AreEqual("1 + 4", expr.ToString());

      var transformer = new Transformer()
        .Mutator<Addition>(a => { a.Expr1 = a.Expr1.As<Expr>(); a.Expr2 = a.Expr2.As<Expr>(); })
        .Mutator<Const>(c => c.Value += 1);

      var result = transformer.Transform<Expr>(expr, TransformationStrategy.NONE);

      Assert.AreEqual("2 + 5", result.ToString());
    }

    [TestMethod]
    public void ExtensionTests_AsOnNull() {
      var expr = Addition(Const(1), Const(4));
      Assert.AreEqual("1 + 4", expr.ToString());

      expr.Expr1 = null;
      Assert.IsNull(expr.Expr1);
      var exceptionCatched = false;

      var transformer = new Transformer()
        .Mutator<Addition>(a => {
          try {
            a.Expr1 = a.Expr1.As<Expr>();
          } catch (NullReferenceException) {
            exceptionCatched = true;
          }          
          a.Expr2 = a.Expr2.As<Expr>();
        })
        .Mutator<Const>(c => c.Value += 1);

      var result = transformer.Transform<Addition>(expr, TransformationStrategy.NONE);
      
      Assert.IsTrue(exceptionCatched);
    }

    [TestMethod]
    public void ExtensionTests_EachAs() {
      var expr = LAddition(LConst(1), LConst(4));
      Assert.AreEqual("1 + 4", expr.ToString());

      var transformer = new Transformer()
        .Outputter<LAddition>((input, output) => output.Expressions.AddRange(input.Expressions.EachAs<LExpr>()))
        .Mutator<LConst>(c => c.Value += 1);

      var result = transformer.Transform<LExpr>(expr, TransformationStrategy.NONE);

      Assert.AreEqual("2 + 5", result.ToString());
    }

    [TestMethod]
    public void ExtensionTests_EachAsAction() {
      var expr = LAddition(LConst(1), LConst(4));
      Assert.AreEqual("1 + 4", expr.ToString());

      var transformer = new Transformer()
        .Outputter<LAddition, LAddition>((input, output) =>
          input.Expressions.EachAs<LConst>(e => {
            e.Value += 1;            
            output.Expressions.Add(e);
          }
        ));

      var result = transformer.Transform<LExpr>(expr, TransformationStrategy.NONE);

      Assert.AreEqual("2 + 5", result.ToString());
    }

    [TestMethod]
    public void ExtensionTests_EachAsIgnoreNull() {
      var expr = LAddition(LConst(1), LVar("x"));
      Assert.AreEqual("1 + x", expr.ToString());

      var transformer = new Transformer()
        .Outputter<LAddition>((input, output) => {
          output.Expressions.AddRange(input.Expressions.EachAs<LConst>()); //should only add the LConst
        })
        .Mutator<LConst>(c => c.Value += 1);

      var result = transformer.Transform<LExpr>(expr, TransformationStrategy.NONE);

      Assert.AreEqual("2", result.ToString());
    }

    [TestMethod]
    public void ExtensionTests_EachAsActionIgnoreNull() {
      var expr = LAddition(LConst(1), LVar("x"));
      Assert.AreEqual("1 + x", expr.ToString());

      var transformer = new Transformer()
        .Outputter<LAddition, LAddition>((input, output) =>
          input.Expressions.EachAs<LConst>(e => { //should skip action for the LVar
            e.Value += 1;
            output.Expressions.Add(e);
          }
        ));

      var result = transformer.Transform<LExpr>(expr, TransformationStrategy.NONE);

      Assert.AreEqual("2", result.ToString());
    }

    [TestMethod]
    public void ExtensionTests_AddAs() {
      var expr = LAddition(LConst(1), LConst(4));
      Assert.AreEqual("1 + 4", expr.ToString());

      var transformer = new Transformer()
        .Outputter<LAddition>((input, output) => {
          output.Expressions.AddAs<LExpr>(input.Expressions[0]);
          output.Expressions.AddAs<LExpr>(input.Expressions[1]);
        })
        .Mutator<LConst>(c => c.Value += 1);

      var result = transformer.Transform<LExpr>(expr, TransformationStrategy.NONE);

      Assert.AreEqual("2 + 5", result.ToString());
    }

    [TestMethod]
    public void ExtensionTests_AddEachAs() {
      var expr = LAddition(LConst(1), LConst(4));
      Assert.AreEqual("1 + 4", expr.ToString());

      var transformer = new Transformer()
        .Outputter<LAddition>((input, output) => output.Expressions.AddEachAs<LExpr>(input.Expressions))
        .Mutator<LConst>(c => c.Value += 1);

      var result = transformer.Transform<LExpr>(expr, TransformationStrategy.NONE);

      Assert.AreEqual("2 + 5", result.ToString());
    }

    [TestMethod]
    public void ExtensionTests_AddEachAsIgnoreNull() {
      var expr = LAddition(LConst(1), LVar("x"));
      Assert.AreEqual("1 + x", expr.ToString());

      var transformer = new Transformer()
        .Outputter<LAddition>((input, output) => output.Expressions.AddEachAs<LConst>(input.Expressions)) //should only add the LConst
        .Mutator<LConst>(c => c.Value += 1);

      var result = transformer.Transform<LExpr>(expr, TransformationStrategy.NONE);

      Assert.AreEqual("2", result.ToString());
    }
  }
}
