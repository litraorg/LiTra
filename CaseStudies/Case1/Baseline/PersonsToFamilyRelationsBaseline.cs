using System;
using System.Collections.Generic;
using System.Linq;
using Ps = NMF.Transformations.Example.Persons;
using Fam = NMF.Transformations.Example.FamilyRelations;

namespace CaseStudies.Case1.Baseline {
  public class PersonsToFamilyRelationsBaseline {
    private Dictionary<Ps.Person, Fam.Person> cache;
    private Dictionary<Ps.Person, Queue<Action<Fam.Person>>> savedActions;

    public PersonsToFamilyRelationsBaseline() {
      cache = new Dictionary<Ps.Person, Fam.Person>();
      savedActions = new Dictionary<Ps.Person, Queue<Action<Fam.Person>>>();
    }

    public Fam.Root Transform(Ps.Root input) {
      var output = new Fam.Root();
      foreach (var person in input.Persons) {
        output.People.Add(PersonToPerson(person));
      }
      foreach (var person in output.People) {
        AddAuntsAndUncles(person);
      }
      return output;
    }

    private Fam.Person PersonToPerson(Ps.Person input) {
      Fam.Person output;
      if (input.Gender == Ps.Gender.Male) {
        output = PersonToMale(input);
      } else {
        output = PersonToFemale(input);
      }
      output.LastName = input.Name;
      output.FirstName = input.FirstName;
      CallWhenTransformed(input.Children.Where(child => child.Gender == Ps.Gender.Female), uncastedDaughters => {
        CallWhenTransformed(input.Children.Where(child => child.Gender == Ps.Gender.Male), uncastedSons => {
          var daughters = uncastedDaughters.Select(d => (Fam.Female)d);
          var sons = uncastedSons.Select(s => (Fam.Male)s);
          foreach (var daughter in daughters) {
            output.Daughters.Add(daughter);
            foreach (var sister in daughters) {
              if (sister == daughter) continue;
              daughter.Sisters.Add(sister);
            }
            foreach (var brother in sons) {
              daughter.Brothers.Add(brother);
            }            
          }
          foreach (var son in sons) {
            output.Sons.Add(son);
            foreach (var sister in daughters) {
              son.Sisters.Add(sister);
            }
            foreach (var brother in sons) {
              if (brother == son) continue;
              son.Brothers.Add(brother);
            }
          }
        });
      });
      SaveToCache(input, output);
      return output;
    }

    private Fam.Female PersonToFemale(Ps.Person input) {
      var output = new Fam.Female();
      CallWhenTransformed(input.Spouse, h => output.Husband = (Fam.Male)h);
      foreach (var child in input.Children) {
        CallWhenTransformed(child, c => c.Mother = output);
      }
      return output;
    }

    private Fam.Male PersonToMale(Ps.Person input) {
      var output = new Fam.Male();
      CallWhenTransformed(input.Spouse, w => output.Wife = (Fam.Female)w);
      foreach (var child in input.Children) {
        CallWhenTransformed(child, c => c.Father = output);
      }
      return output;
    }

    private void AddAuntsAndUncles(Fam.Person person) {
      if (person.Father != null) {
        foreach (var uncle in person.Father.Brothers) {
          person.Uncles.Add(uncle);
        }
        foreach (var aunt in person.Father.Sisters) {
          person.Aunts.Add(aunt);
        }
      }
      if (person.Mother != null) {
        foreach (var uncle in person.Mother.Brothers) {
          person.Uncles.Add(uncle);
        }
        foreach (var aunt in person.Mother.Sisters) {
          person.Aunts.Add(aunt);
        }
      }
    }

    //Helper methods for caching
    private void SaveToCache(Ps.Person input, Fam.Person output) {
      cache[input] = output;
      Queue<Action<Fam.Person>> actions;
      if (savedActions.TryGetValue(input, out actions)) {
        foreach (var action in actions) {
          action(output);
        }
      }
    }

    private void CallWhenTransformed(IEnumerable<Ps.Person> persons, Action<IEnumerable<Fam.Person>> action) {
      var results = new List<Fam.Person>();
      var resultsLeft = persons.Count();
      foreach (var person in persons) {
        CallWhenTransformed(person, p => {
          results.Add(p);
          if (--resultsLeft == 0) {
            action(results);
          }
        });
      }
    }

    private void CallWhenTransformed(Ps.Person person, Action<Fam.Person> action) {
      if (ReferenceEquals(person, null)) return;
      Fam.Person result;
      if (cache.TryGetValue(person, out result)) {
        action(result);
      } else {
        InvokeLater(person, action);
      }
    }

    private void InvokeLater(Ps.Person person, Action<Fam.Person> action) {
      Queue<Action<Fam.Person>> actionQueue;
      if (!savedActions.TryGetValue(person, out actionQueue)) {
        actionQueue = new Queue<Action<Fam.Person>>();
        savedActions[person] = actionQueue;
      }
      actionQueue.Enqueue(action);
    }
  }
}