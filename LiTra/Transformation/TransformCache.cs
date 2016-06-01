using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiTra.Transformation {
  internal class TransformCache {
    private Dictionary<object, List<Tuple<Type, object>>> _cache;

    internal TransformCache() {
      _cache = new Dictionary<object, List<Tuple<Type, object>>>();
    }

    internal bool TryGetValueOrInitialize(object key, Type outputType, out object value) {
      List<Tuple<Type, object>> list;
      if (_cache.TryGetValue(key, out list)) {
        foreach (var tuple in list) {
          if (outputType.IsAssignableFrom(tuple.Item1)) {
            value = tuple.Item2;
            return true;
          }
        }
      } else {
        _cache[key] = new List<Tuple<Type, object>>();
      }
      value = null;
      return false;
    }

    internal void AddValue(object key, Type outputType, object value) {
      _cache[key].Add(Tuple.Create(outputType, value));      
    }

    internal void Clear() {
      _cache.Clear();
    }
  }
}
