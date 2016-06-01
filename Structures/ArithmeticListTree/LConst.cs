namespace Structures.ArithmeticListTree {
  public class LConst : LExpr {
    public int Value { get; set; }

    public LConst() { }

    public LConst(int value) {
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
