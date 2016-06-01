using System.Text;

namespace Structures.ArithmeticTree {
  public class AppendableVar : Expr {
    public StringBuilder Name { get; set; }

    public AppendableVar() { }

    public AppendableVar(StringBuilder value) {
      Name = value;
    }

    public void Append(string value) {
      Name.Append(value);
    }

    internal override string ToNestedString() {
      return ToString();
    }

    public override string ToString() {
      return $"{Name}";
    }
  }
}
