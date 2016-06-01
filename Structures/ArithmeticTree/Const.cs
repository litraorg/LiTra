using System.Collections.Generic;

namespace Structures.ArithmeticTree {
  public class Const : Expr {
    public int Value
    {
      get; set;
    }

    public Const() { }

    public Const(int value) {
      Value = value;
    }

    internal override string ToNestedString() {
      return ToString();
    }

    public override string ToString() {
      return $"{Value}";
    }    
  }
}
