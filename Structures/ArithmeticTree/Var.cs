namespace Structures.ArithmeticTree {
  public class Var : Expr {
    public string Name { get; set; }

    public Var() { }

    public Var(string name) {
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
