namespace Structures.ReadOnlyArithmeticListTree {
  public abstract class ROLExpr{

    internal virtual string ToNestedString() {
      return $"({this})";
    }

    public abstract override string ToString();
  }
}
