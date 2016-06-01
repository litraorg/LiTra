namespace Structures.ArithmeticListTree {
  public class LVar : LExpr {
    public string Name { get; set; }

    public LVar() { }

    public LVar(string name) {
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
