namespace Structures.ArithmeticTree {
  public abstract class BinaryExpr : Expr {
    public Expr Expr1 { get; set; }
    public Expr Expr2;      
  }
}
