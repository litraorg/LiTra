using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LiTra.DeepQuery.Context {
  internal class ListContext<T> : Context<T> {
    private IList list;
    private int index;
    private T value;

    internal ListContext(IList list, int index, T value) {
      this.list = list;
      this.index = index;
      this.value = value;
    }

    internal override void Transform<TResult>(Func<T, TResult> transformer) {
      if (ReferenceEquals(list[index], value)) {
        try {
          list[index] = transformer(value);
        } catch (NotSupportedException) { } //Is thrown if the list is unmodifiable
      }
    }

    internal override void Mutate(Action<T> mutator) {
      if (ReferenceEquals(list[index], value)) {
        mutator(value);
      }
    }
  }
}
