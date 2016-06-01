using Microsoft.VisualStudio.TestTools.UnitTesting;
using LiTra.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.TestStructures.Persons;

namespace Tests.FunctionalityTests.TransformerTests {
  [TestClass]
  public class CycleTests {

    [TestMethod]
    public void CycleTests_Caching() {
      var personRoot = GetPersonRoot();
      Assert.AreEqual("Catherine - Carl, Lisa - William, Carl - Catherine, William - Lisa", personRoot.ToString());

      var transformer = new Transformer()
          .Mutator<Person>(
            input => input.Gender == Gender.FEMALE,
            input => input.Name = "Ms. " + input.Name
          )
          .Mutator<Person>(
            input => input.Gender == Gender.MALE,
            input => input.Name = "Mr. " + input.Name
           );

      var result = transformer.Transform<PersonRoot>(personRoot, TransformationStrategy.TOP_DOWN);
      Assert.AreEqual("Ms. Catherine - Mr. Carl, Ms. Lisa - Mr. William, Mr. Carl - Ms. Catherine, Mr. William - Ms. Lisa", personRoot.ToString());
    }

    [TestMethod]
    public void CycleTests_CorrectBottomInBottomUp() {
      var personRoot = GetPersonRoot();
      Assert.AreEqual("Catherine - Carl, Lisa - William, Carl - Catherine, William - Lisa", personRoot.ToString());

      int nextId = 1;

      var transformer = new Transformer()
          .Mutator<Person>(
            input => input.Gender == Gender.FEMALE,
            input => input.Name = nextId++.ToString()
          )
          .Mutator<Person>(
            input => input.Gender == Gender.MALE,
            input => input.Name = nextId++.ToString()
           );

      var result = transformer.Transform<PersonRoot>(personRoot, TransformationStrategy.BOTTOM_UP);
      Assert.AreEqual("2 - 1, 4 - 3, 1 - 2, 3 - 4", personRoot.ToString());
    }

    private PersonRoot GetPersonRoot() {
      var root = new PersonRoot();

      var catherine = new Person { Name = "Catherine", Gender = Gender.FEMALE };
      var lisa = new Person { Name = "Lisa", Gender = Gender.FEMALE };
      var carl = new Person { Name = "Carl", Spouse = catherine, Gender = Gender.MALE };
      var william = new Person { Name = "William", Spouse = lisa, Gender = Gender.MALE };
      catherine.Spouse = carl;
      lisa.Spouse = william;

      root.People = new List<Person> { catherine, lisa, carl, william };
      return root;
    }
  }
}
