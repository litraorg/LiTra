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
using LiTra.LINQExtensions;

namespace Tests.FunctionalityTests.LINQExtensionTests {
  [TestClass]
  public class LINQExtensionTests {
    [TestMethod]
    public void LINQExtensionTests_Each() {
      IEnumerable<int> collection = new List<int> { 1, 2, 3 };
      Assert.AreEqual(3, collection.Count());
      Assert.IsTrue(collection.Contains(1));
      Assert.IsTrue(collection.Contains(2));
      Assert.IsTrue(collection.Contains(3));

      var result = new List<int>();
      collection.Each(i => result.Add(i));


      Assert.AreEqual(3, result.Count());
      Assert.IsTrue(result.Contains(1));
      Assert.IsTrue(result.Contains(2));
      Assert.IsTrue(result.Contains(3));
    }

    [TestMethod]
    public void LINQExtensionTests_EachAlsoAppliesToNull() {
      IEnumerable<string> collection = new List<string> { "Hello", null, "World" };
      Assert.AreEqual(3, collection.Count());
      Assert.IsTrue(collection.Contains("Hello"));
      Assert.IsTrue(collection.Contains(null));
      Assert.IsTrue(collection.Contains("World"));

      var result = new List<string>();
      collection.Each(s => result.Add(s));

      Assert.AreEqual(3, result.Count());
      Assert.IsTrue(result.Contains("Hello"));
      Assert.IsTrue(result.Contains(null));
      Assert.IsTrue(result.Contains("World"));
    }

    [TestMethod]
    public void LINQExtensionTests_EachEmpty() {
      IEnumerable<int> collection = new List<int>();
      Assert.AreEqual(0, collection.Count());

      var result = new List<int>();
      collection.Each(i => result.Add(i));

      Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void LINQExtensionTests_EachCalledOnNull() {
      IEnumerable<int> collection = null;
      Assert.IsNull(collection);

      var result = new List<int>();
      collection.Each(i => result.Add(i));      
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void LINQExtensionTests_EachCalledWithNull() {
      IEnumerable<int> collection = new List<int> { 1, 2, 3 };
      Assert.AreEqual(3, collection.Count());
      Assert.IsTrue(collection.Contains(1));
      Assert.IsTrue(collection.Contains(2));
      Assert.IsTrue(collection.Contains(3));

      Action<int> action = null;
      collection.Each(action);
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void LINQExtensionTests_EachBothNull() {
      IEnumerable<int> collection = null;
      Assert.IsNull(collection);

      Action<int> action = null;
      collection.Each(action);
    }

    [TestMethod]
    public void LINQExtensionTests_AddRange() {
      ICollection<int> result = new List<int> { 0 };
      Assert.AreEqual(1, result.Count());
      Assert.IsTrue(result.Contains(0));

      result.AddRange(new List<int> { 1, 2, 3 });

      Assert.AreEqual(4, result.Count());
      Assert.IsTrue(result.Contains(0));
      Assert.IsTrue(result.Contains(1));
      Assert.IsTrue(result.Contains(2));
      Assert.IsTrue(result.Contains(3));
    }

    [TestMethod]
    public void LINQExtensionTests_AddRangeAlsoAddsNull() {
      ICollection<string> result = new List<string> { "Hello" };
      Assert.AreEqual(1, result.Count());
      Assert.IsTrue(result.Contains("Hello"));

      result.AddRange(new List<string> { null, "world" });

      Assert.AreEqual(3, result.Count());
      Assert.IsTrue(result.Contains("Hello"));
      Assert.IsTrue(result.Contains(null));
      Assert.IsTrue(result.Contains("world"));
    }

    [TestMethod]
    public void LINQExtensionTests_AddRangeCalledWithEmpty() {
      ICollection<int> result = new List<int> { 0 };
      Assert.AreEqual(1, result.Count());
      Assert.IsTrue(result.Contains(0));

      var list = new List<int>();
      result.AddRange(list);

      Assert.AreEqual(1, result.Count());
      Assert.IsTrue(result.Contains(0));
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void LINQExtensionTests_AddRangeCalledOnNull() {
      ICollection<int> result = null;
      Assert.IsNull(result);

      result.AddRange(new List<int> { 1, 2, 3 });      
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void LINQExtensionTests_AddRangeCalledWithNull() {
      ICollection<int> result = new List<int>();
      Assert.AreEqual(0, result.Count());

      List<int> otherList = null;
      result.AddRange(otherList);
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void LINQExtensionTests_AddRangeBothNull() {
      ICollection<int> result = null;
      Assert.IsNull(result);

      List<int> otherList = null;
      result.AddRange(otherList);
    }
  }
}
