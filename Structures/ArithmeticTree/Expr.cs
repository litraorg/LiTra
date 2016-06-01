namespace Structures.ArithmeticTree {
  public abstract class Expr {

    internal virtual string ToNestedString() {
      return $"({this})";
    }

    public abstract override string ToString();
  }
}
