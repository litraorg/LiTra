using System.Linq;
using LiTra.Transformation;
using LiTra.LINQExtensions;
using Ps = NMF.Transformations.Example.Persons;
using Fam = NMF.Transformations.Example.FamilyRelations;

namespace CaseStudies.Case1.LiTra {
  public class PersonsToFamilyRelationsLiTra {
    public static Fam.Root Transform(Ps.Root root) { 
        var firstTransformer = new Transformer()
          .Outputter<Ps.Root, Fam.Root>((input, output) =>
            output.People.AddRange(input.Persons.EachAs<Fam.Person>())
          )
          .Outputter<Ps.Person, Fam.Female>(
            input => input.Gender == Ps.Gender.Female,
            (input, output) => {
              output.Husband = input.Spouse?.As<Fam.Male>();
              input.Children.EachAs<Fam.Person>(child => child.Mother = output);
            }
          )
          .Outputter<Ps.Person, Fam.Male>(
            input => input.Gender == Ps.Gender.Male,
            (input, output) => {
              output.Wife = input.Spouse?.As<Fam.Female>();
              input.Children.EachAs<Fam.Person>(child => child.Father = output);
            }
          )
          .Outputter<Ps.Person, Fam.Person>((input, output) => {
            output.LastName = input.Name;
            output.FirstName = input.FirstName;
            var daughters = input.Children.Where(child => child.Gender == Ps.Gender.Female).EachAs<Fam.Female>();
            var sons = input.Children.Where(child => child.Gender == Ps.Gender.Male).EachAs<Fam.Male>();
            daughters.Each(daughter => {
              output.Daughters.Add(daughter);
              daughter.Sisters.AddRange(daughters.Except(new Fam.Female[] { daughter }));
              daughter.Brothers.AddRange(sons);
            });
            sons.Each(son => {
              output.Sons.Add(son);
              son.Sisters.AddRange(daughters);
              son.Brothers.AddRange(sons.Except(new Fam.Male[] { son }));
            });
          });
          //Define a second in-place transformation to add aunts and uncles
          var secondTransformer = new Transformer()
            .Mutator<Fam.Person>((input) => {
              if (input.Father != null) {
                input.Uncles.AddRange(input.Father.Brothers);
                input.Aunts.AddRange(input.Father.Sisters);
              }
              if (input.Mother != null) {
                input.Uncles.AddRange(input.Mother.Brothers);
                input.Aunts.AddRange(input.Mother.Sisters);
              }
            });
          var firstresult = firstTransformer.Transform<Fam.Root>(root);     
          return secondTransformer.Transform<Fam.Root>(firstresult, TransformationStrategy.BOTTOM_UP);
    }
  }
}