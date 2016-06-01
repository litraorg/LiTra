using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiTra.Transformation {
  [Flags]
  public enum TransformationStrategy {
    NONE = 0, BOTTOM_UP = 1, TOP_DOWN = 2, REPEAT = 4
  }
}
