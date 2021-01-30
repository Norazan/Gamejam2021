using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class Extensions
    {
        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            var index = UnityEngine.Random.Range(0, enumerable.Count());
            return enumerable.ElementAt(index);
        }
    }    
}
