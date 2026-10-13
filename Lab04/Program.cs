using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lab04
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
            Console.WriteLine($"ЛАБОРАТОРНА РОБОТА 4. Алгоритми над списками");
            Console.WriteLine($"Варіант: N = {N}, K = {K}, a = {N % 3}, b = {N % 4}, c = {N % 5}");
            Console.WriteLine("==================================================\n");

            // 0. Генерація основного списку
            List<int> mainList = GenerateMainList(20, N);
            Console.WriteLine("Основний список (20 елементів [-9; 9]):");
            PrintList("  Оригінал", mainList);
            Console.WriteLine();

            // 1. Завдання 1: побудова за критерієм (елементи > 3)
            Console.WriteLine("=== Завдання 1: Побудова нового списку (елементи > 3) ===");
            List<int> task1Result = Task1_FilterGreaterThan3(mainList);
            PrintList("  Результат", task1Result);
            Console.WriteLine($"  Кількість елементів (Count): {task1Result.Count}");
            Console.WriteLine();

            // 2. Завдання 2: вилучення елементів кратних 3 (три варіанти)
            Console.WriteLine("=== Завдання 2: Вилучення елементів кратних 3 (c = 1) ===");
            List<int> task2Variant1 = Task2_IncorrectRemove(mainList, out List<int> skippedElements);
            PrintList("  1. Хибний варіант (з початку)", task2Variant1);
            PrintList("     Пропущені елементи через зсув індексів", skippedElements);

            List<int> task2Variant2 = Task2_ReverseRemove(mainList);
            PrintList("  2. Правильний варіант (з кінця)", task2Variant2);

            List<int> task2Variant3 = Task2_NewListRemove(mainList);
            PrintList("  3. Правильний варіант (новий список)", task2Variant3);

            bool variantsAreEqual = CompareLists(task2Variant2, task2Variant3);
            Console.WriteLine($"  Перевірка збігу варіантів 2 та 3: {variantsAreEqual}");
            Console.WriteLine();

            // 3. Завдання 3: злиття двох впорядкованих списків
            Console.WriteLine("=== Завдання 3: Злиття двох впорядкованих списків ===");
            List<int> list1 = GenerateSortedList(10 + K % 5, N);       // 12 елементів
            List<int> list2 = GenerateSortedList(6 + K % 4, N + 100);  // 7 елементів

            PrintList("  Список 1 (12 ел.)", list1);
            PrintList("  Список 2 (7 ел.)", list2);

            List<int> mergedList = Task3_MergeSortedLists(list1, list2);
            PrintList("  Злитий список (19 ел.)", mergedList);

            bool isMergedSorted = Task3_IsSorted(mergedList);
            Console.WriteLine($"  Перевірка впорядкованості результату: {isMergedSorted}");
            Console.WriteLine();

            // 4. Завдання 4: розділення списку на парні та непарні
            Console.WriteLine("=== Завдання 4: Розділення списку на парні та непарні ===");
            Task4_SplitEvenOdd(mainList, out List<int> evens, out List<int> odds);
            PrintList("  Парні елементи", evens);
            PrintList("  Непарні елементи", odds);

            bool countCheck = (evens.Count + odds.Count) == mainList.Count;
            Console.WriteLine($"  Перевірка суми Count ({evens.Count} + {odds.Count} == {mainList.Count}): {countCheck}");
            Console.WriteLine();

            // 5. Завдання 5: вимірювання продуктивності
            Console.WriteLine("=== Завдання 5: Замір часу операцій над List<T> ===");
            Task5_Benchmark();
        }

        // --- Генератори списків ---
        public static List<int> GenerateMainList(int count, int seed)
        {
            Random rnd = new Random(seed);
            List<int> list = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(rnd.Next(-9, 10));
            }
            return list;
        }

        public static List<int> GenerateSortedList(int count, int seed)
        {
            Random rnd = new Random(seed);
            List<int> list = new List<int>(count);
            if (count == 0) return list;

            int current = rnd.Next(1, 10);
            list.Add(current);

            for (int i = 1; i < count; i++)
            {
                current += rnd.Next(1, 10);
                list.Add(current);
            }
            return list;
        }

        // --- Завдання 1: Побудова за критерієм (елементи > 3) ---
        public static List<int> Task1_FilterGreaterThan3(List<int> list)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] > 3)
                {
                    result.Add(list[i]);
                }
            }
            return result;
        }

        // --- Завдання 2: Вилучення кратних 3 ---

        // Варіант 1: Хибний (з початку з RemoveAt(i))
        public static List<int> Task2_IncorrectRemove(List<int> list, out List<int> skipped)
        {
            List<int> copy = new List<int>(list);
            skipped = new List<int>();

            for (int i = 0; i < copy.Count; i++)
            {
                if (copy[i] % 3 == 0)
                {
                    copy.RemoveAt(i);
                    // Якщо наступний елемент перемістився на позицію i і він теж кратний 3, він буде пропущений
                    if (i < copy.Count && copy[i] % 3 == 0)
                    {
                        skipped.Add(copy[i]);
                    }
                }
            }
            return copy;
        }

        // Варіант 2: Правильний (цикл з кінця)
        public static List<int> Task2_ReverseRemove(List<int> list)
        {
            List<int> copy = new List<int>(list);
            for (int i = copy.Count - 1; i >= 0; i--)
            {
                if (copy[i] % 3 == 0)
                {
                    copy.RemoveAt(i);
                }
            }
            return copy;
        }

        // Варіант 3: Правильний (новий список)
        public static List<int> Task2_NewListRemove(List<int> list)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] % 3 != 0)
                {
                    result.Add(list[i]);
                }
            }
            return result;
        }

        public static bool CompareLists(List<int> a, List<int> b)
        {
            if (a.Count != b.Count) return false;
            for (int i = 0; i < a.Count; i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }

        // --- Завдання 3: Злиття двох впорядкованих списків ---
        public static List<int> Task3_MergeSortedLists(List<int> list1, List<int> list2)
        {
            List<int> result = new List<int>(list1.Count + list2.Count);
            int i = 0;
            int j = 0;

            while (i < list1.Count && j < list2.Count)
            {
                if (list1[i] <= list2[j])
                {
                    result.Add(list1[i]);
                    i++;
                }
                else
                {
                    result.Add(list2[j]);
                    j++;
                }
            }

            while (i < list1.Count)
            {
                result.Add(list1[i]);
                i++;
            }

            while (j < list2.Count)
            {
                result.Add(list2[j]);
                j++;
            }

            return result;
        }

        public static bool Task3_IsSorted(List<int> list)
        {
            for (int k = 0; k < list.Count - 1; k++)
            {
                if (list[k] > list[k + 1]) return false;
            }
            return true;
        }

        // --- Завдання 4: Розділення на парні та непарні ---
        public static void Task4_SplitEvenOdd(List<int> list, out List<int> evens, out List<int> odds)
        {
            evens = new List<int>();
            odds = new List<int>();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] % 2 == 0)
                {
                    evens.Add(list[i]);
                }
                else
                {
                    odds.Add(list[i]);
                }
            }
        }

        // --- Завдання 5: Вимірювання продуктивності ---
        public static void Task5_Benchmark()
        {
            // Прогрівальний прогін (Warmup)
            List<int> warmup = new List<int>();
            for (int i = 0; i < 1000; i++) warmup.Add(i);
            warmup.Insert(0, -1);
            warmup.Contains(-999);

            Stopwatch sw = new Stopwatch();

            // 1. 100 000 x Add(x) без заданої місткості
            sw.Restart();
            List<int> list1 = new List<int>();
            for (int i = 0; i < 100000; i++)
            {
                list1.Add(i);
            }
            sw.Stop();
            double timeAddDefault = sw.Elapsed.TotalMilliseconds;

            // 2. 100 000 x Add(x) з відомою місткістю
            sw.Restart();
            List<int> list2 = new List<int>(100000);
            for (int i = 0; i < 100000; i++)
            {
                list2.Add(i);
            }
            sw.Stop();
            double timeAddCapacity = sw.Elapsed.TotalMilliseconds;

            // 3. 100 000 x Insert(0, x)
            sw.Restart();
            List<int> list3 = new List<int>();
            for (int i = 0; i < 100000; i++)
            {
                list3.Insert(0, i);
            }
            sw.Stop();
            double timeInsertZero = sw.Elapsed.TotalMilliseconds;

            // 4. 1 000 x Contains() у списку з 100 000 елементів
            sw.Restart();
            for (int i = 0; i < 1000; i++)
            {
                list1.Contains(-99999);
            }
            sw.Stop();
            double timeContains = sw.Elapsed.TotalMilliseconds;

            Console.WriteLine("+-----------------------------------------------------------------------+-------------+");
            Console.WriteLine("| Що міряємо                                                            | Час, мс     |");
            Console.WriteLine("+-----------------------------------------------------------------------+-------------+");
            Console.WriteLine($"| 100 000 x Add(x), список без заданої місткості                       | {timeAddDefault,11:F2} |");
            Console.WriteLine($"| 100 000 x Add(x), new List<int>(100000)                               | {timeAddCapacity,11:F2} |");
            Console.WriteLine($"| 100 000 x Insert(0, x)                                                | {timeInsertZero,11:F2} |");
            Console.WriteLine($"| 1 000 x Contains(значення, якого немає) у списку з 100 000 елементів   | {timeContains,11:F2} |");
            Console.WriteLine("+-----------------------------------------------------------------------+-------------+");
        }

        public static void PrintList(string label, List<int> list)
        {
            Console.WriteLine($"{label}: [{string.Join(", ", list)}]");
        }
    }
}