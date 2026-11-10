using System;
using System.Text;

namespace Lab09
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            int N = 21;
            int K = 17;

            Console.WriteLine("==================================================");
            Console.WriteLine("ЛАБОРАТОРНА РОБОТА 9. Алгоритми сортування");
            Console.WriteLine($"Варіант: N = {N}, K = {K}, a = {N % 3}, b = {N % 4}, c = {N % 5}");
            Console.WriteLine("==================================================\n");

            // --- Демонстрація Завдання 1, 2, 3 ---
            Console.WriteLine("=== Перевірка три алгоритми сортування на тестовому масиві ===");
            int[] sample = new int[] { 237, 333, 152, 13, 234, 310, 107, 333 };
            Func<int, int, int> cmp = (x, y) => x.CompareTo(y);

            Console.WriteLine($"Вхідний масив (8 елементів): [{string.Join(", ", sample)}]\n");

            // 1. Сортування вибором
            int[] a1 = (int[])sample.Clone();
            Sorts.SelectionSort(a1, cmp, out long c1, out long m1);
            Console.WriteLine($"1. Сортування вибором:         [{string.Join(", ", a1)}] | Порівнянь: {c1}, Записів: {m1}");
            Console.WriteLine($"   IsSorted: {Sorts.IsSorted(a1, cmp)}, IsPermutationOf: {Sorts.IsPermutationOf(sample, a1, cmp)}");

            // 2. Сортування злиттям знизу вгору
            int[] a2 = (int[])sample.Clone();
            Sorts.BottomUpMergeSort(a2, cmp, out long c2, out long m2);
            Console.WriteLine($"2. Злиттям знизу вгору:        [{string.Join(", ", a2)}] | Порівнянь: {c2}, Записів: {m2}");
            Console.WriteLine($"   IsSorted: {Sorts.IsSorted(a2, cmp)}, IsPermutationOf: {Sorts.IsPermutationOf(sample, a2, cmp)}");

            // 3. Бінарні вставки
            int[] a3 = (int[])sample.Clone();
            Sorts.BinaryInsertionSort(a3, cmp, out long c3, out long m3);
            Console.WriteLine($"3. Бінарними вставками:        [{string.Join(", ", a3)}] | Порівнянь: {c3}, Записів: {m3}");
            Console.WriteLine($"   IsSorted: {Sorts.IsSorted(a3, cmp)}, IsPermutationOf: {Sorts.IsPermutationOf(sample, a3, cmp)}\n");

            // --- Виконання Завдань 4 та 5 ---
            Benchmark.RunTask4Table(N, K);
            Benchmark.RunTask5Timing(N);
            Benchmark.RunTask5Stability();

            // --- Перевірка крайових випадків ---
            RunEdgeCases();
        }

        private static void RunEdgeCases()
        {
            Console.WriteLine("\n=========================================================================================");
            Console.WriteLine("ПЕРЕВІРКА КРАЙОВИХ ВИПАДКІВ:");
            Console.WriteLine("=========================================================================================");
            Func<int, int, int> cmp = (x, y) => x.CompareTo(y);

            int[][] cases = new int[][]
            {
                new int[0],                             // Порожній
                new int[] { 42 },                       // 1 елемент
                new int[] { 10, 20 },                   // 2 елементи в порядку
                new int[] { 20, 10 },                   // 2 елементи навпаки
                new int[] { 5, 5, 5, 5, 5 },            // Усі однакові
                new int[] { 1, 1, 1, 2, 2, 2 }          // Дві півгрупи
            };

            string[] names = new string[]
            {
                "Порожній масив", "1 елемент", "2 елементи в порядку",
                "2 елементи навпаки", "Усі елементи однакові", "Два значення 50/50"
            };

            for (int i = 0; i < cases.Length; i++)
            {
                int[] orig = cases[i];

                int[] test1 = (int[])orig.Clone();
                Sorts.SelectionSort(test1, cmp, out _, out _);

                int[] test2 = (int[])orig.Clone();
                Sorts.BottomUpMergeSort(test2, cmp, out _, out _);

                int[] test3 = (int[])orig.Clone();
                Sorts.BinaryInsertionSort(test3, cmp, out _, out _);

                bool ok1 = Sorts.IsSorted(test1, cmp) && Sorts.IsPermutationOf(orig, test1, cmp);
                bool ok2 = Sorts.IsSorted(test2, cmp) && Sorts.IsPermutationOf(orig, test2, cmp);
                bool ok3 = Sorts.IsSorted(test3, cmp) && Sorts.IsPermutationOf(orig, test3, cmp);

                Console.WriteLine($"| {names[i],-25} | SelSort: {(ok1?"OK":"FAIL")} | MergeSort: {(ok2?"OK":"FAIL")} | BinInsSort: {(ok3?"OK":"FAIL")} |");
            }
        }
    }
}