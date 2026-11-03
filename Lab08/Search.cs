using System;

namespace Lab08
{
    public static class Search
    {
        // 1. Лінійний пошук з лічильником порівнянь
        public static int LinearSearch<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
        {
            comparisons = 0;
            for (int i = 0; i < a.Length; i++)
            {
                comparisons++;
                if (cmp(a[i], target) == 0)
                {
                    return i;
                }
            }
            return -1;
        }

        // 2. Стандартний класичний бінарний пошук
        public static int BinarySearch<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
        {
            comparisons = 0;
            int lo = 0;
            int hi = a.Length - 1;

            while (lo <= hi)
            {
                int mid = lo + (hi - lo) / 2;
                comparisons++;
                int res = cmp(a[mid], target);

                if (res == 0)
                {
                    return mid;
                }

                comparisons++;
                if (res < 0)
                {
                    lo = mid + 1;
                }
                else
                {
                    hi = mid - 1;
                }
            }
            return -1;
        }

        // 3. Універсальний метод пошуку межі: повертає індекс першого елемента, що задовольняє умову
        public static int FindFirst<T>(T[] a, Func<T, bool> predicate, out int comparisons)
        {
            comparisons = 0;
            int lo = 0;
            int hi = a.Length;

            while (lo < hi)
            {
                int mid = lo + (hi - lo) / 2;
                comparisons++;
                if (predicate(a[mid]))
                {
                    hi = mid;
                }
                else
                {
                    lo = mid + 1;
                }
            }
            return lo;
        }

        // 4. Варіант c = 1: Бінарний пошук без перевірки на рівність у циклі
        public static int BinarySearchNoEqInLoop<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
        {
            comparisons = 0;
            if (a.Length == 0) return -1;

            int lo = 0;
            int hi = a.Length - 1;

            while (lo < hi)
            {
                int mid = lo + (hi - lo) / 2;
                comparisons++;
                if (cmp(a[mid], target) < 0)
                {
                    lo = mid + 1;
                }
                else
                {
                    hi = mid;
                }
            }

            comparisons++;
            if (cmp(a[lo], target) == 0)
            {
                return lo;
            }

            return -1;
        }
    }
}