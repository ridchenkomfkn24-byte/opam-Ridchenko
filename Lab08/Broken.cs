using System;

namespace Lab08
{
    public static class Broken
    {
        // Поломка 1: while (lo < hi) замість lo <= hi
        public static int BinarySearch_Broken1<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
        {
            comparisons = 0;
            int lo = 0;
            int hi = a.Length - 1;

            while (lo < hi) // <--- ПОЛОМКА
            {
                int mid = lo + (hi - lo) / 2;
                comparisons++;
                int res = cmp(a[mid], target);

                if (res == 0) return mid;

                comparisons++;
                if (res < 0) lo = mid + 1;
                else hi = mid - 1;
            }
            return -1;
        }

        // Поломка 2: lo = mid замість mid + 1 (обмежено 100 ітераціями для уникнення завісання)
        public static int BinarySearch_Broken2<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons, out bool timedOut)
        {
            comparisons = 0;
            timedOut = false;
            int lo = 0;
            int hi = a.Length - 1;
            int iterations = 0;

            while (lo <= hi)
            {
                iterations++;
                if (iterations > 100)
                {
                    timedOut = true; // Фіксуємо завісання циклу
                    return -1;
                }

                int mid = lo + (hi - lo) / 2;
                comparisons++;
                int res = cmp(a[mid], target);

                if (res == 0) return mid;

                comparisons++;
                if (res < 0) lo = mid; // <--- ПОЛОМКА (зациклення при lo = mid)
                else hi = mid - 1;
            }
            return -1;
        }

        // Поломка 3: mid = (lo + hi) / 2 замість lo + (hi - lo) / 2
        public static int BinarySearch_Broken3<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
        {
            comparisons = 0;
            int lo = 0;
            int hi = a.Length - 1;

            while (lo <= hi)
            {
                int mid = (lo + hi) / 2; // <--- ПОЛОМКА (overflow при великих індексах)
                comparisons++;
                int res = cmp(a[mid], target);

                if (res == 0) return mid;

                comparisons++;
                if (res < 0) lo = mid + 1;
                else hi = mid - 1;
            }
            return -1;
        }

        // Поломка 4: Запуск бінарного пошуку на Невідсортованому масиві
        public static int TestUnsortedSearch(int[] unsortedArray, int[] targetSamples, Func<int, int, int> cmp)
        {
            int failedCount = 0;
            for (int i = 0; i < targetSamples.Length; i++)
            {
                int result = Search.BinarySearch(unsortedArray, targetSamples[i], cmp, out _);
                if (result == -1)
                {
                    failedCount++;
                }
            }
            return failedCount;
        }
    }
}