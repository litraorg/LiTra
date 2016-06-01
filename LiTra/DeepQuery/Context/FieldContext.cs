using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LiTra.DeepQuery.Context {
  internal class FieldContext<T> : Context<T> {
    private object parent;
    private FieldInfo field;
    private T value;

    internal FieldContext(object parent, FieldInfo field, T value) {
      this.parent = parent;
      this.field = field;
      this.value = value;
    }

    internal override void Transform<TResult>(Func<T, TResult> transformer) {
      if (ReferenceEquals(field.GetValue(parent), value)) {
        var result = transformer(value);
        field.SetValue(parent, result);
      }      
    }

    internal override void Mutate(Action<T> mutator) {
      if (ReferenceEquals(field.GetValue(parent), value)) {
        mutator(value);
      }
    }
  }
}
