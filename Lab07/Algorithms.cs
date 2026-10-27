using System;
using System.Collections.Generic;

namespace Lab07
{
    public static class Algorithms
    {
        // Завдання 1: Відбір за критерієм
        public static List<T> Filter<T>(List<T> src, Func<T, bool> keep)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < src.Count; i++)
            {
                if (keep(src[i]))
                {
                    result.Add(src[i]);
                }
            }
            return result;
        }

        // Завдання 2: Перетворення
        public static List<TOut> Map<TIn, TOut>(List<TIn> src, Func<TIn, TOut> convert)
        {
            List<TOut> result = new List<TOut>(src.Count);
            for (int i = 0; i < src.Count; i++)
            {
                result.Add(convert(src[i]));
            }
            return result;
        }

        // Завдання 3: Узагальнена агрегація
        public static TAcc Aggregate<T, TAcc>(List<T> src, TAcc start, Func<TAcc, T, TAcc> step)
        {
            TAcc state = start;
            for (int i = 0; i < src.Count; i++)
            {
                state = step(state, src[i]);
            }
            return state;
        }

        // Завдання 4: Узагальнене групування (додано обмеження where TKey : notnull)
        public static Dictionary<TKey, List<T>> GroupBy<T, TKey>(List<T> src, Func<T, TKey> keyOf) where TKey : notnull
        {
            Dictionary<TKey, List<T>> groups = new Dictionary<TKey, List<T>>();
            for (int i = 0; i < src.Count; i++)
            {
                T item = src[i];
                TKey key = keyOf(item);

                if (!groups.ContainsKey(key))
                {
                    groups[key] = new List<T>();
                }
                groups[key].Add(item);
            }
            return groups;
        }
    }
}