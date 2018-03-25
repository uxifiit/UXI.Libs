using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class IEnumerableEx
    {
        public static IEnumerable<TSource> TakeUntil<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source != null)
            {
                foreach (TSource item in source)
                {
                    yield return item;
                    if (predicate(item))
                    {
                        yield break;
                    }
                }
            }
        }


        public static IEnumerable<TSource> Execute<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source != null)
            {
                foreach (TSource item in source)
                {
                    action.Invoke(item);
                    yield return item;
                }
            }
        }


        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source != null)
            {
                foreach (TSource item in source)
                {
                    action.Invoke(item);
                }
            }
        }


        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T item)
        {
            yield return item;
            if (source != null)
            {
                foreach (var element in source)
                {
                    yield return element;
                }
            }
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T item)
        {
            if (source != null)
            {
                foreach (var element in source)
                {
                    yield return element;
                }
            }

            yield return item;
        }


        public static IEnumerable<TResult> TrySelect<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, Func<TSource, Exception, TResult> handler)
        {
            if (source != null)
            {
                foreach (var element in source)
                {
                    TResult result;
                    try
                    {
                        result = selector.Invoke(element);
                    }
                    catch (Exception ex)
                    {
                        result = handler.Invoke(element, ex);
                    }

                    yield return result;
                }
            }
        }


        public static bool TryGet<T>(this IEnumerable<T> source, Func<T, bool> selector, out T value)
        {
            value = default(T);
            if (source != null)
            {
                foreach (var element in source)
                {
                    if (selector.Invoke(element))
                    {
                        value = element;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
