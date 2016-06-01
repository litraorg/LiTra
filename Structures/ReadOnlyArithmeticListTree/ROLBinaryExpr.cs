using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures.ReadOnlyArithmeticListTree {
  public abstract class ROLBinaryExpr : ROLExpr {
    public ReadOnlyCollection<ROLExpr> Expressions { get; set; }
  }
}
