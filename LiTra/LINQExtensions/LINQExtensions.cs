using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiTra.LINQExtensions {
  public static class LINQExtensions {
    public static void Each<TSource>(this IEnumerable<TSource> source, Action<TSource> action) {
      if (ReferenceEquals(source, null)) throw new NullReferenceException();
      if (ReferenceEquals(action, null)) throw new ArgumentNullException();
      foreach (var s in source) action(s);
    }

    //Exceptions:
    //   T:System.ArgumentNullException:
    //     otherCollection is null.
    public static void AddRange<T, U>(this ICollection<T> collection, IEnumerable<U> otherCollection) where U : T {
      if (ReferenceEquals(collection, null)) throw new NullReferenceException();
      if (ReferenceEquals(otherCollection, null)) throw new ArgumentNullException();
      foreach (var i in otherCollection) collection.Add(i);
    }
  }
}