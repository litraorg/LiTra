using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures.BooleanTree {
  public class BVar : BooleanExpr {
    public string Name { get; set; }

    public BVar() {

    }
    public BVar(string name) {
      Name = name;
    }

    internal override string ToNestedString() {
      return ToString();
    }

    public override string ToString() {
      return $"{Name}";
    }
  }
}