using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LiTra.DeepQuery {
  public static class DeepQueryExtensions {
    
    public static void Mutate<TSource>(this IEnumerable<TSource> source, DeepQueryResult<TSource> queryResult, Action<TSource> mutator) {
      queryResult.Mutate(source, mutator);
    }

    public static void Transform<TSource, TResult>(this IEnumerable<TSource> source, DeepQueryResult<TSource> queryResult, Func<TSource, TResult> transformer) {
      queryResult.Transform(source, transformer);
    }

    public static DeepQueryResult<T> DeepQuery<T>(this object input) {
      return input.DeepQueryRoot<T>();
    }

    private static DeepQueryResult<T> DeepQueryRoot<T>(this object input) {
      if (ReferenceEquals(input, null)) return new DeepQueryResult<T>();

      DeepQueryResult<T> result;
      if (Cache.TryGetCachedResult<T>(input, out result)) {
        return result;
      }

      result = new DeepQueryResult<T>();
      if (input is T) {
        result.AddValueWithoutContext((T)input);
      }        

      input.Descend(result);
      return result;
    }

    private static void Descend<T>(this object input, DeepQueryResult<T> result) {
      Cache.AddResult<T>(input, result);

      DeepQueryProperties(input, result);
      DeepQueryFields(input, result);
      DeepQueryListElements(input, result);
    }

    private static void DeepQueryProperties<T>(this object input, DeepQueryResult<T> result) {
      foreach (var property in input.GetType().GetProperties()) {
        if (property.GetIndexParameters().Length > 0) continue;
        result.MergeWith(DeepQueryProperty<T>(input, property));
      }
    }

    private static DeepQueryResult<T> DeepQueryProperty<T>(object parent, PropertyInfo property) {
      var value = property.GetValue(parent);
      if (ReferenceEquals(value, null)) return new DeepQueryResult<T>();

      DeepQueryResult<T> result;
      if (Cache.TryGetCachedResult<T>(value, out result)) {
        return result;
      }

      result = new DeepQueryResult<T>();      
      if (value is T) {
        result.AddPropertyContext(parent, property, (T)value);
      }        
      value.Descend(result);      
      return result;
    }

    private static void DeepQueryFields<T>(this object input, DeepQueryResult<T> result) {
      foreach (var field in input.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)) {
        result.MergeWith(DeepQueryField<T>(input, field));
      }
    }

    private static DeepQueryResult<T> DeepQueryField<T>(object parent, FieldInfo field) {
      var value = field.GetValue(parent);
      if (ReferenceEquals(value, null)) return new DeepQueryResult<T>();

      DeepQueryResult<T> result;
      if (Cache.TryGetCachedResult<T>(value, out result)) {
        return result;
      }

      result = new DeepQueryResult<T>();
      if (value is T) {
        result.AddFieldContext(parent, field, (T)value);
      }        

      value.Descend(result);
      return result;
    }

    private static void DeepQueryListElements<T>(this object input, DeepQueryResult<T> result) {
      if (input is IList) {
        IList parent = (IList)input;
        for (int i = 0; i < parent.Count; i++) {
          result.MergeWith(DeepQueryListElement<T>(parent, i));
        }
      }
    }

    private static DeepQueryResult<T> DeepQueryListElement<T>(IList parent, int index) {
      var value = parent[index];
      if (ReferenceEquals(value, null)) return new DeepQueryResult<T>();

      DeepQueryResult<T> result;
      if (Cache.TryGetCachedResult<T>(value, out result)) {
        return result;
      }

      result = new DeepQueryResult<T>();
      if (value is T) {
        result.AddListContext(parent, index, (T)value);
      }        

      value.Descend(result);
      return result;
    }


    private static void CollectLoop<T>(this object input, DeepQueryResult<T> result) {
      Queue<object> queue = new Queue<object>();
      queue.Enqueue(input);

      while (queue.Count > 0) {
        var parent = queue.Dequeue();
        if (parent is IList) {
          IList list = (IList)parent;
          for (int i = 0; i < list.Count; i++) {
            var value = list[i];
            if (value is T) {
              result.AddListContext(list, i, (T)value);
            }
            queue.Enqueue(value);
          }                     
        }
        foreach (var property in parent.GetType().GetProperties()) {
          if (property.GetIndexParameters().Length > 0) continue;
          var value = property.GetValue(parent);
          if (value is T) {
            result.AddPropertyContext(parent, property, (T)value);
          }
          queue.Enqueue(value);
        }
      };
    }
  }
}
