namespace Structures.ArithmeticListTree {
  public abstract class LExpr{

    internal virtual string ToNestedString() {
      return $"({this})";
    }

    public abstract override string ToString();
  }
}
