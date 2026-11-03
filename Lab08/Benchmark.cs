using System;
using System.Diagnostics;

namespace Lab08
{
    public static class Benchmark
    {
        // 1. Таблиця порівнянь при різних n
        public static void RunComparisonsBenchmark(int seed)
        {
            int[] sizes = new int[] { 1000, 10000, 100000, 1000000 };
            Func<int, int, int> cmp = (x, y) => x.CompareTo(y);

            Console.WriteLine("+------------+--------------------+--------------------+--------------------+");
            Console.WriteLine("| n          | Лінійний (сер.)   | Бінарний (сер.)    | Відношення         |");
            Console.WriteLine("+------------+--------------------+--------------------+--------------------+");

            foreach (int n in sizes)
            {
                Random rnd = new Random(seed);
                int[] arr = new int[n];
                for (int i = 0; i < n; i++) arr[i] = rnd.Next(0, 341);

                Array.Sort(arr, (x, y) => cmp(x, y));

                long totalLinearComp = 0;
                long totalBinaryComp = 0;
                int queryCount = 100;

                for (int q = 0; q < queryCount; q++)
                {
                    int target = rnd.Next(0, 341);

                    Search.LinearSearch(arr, target, cmp, out int cLin);
                    totalLinearComp += cLin;

                    Search.BinarySearch(arr, target, cmp, out int cBin);
                    totalBinaryComp += cBin;
                }

                double avgLin = (double)totalLinearComp / queryCount;
                double avgBin = (double)totalBinaryComp / queryCount;
                double ratio = avgLin / avgBin;

                Console.WriteLine($"| {n,10} | {avgLin,18:F1} | {avgBin,18:F1} | {ratio,18:F1} |");
            }
            Console.WriteLine("+------------+--------------------+--------------------+--------------------+");
        }

        // 2. Вимірювання точки окупності сортування (час у мс)
        public static void RunTimeCutoffBenchmark(int seed)
        {
            int n = 100000;
            Func<int, int, int> cmp = (x, y) => x.CompareTo(y);
            int[] qValues = new int[] { 1, 5, 10, 20, 30, 50, 100 };

            // Прогрівальний прогін
            int[] warmupArr = new int[1000];
            Array.Sort(warmupArr);
            Search.BinarySearch(warmupArr, 5, cmp, out _);

            Console.WriteLine("+-----+-----------------+------------------------+-----------------+");
            Console.WriteLine("| q   | Лінійно, мс     | Сорт + Бінарно, мс     | Хто швидший     |");
            Console.WriteLine("+-----+-----------------+------------------------+-----------------+");

            foreach (int q in qValues)
            {
                double minTimeLinear = double.MaxValue;
                double minTimeSortBinary = double.MaxValue;

                // Виконуємо заміри 3 рази і беремо найменший
                for (int run = 0; run < 3; run++)
                {
                    Random rnd = new Random(seed + run);
                    int[] rawArray = new int[n];
                    for (int i = 0; i < n; i++) rawArray[i] = rnd.Next(0, 341);

                    int[] targets = new int[q];
                    for (int i = 0; i < q; i++) targets[i] = rnd.Next(0, 341);

                    // Варіант A: Тільки Лінійні пошуки
                    Stopwatch sw = Stopwatch.StartNew();
                    for (int i = 0; i < q; i++)
                    {
                        Search.LinearSearch(rawArray, targets[i], cmp, out _);
                    }
                    sw.Stop();
                    if (sw.Elapsed.TotalMilliseconds < minTimeLinear)
                    {
                        minTimeLinear = sw.Elapsed.TotalMilliseconds;
                    }

                    // Варіант B: Сортування один раз + q Бінарних пошуків
                    sw.Restart();
                    int[] copyArr = (int[])rawArray.Clone();
                    Array.Sort(copyArr, (x, y) => cmp(x, y));
                    for (int i = 0; i < q; i++)
                    {
                        Search.BinarySearch(copyArr, targets[i], cmp, out _);
                    }
                    sw.Stop();
                    if (sw.Elapsed.TotalMilliseconds < minTimeSortBinary)
                    {
                        minTimeSortBinary = sw.Elapsed.TotalMilliseconds;
                    }
                }

                string winner = minTimeLinear < minTimeSortBinary ? "Лінійний" : "Сорт + Бінарний";
                Console.WriteLine($"| {q,3} | {minTimeLinear,15:F3} | {minTimeSortBinary,22:F3} | {winner,-15} |");
            }
            Console.WriteLine("+-----+-----------------+------------------------+-----------------+");
        }
    }
}