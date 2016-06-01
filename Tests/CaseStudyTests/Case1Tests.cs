using System.Linq;
using Ps = NMF.Transformations.Example.Persons;
using Fam = NMF.Transformations.Example.FamilyRelations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Transformations;
using NMF.Transformations.Example;
using CaseStudies.Case1.Baseline;
using CaseStudies.Case1.LiTra;

namespace Tests.EvaluationTests {
  [TestClass]
  public class Case1Tests {
    private static Ps.Root CreateTestPersonsRoot() {
      var root = new Ps.Root();
      var granddad = new Ps.Person() { FirstName = "Søren", Name = "Hansen", Gender = Ps.Gender.Male };

      var dad = new Ps.Person() { FirstName = "John", Name = "Hansen", Gender = Ps.Gender.Male };
      var uncle = new Ps.Person() { FirstName = "Ole", Name = "Hansen", Gender = Ps.Gender.Male };
      var aunt = new Ps.Person() { FirstName = "Kirsten", Name = "Hansen", Gender = Ps.Gender.Female };

      granddad.Children.Add(dad);
      granddad.Children.Add(uncle);
      granddad.Children.Add(aunt);

      var mom = new Ps.Person() { FirstName = "Ulla", Name = "Hansen", Gender = Ps.Gender.Female };
      dad.Spouse = mom;
      mom.Spouse = dad;

      var son = new Ps.Person() { FirstName = "Peter", Name = "Hansen", Gender = Ps.Gender.Male };
      dad.Children.Add(son);
      mom.Children.Add(son);

      var daughter = new Ps.Person() { FirstName = "Pia", Name = "Hansen", Gender = Ps.Gender.Female };
      dad.Children.Add(daughter);
      mom.Children.Add(daughter);

      root.Persons.Add(granddad);
      root.Persons.Add(mom);
      root.Persons.Add(dad);
      root.Persons.Add(uncle);
      root.Persons.Add(aunt);

      root.Persons.Add(son);
      root.Persons.Add(daughter);

      return root;
    }

    [TestMethod]
    public void Case1Test() {
      var input = CreateTestPersonsRoot();
      var baselineResult = new PersonsToFamilyRelationsBaseline().Transform(input);
      var nmfResult = TransformationEngine.Transform<Ps.Root, Fam.Root>(input, new Persons2FamilyRelations());
      var litraResult = PersonsToFamilyRelationsLiTra.Transform(input);

      Assert.AreEqual(baselineResult.People.Count(), litraResult.People.Count());
      Assert.AreEqual(nmfResult.People.Count(), litraResult.People.Count());

      Assert.AreEqual("Ole", baselineResult.People.Where(p => p.FirstName == "Peter").First().Uncles.First().FirstName);
      Assert.AreEqual("Ole", nmfResult.People.Where(p => p.FirstName == "Peter").First().Uncles.First().FirstName);
      Assert.AreEqual("Ole", litraResult.People.Where(p => p.FirstName == "Peter").First().Uncles.First().FirstName);
    }
  }
}
