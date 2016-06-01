using LiTra.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LiTra.Transformation {
  public static class TransformationExtensions {
    private static Dictionary<int, Transformer> transformers = new Dictionary<int, Transformer>();

    internal static void RegisterTransformer(Transformer transformer) {
      var threadId = Thread.CurrentThread.ManagedThreadId;     
      transformers[threadId] = transformer;
    }

    internal static void DeregisterTransformer() {
      transformers.Remove(Thread.CurrentThread.ManagedThreadId);
    }

    private static Transformer getTransformer() {
      var threadId = Thread.CurrentThread.ManagedThreadId;
      Transformer transformer;
      if (transformers.TryGetValue(threadId, out transformer)) {
        return transformer;
      } else {
        throw new ResolveOutsideTransformationRuleException();
      }
    }

    private static O transform<O>(object input) {
      var transformer = getTransformer();
      return transformer.Resolve<O>(input, transformer.strategy & ~TransformationStrategy.REPEAT);
    }

    private static IEnumerable<O> transformEach<O>(IEnumerable<object> input) {
      var transformer = getTransformer();
      var list = new List<O>();
      foreach (var i in input) {
        var result = transformer.Resolve<O>(i, transformer.strategy & ~TransformationStrategy.REPEAT);
        if (!ReferenceEquals(result, null))
          list.Add(result);
      }
      return list;
    }

    public static T As<T>(this object input) {
      if (ReferenceEquals(input, null)) throw new NullReferenceException();
      return transform<T>(input);
    }

    public static IEnumerable<T> EachAs<T>(this IEnumerable<object> input) {
      if (ReferenceEquals(input, null)) throw new NullReferenceException();
      return transformEach<T>(input);
    }

    public static void EachAs<T>(this IEnumerable<object> input, Action<T> action) {
      if (ReferenceEquals(input, null)) throw new NullReferenceException();
      if (ReferenceEquals(action, null)) throw new ArgumentNullException();
      foreach (var i in transformEach<T>(input)) {
        action(i);
      }        
    }

    public static void AddAs<T>(this IList list, object input) {
      if (ReferenceEquals(list, null)) throw new NullReferenceException();
      list.Add(transform<T>(input));
    }

    public static void AddEachAs<T>(this IList list, IEnumerable<object> input) {
      if (ReferenceEquals(list, null)) throw new NullReferenceException();
      if (ReferenceEquals(input, null)) throw new ArgumentNullException();
      foreach (var output in transformEach<T>(input)) {
        list.Add(output);
      }
    }
  }
}
