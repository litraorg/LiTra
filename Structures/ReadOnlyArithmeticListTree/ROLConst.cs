namespace Structures.ReadOnlyArithmeticListTree {
  public class ROLConst : ROLExpr {
    public int Value { get; set; }

    public ROLConst() { }

    public ROLConst(int value) {
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
