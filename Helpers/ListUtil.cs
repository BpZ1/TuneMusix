using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneMusix.Helpers
{
    public static class ListUtil
    {
        /// <summary>
        /// Splits a given list into parts of the given size.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IEnumerable<List<T>> SplitList<T>(List<T> list, int size)
        {
            for (int i = 0; i < list.Count; i += size)
            {
                yield return list.GetRange(i, Math.Min(size, list.Count - i));
            }
        }
    }
}
