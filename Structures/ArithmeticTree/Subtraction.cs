namespace Structures.ArithmeticTree {
  public class Subtraction : BinaryExpr {

    public Subtraction() { }

    public Subtraction(Expr expr1, Expr expr2) {
      Expr1 = expr1;
      Expr2 = expr2;
    }

    public override string ToString() {
      return $"{Expr1?.ToNestedString()} - {Expr2?.ToNestedString()}";
    }
  }
}
