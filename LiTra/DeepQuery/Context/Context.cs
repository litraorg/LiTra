using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiTra.DeepQuery.Context {
  internal abstract class Context<T> {
    internal abstract void Mutate(Action<T> mutator);
    internal abstract void Transform<TResult>(Func<T, TResult> transformer);
  }
}
