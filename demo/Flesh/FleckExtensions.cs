using System;
using System.Collections.Generic;


namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method,
        Inherited = false, AllowMultiple = false)]
    public class ExtensionAttribute : Attribute
    {
    }
}


namespace Fleck2
{

    /// <summary>
    /// Since Fleck is targeting NET40 we need to add workarounds for
    /// getting this awesome library running on older CLR's. 
    /// </summary>
    public static class Fleck2Extensions
    {

        #region Action, Func
        public delegate void Action();
        public delegate void Action<in T>(T t);
        public delegate void Action<in T, in TU>(T t, TU u);
        public delegate void Action<in T, in TU, in TV>(T t, TU u, TV v);
        public delegate TResult Func<out TResult>();
        public delegate TResult Func<in T, out TResult>(T t);
        public delegate TResult Func<in T, in TU, out TResult>(T t, TU u);
        public delegate TResult Func<in T, in TU, in TV, out TResult>(T t, TU u, TV v);
        #endregion

        #region Miscellaneous
        /// <summary>
        /// Convert enumerable to array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static T[] ToArray<T>(this IEnumerable<T> enumerable)
        {
            return ToList(enumerable).ToArray();
        }
        /// <summary>
        /// Convert enumerable to list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this IEnumerable<T> enumerable)
        {
            var tmp = new List<T>();
            foreach (var value in enumerable)
            {
                tmp.Add(value);
            }
            return tmp;
        }
        /// <summary>
        /// Returns a specified number of contiguous elements from 
        /// the start of a sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> Skip<T>(this IEnumerable<T> enumerable, int count)
        {
            foreach(var value in enumerable)
            {
                if(count-- > 0)
                {
                    continue;
                }
                yield return value;
            }
        }
        /// <summary>
        /// Bypasses a specified number of elements in a sequence and
        /// then returns the remaining elements.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static byte[] Skip(this byte[] array, int count)
        {
            var newArray = new byte[array.Length - count];
            var n = 0;
            for(var i = 0; i < array.Length; i++)
            {
                if (count-- > 0)
                {
                    continue;
                }
                newArray[n] = array[i];
                n++;
            }
            return newArray;
        }
        /// <summary>
        /// Returns a specified number of contiguous elements from 
        /// the start of a sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> Take<T>(this IEnumerable<T> enumerable, int count)
        {
            foreach (var value in enumerable)
            {
                if (count-- > 0)
                {
                    yield return value;
                }
                else
                {
                    yield break;
                }
            }
        }
        /// <summary>
        /// Bypasses a specified number of elements in a sequence and
        /// then returns the remaining elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> Skip<T>(this T[] enumerable, int count)
        {
            foreach (var value in enumerable)
            {
                if (count-- > 0)
                {
                    yield return value;
                }
                else
                {
                    break;
                }
            }
        }
        /// <summary>
        /// Returns a specified number of contiguous elements from 
        /// the start of a sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> Take<T>(this T[] enumerable, int count)
        {
            foreach (var value in enumerable)
            {
                if (count-- > 0)
                {
                    yield return value;
                }
                else
                {
                    break;
                }
            }
        }
        /// <summary>
        /// Project over a sequence of values.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> enumerable, 
                                                                    Converter<TSource, TResult> selector)
        {
            foreach (var newValue in ToList(enumerable).ConvertAll(selector))
            {
                yield return newValue;
            }
        }
        #endregion

    }
}