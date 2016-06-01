namespace Structures.ReadOnlyArithmeticListTree {
  public class ROLVar : ROLExpr {
    public string Name { get; set; }

    public ROLVar() { }

    public ROLVar(string name) {
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
