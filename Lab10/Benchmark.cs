using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lab10
{
    public static class Benchmark
    {
        // Прогрівальний запуск
        public static void Warmup(string sampleText)
        {
            StringAlgorithms.ConcatWithStringBuilder(500);
            StringAlgorithms.LongestUniqueSubstringFast(sampleText.Substring(0, Math.Min(500, sampleText.Length)));
            StringAlgorithms.NaiveIndexOf("тестовий рядок", "рядок", out _);
            StringAlgorithms.CountLettersArray33(sampleText.Substring(0, Math.Min(500, sampleText.Length)));
            StringAlgorithms.CountLettersDictionary(sampleText.Substring(0, Math.Min(500, sampleText.Length)));
        }

        private static double MeasureMinMs(Action action, int runs = 3)
        {
            double minMs = double.MaxValue;
            for (int i = 0; i < runs; i++)
            {
                Stopwatch sw = Stopwatch.StartNew();
                action();
                sw.Stop();
                if (sw.Elapsed.TotalMilliseconds < minMs)
                {
                    minMs = sw.Elapsed.TotalMilliseconds;
                }
            }
            return minMs;
        }

        // --- Завдання 1: Замір трьох пасток ---
        public static void RunTask1Benchmarks(string prose)
        {
            Console.WriteLine("=== ЗАВДАННЯ 1: Три пастки рядків ===");

            // Пастка 1: += проти StringBuilder
            double tPlus10k = MeasureMinMs(() => StringAlgorithms.ConcatWithPlusEquals(10000));
            double tSb10k = MeasureMinMs(() => StringAlgorithms.ConcatWithStringBuilder(10000));
            double tPlus100k = MeasureMinMs(() => StringAlgorithms.ConcatWithPlusEquals(100000), 1); // 1 прогін для 100k += через тривалість
            double tSb100k = MeasureMinMs(() => StringAlgorithms.ConcatWithStringBuilder(100000));

            Console.WriteLine($"Пастка 1 (10 000 додавань):  += : {tPlus10k:F3} мс | StringBuilder: {tSb10k:F4} мс (у {tPlus10k / Math.Max(0.0001, tSb10k):F0}x швидше)");
            Console.WriteLine($"Пастка 1 (100 000 додавань): += : {tPlus100k:F3} мс | StringBuilder: {tSb100k:F4} мс (у {tPlus100k / Math.Max(0.0001, tSb100k):F0}x швидше)\n");

            // Пастка 2: ToLower() == ToLower() проти OrdinalIgnoreCase
            string[] rawWords = prose.Split(new char[] { ' ', '\n', '\r', ',', '.', '—', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            int pairCount = 100000;
            string[] wordsA = new string[pairCount];
            string[] wordsB = new string[pairCount];
            for (int i = 0; i < pairCount; i++)
            {
                string w = rawWords[i % rawWords.Length];
                wordsA[i] = w;
                wordsB[i] = (i % 2 == 0) ? w.ToUpper() : "іншеСлово";
            }

            double tToLower = MeasureMinMs(() => StringAlgorithms.CountEqualToLower(wordsA, wordsB));
            double tOrdinalIC = MeasureMinMs(() => StringAlgorithms.CountEqualOrdinalIgnoreCase(wordsA, wordsB));
            Console.WriteLine($"Пастка 2 (100 000 порівнянь): ToLower: {tToLower:F3} мс | OrdinalIgnoreCase: {tOrdinalIC:F3} мс (у {tToLower / Math.Max(0.001, tOrdinalIC):F1}x швидше)\n");

            // Пастка 3: Ordinal проти CurrentCulture
            StringAlgorithms.DemonstrateCultureTrap();
            Console.WriteLine();
        }

        // --- Завдання 2: Наївний проти ковзного вікна ---
        public static void RunTask2Benchmarks(string prose)
        {
            Console.WriteLine("=== ЗАВДАННЯ 2: Найдовший підрядок без повторів (a = 0) ===");
            int[] sizes = new int[] { 1000, 10000, 50000 };

            Console.WriteLine("+------------+-----------------+-----------------+----------------+-----------+");
            Console.WriteLine("| Символів   | Наївно, мс      | Швидко, мс      | У скільки разів| Відповідь |");
            Console.WriteLine("+------------+-----------------+-----------------+----------------+-----------+");

            foreach (int len in sizes)
            {
                string sub = prose.Substring(0, len);
                int ansNaive = 0;
                int ansFast = 0;

                double tNaive = MeasureMinMs(() => ansNaive = StringAlgorithms.LongestUniqueSubstringNaive(sub), len == 50000 ? 1 : 3);
                double tFast = MeasureMinMs(() => ansFast = StringAlgorithms.LongestUniqueSubstringFast(sub));

                if (ansNaive != ansFast)
                {
                    throw new Exception($"Помилка узгодженості! Naive={ansNaive}, Fast={ansFast}");
                }

                double ratio = tNaive / Math.Max(0.001, tFast);
                Console.WriteLine($"| {len,10} | {tNaive,15:F3} | {tFast,15:F4} | {ratio,13:F1}x | {ansFast,9} |");
            }
            Console.WriteLine("+------------+-----------------+-----------------+----------------+-----------+\n");
        }

        // --- Завдання 3: Власний IndexOf проти string.IndexOf ---
        public static void RunTask3Benchmarks(string prose)
        {
            Console.WriteLine("=== ЗАВДАННЯ 3: Наївний пошук підрядка (шаблон: 1069 'М' + 'Н') ===");

            string prose50k = prose.Substring(0, 50000);
            // Слово, яке є ближче до кінця тексту (наприклад, останні 12 символів перед крапкою)
            string wordAtEnd = "закономірність";
            string missingWord = "синхрофазотрон";

            string repeats10k = new string('М', 10000);
            string pattern1070 = new string('М', 1069) + "Н";

            Console.WriteLine("+-----------------------------------+------------------------+------------+----------+-------------------+");
            Console.WriteLine("| Вхід                              | Шаблон                 | Порівнянь  | Час, мс  | string.IndexOf,мс |");
            Console.WriteLine("+-----------------------------------+------------------------+------------+----------+-------------------+");

            RunSearchRow("проза, 50 тис. символів", $"\"{wordAtEnd}\"", prose50k, wordAtEnd);
            RunSearchRow("проза, 50 тис. символів", $"\"{missingWord}\"", prose50k, missingWord);
            RunSearchRow("повтори, 10 тис. літер М", "1069 'М' + 'Н' (1070)", repeats10k, pattern1070);

            Console.WriteLine("+-----------------------------------+------------------------+------------+----------+-------------------+\n");
        }

        private static void RunSearchRow(string inputLabel, string patLabel, string text, string pat)
        {
            int comparisons = 0;
            int idx1 = 0;
            int idx2 = 0;

            double tNaive = MeasureMinMs(() => idx1 = StringAlgorithms.NaiveIndexOf(text, pat, out comparisons));
            double tBuiltIn = MeasureMinMs(() => idx2 = text.IndexOf(pat, StringComparison.Ordinal));

            if (idx1 != idx2)
            {
                throw new Exception($"Незбіг IndexOf: Naive={idx1}, BuiltIn={idx2}");
            }

            Console.WriteLine($"| {inputLabel,-33} | {patLabel,-22} | {comparisons,10} | {tNaive,8:F4} | {tBuiltIn,17:F4} |");
        }

        // --- Завдання 4: Жадібно проти повного перебору ---
        public static void RunTask4Benchmarks(int seed)
        {
            Console.WriteLine("=== ЗАВДАННЯ 4: Найбільше заявок без перетинів (b = 1) ===");
            Random rnd = new Random(seed);
            int totalTests = 100;
            int optimalMatches = 0;
            double worstLagPercent = 0.0;

            for (int t = 0; t < totalTests; t++)
            {
                int count = rnd.Next(1, 16); // до 15 елементів на тест
                RequestInterval[] reqs = new RequestInterval[count];
                for (int i = 0; i < count; i++)
                {
                    int start = rnd.Next(0, 44);
                    int end = rnd.Next(start + 1, 45);
                    reqs[i] = new RequestInterval(start, end);
                }

                int greedyCount = SchedulingAlgorithms.SolveGreedy(reqs).Count;
                int bruteCount = SchedulingAlgorithms.SolveBruteForce(reqs);

                if (greedyCount == bruteCount)
                {
                    optimalMatches++;
                }
                else if (bruteCount > 0)
                {
                    double lag = (double)(bruteCount - greedyCount) / bruteCount * 100.0;
                    if (lag > worstLagPercent) worstLagPercent = lag;
                }
            }

            // Замір часу повного перебору на 15 і 20 елементах
            RequestInterval[] test15 = GenerateRandomRequests(15, seed);
            RequestInterval[] test20 = GenerateRandomRequests(20, seed + 1);

            double timeBrute15 = MeasureMinMs(() => SchedulingAlgorithms.SolveBruteForce(test15));
            double timeBrute20 = MeasureMinMs(() => SchedulingAlgorithms.SolveBruteForce(test20));

            Console.WriteLine($"Тестів, де жадібне рішення дало оптимум: {optimalMatches} з {totalTests}");
            Console.WriteLine($"Найгірше відставання: {worstLagPercent:F2}%");
            Console.WriteLine($"Час повного перебору на 15 елементах: {timeBrute15:F4} мс");
            Console.WriteLine($"Час повного перебору на 20 елементах: {timeBrute20:F4} мс\n");

            SchedulingAlgorithms.DemonstrateWrongRuleCounterexamples();
        }

        private static RequestInterval[] GenerateRandomRequests(int count, int seed)
        {
            Random rnd = new Random(seed);
            RequestInterval[] reqs = new RequestInterval[count];
            for (int i = 0; i < count; i++)
            {
                int start = rnd.Next(0, 44);
                int end = rnd.Next(start + 1, 45);
                reqs[i] = new RequestInterval(start, end);
            }
            return reqs;
        }

        // --- Додатково (c = 1): Масив [33] проти Dictionary<char, int> ---
        public static void RunTaskCBenchmarks(string prose)
        {
            Console.WriteLine("=== ДОДАТКОВО (c = 1): Масив [33] проти Dictionary<char, int> ===");
            int[] arrResult = null!;
            Dictionary<char, int> dictResult = null!;

            double tArray = MeasureMinMs(() => arrResult = StringAlgorithms.CountLettersArray33(prose));
            double tDict = MeasureMinMs(() => dictResult = StringAlgorithms.CountLettersDictionary(prose));

            Console.WriteLine($"Текст обсягом {prose.Length} символів:");
            Console.WriteLine($"  Масив int[33]:            {tArray:F4} мс");
            Console.WriteLine($"  Dictionary<char, int>:    {tDict:F4} мс");
            Console.WriteLine($"  Прискорення масиву:       у {tDict / Math.Max(0.0001, tArray):F1}x разів швидше\n");
        }
    }
}