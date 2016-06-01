using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.TestStructures.Persons {
  public class PersonRoot {
    public List<Person> People { get; set; }

    public override string ToString() {
      return string.Join(", ", People);
    }
  }
}
