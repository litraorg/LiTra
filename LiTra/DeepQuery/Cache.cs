using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiTra.DeepQuery {
  public static class Cache {
    internal static readonly Dictionary<object, Dictionary<Type, object>> cache = new Dictionary<object, Dictionary<Type, object>>();

    public static void ClearCache() {
      cache.Clear();
    }

    public static bool TryGetCachedResult<T>(object input, out DeepQueryResult<T> result) {
      Dictionary<Type, object> innerDict;
      if (cache.TryGetValue(input, out innerDict)) {
        object cachedResult;
        if (innerDict.TryGetValue(typeof(T), out cachedResult)) {
          result = (DeepQueryResult<T>)cachedResult;
          return true;
        }
      }
      result = null;
      return false;
    }

    public static void AddResult<T>(object input, DeepQueryResult<T> result) {
      Dictionary<Type, object> innerDict;
      if (!cache.TryGetValue(input, out innerDict)) {
        innerDict = new Dictionary<Type, object>();
        cache[input] = innerDict;
      }
      innerDict[typeof(T)] = result;
    }
  }
}
