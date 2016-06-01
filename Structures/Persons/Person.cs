using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.TestStructures.Persons {  
  public class Person {
    public string Name { get; set; }
    public Gender Gender { get; set; }
    public Person Spouse { get; set; }

    public override string  ToString() {
      return $"{Name} - {Spouse.Name}";
    }
  }
}
