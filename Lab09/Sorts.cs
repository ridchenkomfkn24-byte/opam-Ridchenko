using System;
using System.Collections.Generic;

namespace Lab09
{
    public static class Sorts
    {
        // 1. Перевірка впорядкованості
        public static bool IsSorted<T>(T[] a, Func<T, T, int> cmp)
        {
            for (int i = 0; i < a.Length - 1; i++)
            {
                if (cmp(a[i], a[i + 1]) > 0)
                {
                    return false;
                }
            }
            return true;
        }

        // 2. Перевірка збереження складу елементів
        public static bool IsPermutationOf<T>(T[] original, T[] sorted, Func<T, T, int> cmp) where T : notnull
        {
            if (original.Length != sorted.Length) return false;

            Dictionary<T, int> counts = new Dictionary<T, int>();

            for (int i = 0; i < original.Length; i++)
            {
                T item = original[i];
                if (counts.ContainsKey(item)) counts[item]++;
                else counts[item] = 1;
            }

            for (int i = 0; i < sorted.Length; i++)
            {
                T item = sorted[i];
                if (!counts.ContainsKey(item) || counts[item] == 0)
                {
                    return false;
                }
                counts[item]--;
            }

            return true;
        }

        // --- Завдання 1: Сортування вибором (a = 0) ---
        public static void SelectionSort<T>(T[] a, Func<T, T, int> cmp, out long comparisons, out long moves)
        {
            comparisons = 0;
            moves = 0;
            int n = a.Length;

            for (int i = 0; i < n - 1; i++)
            {
                int minIdx = i;
                for (int j = i + 1; j < n; j++)
                {
                    comparisons++;
                    if (cmp(a[j], a[minIdx]) < 0)
                    {
                        minIdx = j;
                    }
                }

                if (minIdx != i)
                {
                    T temp = a[i];
                    a[i] = a[minIdx];
                    a[minIdx] = temp;
                    moves += 2;
                }
            }
        }

        // --- Завдання 2: Сортування злиттям знизу вгору, без рекурсії (b = 1) ---
        public static void BottomUpMergeSort<T>(T[] a, Func<T, T, int> cmp, out long comparisons, out long moves)
        {
            comparisons = 0;
            moves = 0;
            int n = a.Length;
            if (n <= 1) return;

            T[] temp = new T[n];

            for (int width = 1; width < n; width *= 2)
            {
                for (int i = 0; i < n; i += 2 * width)
                {
                    int left = i;
                    int mid = Math.Min(i + width, n);
                    int right = Math.Min(i + 2 * width, n);

                    int p = left;
                    int q = mid;
                    int k = left;

                    while (p < mid && q < right)
                    {
                        comparisons++;
                        if (cmp(a[p], a[q]) <= 0)
                        {
                            temp[k++] = a[p++];
                            moves++;
                        }
                        else
                        {
                            temp[k++] = a[q++];
                            moves++;
                        }
                    }

                    while (p < mid)
                    {
                        temp[k++] = a[p++];
                        moves++;
                    }
                    while (q < right)
                    {
                        temp[k++] = a[q++];
                        moves++;
                    }

                    for (int j = left; j < right; j++)
                    {
                        a[j] = temp[j];
                        moves++;
                    }
                }
            }
        }

        // --- Завдання 3: Сортування вставками з бінарним пошуком позиції (c = 1) ---
        public static void BinaryInsertionSort<T>(T[] a, Func<T, T, int> cmp, out long comparisons, out long moves)
        {
            comparisons = 0;
            moves = 0;
            int n = a.Length;

            for (int i = 1; i < n; i++)
            {
                T key = a[i];

                int lo = 0;
                int hi = i;

                while (lo < hi)
                {
                    int mid = lo + (hi - lo) / 2;
                    comparisons++;
                    if (cmp(a[mid], key) > 0)
                    {
                        hi = mid;
                    }
                    else
                    {
                        lo = mid + 1;
                    }
                }

                for (int j = i; j > lo; j--)
                {
                    a[j] = a[j - 1];
                    moves++;
                }

                a[lo] = key;
                moves++;
            }
        }
    }
}