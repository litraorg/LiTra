using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures.ArithmeticListTree {
  public abstract class LBinaryExpr : LExpr {
    public List<LExpr> Expressions { get; set; }
  }
}
