using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class Extensions
    {
        private static readonly Random rng = new Random();

        public static IEnumerable<T> RandomMany<T>(this IEnumerable<T> enumerable, int number)
        {
            for(int _ = 0; _ < number; _++)
            {
                yield return enumerable.Random();
            }
        }

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.ElementAt(rng.Next(enumerable.Count()));
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count();
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static IEnumerable<Tuple<TFirst, TSecond>> Combine<TFirst, TSecond>(this IEnumerable<TFirst> s1, IEnumerable<TSecond> s2)
        {
            using var e1 = s1.GetEnumerator();
            using var e2 = s2.GetEnumerator();
            while (e1.MoveNext() && e2.MoveNext())
            {
                yield return new Tuple<TFirst, TSecond>(e1.Current, e2.Current);
            }
        }
    }

    public static class Helper
    {
        public static IEnumerable<T> GetValues<T>() where T : Enum
        {
            return (T[])Enum.GetValues(typeof(T));
        }
    }
}
