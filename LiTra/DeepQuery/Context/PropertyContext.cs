using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LiTra.DeepQuery.Context {
  internal class PropertyContext<T> : Context<T> {
    private object parent;
    private PropertyInfo property;
    private T value;

    internal PropertyContext(object parent, PropertyInfo property, T value) {
      this.parent = parent;
      this.property = property;
      this.value = value;
    }

    internal override void Transform<TResult>(Func<T, TResult> transformer) {
      if (ReferenceEquals(property.GetValue(parent), value)) {
        var result = transformer(value);
        property.SetValue(parent, result);
      }      
    }

    internal override void Mutate(Action<T> mutator) {
      if (ReferenceEquals(property.GetValue(parent), value)) {
        mutator(value);
      }
    }
  }
}
