using System;
using System.Text;

namespace Lab08
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            int N = 21;
            int K = 17;
            int X = 170;
            int Y = 204;
            int missingVal = 341;

            Console.WriteLine("==================================================");
            Console.WriteLine("ЛАБОРАТОРНА РОБОТА 8. Алгоритми пошуку");
            Console.WriteLine($"Варіант: N = {N}, K = {K}, X = {X}, Y = {Y}, a = {N % 3}, b = {N % 4}, c = {N % 5}");
            Console.WriteLine("==================================================\n");

            // Генерація та сортування основного масиву
            int n = 100000;
            int[] arr = new int[n];
            Random rnd = new Random(N);
            for (int i = 0; i < n; i++)
            {
                arr[i] = rnd.Next(0, 341);
            }

            Func<int, int, int> cmp = (a, b) => a.CompareTo(b);

            // Сортування масиву
            int[] sortedArr = (int[])arr.Clone();
            Array.Sort(sortedArr, (a, b) => cmp(a, b));

            // --- Завдання 1: Лінійний пошук ---
            Console.WriteLine("=== Завдання 1: Лінійний пошук ===");
            int idxL1 = Search.LinearSearch(sortedArr, X, cmp, out int compL1);
            Console.WriteLine($"1. Пошук X = {X}: індекс = {idxL1}, порівнянь = {compL1}");

            int idxL2 = Search.LinearSearch(sortedArr, sortedArr[0], cmp, out int compL2);
            Console.WriteLine($"2. Пошук першого елемента ({sortedArr[0]}): індекс = {idxL2}, порівнянь = {compL2}");

            int idxL3 = Search.LinearSearch(sortedArr, missingVal, cmp, out int compL3);
            Console.WriteLine($"3. Пошук відсутнього ({missingVal}): індекс = {idxL3}, порівнянь = {compL3}\n");

            // --- Завдання 2: Бінарний пошук та Поломки ---
            Console.WriteLine("=== Завдання 2: Правильний бінарний пошук ===");
            int idxB1 = Search.BinarySearch(sortedArr, X, cmp, out int compB1);
            Console.WriteLine($"1. Пошук X = {X}: індекс = {idxB1}, порівнянь = {compB1}");

            int idxB2 = Search.BinarySearch(sortedArr, sortedArr[0], cmp, out int compB2);
            Console.WriteLine($"2. Пошук першого елемента ({sortedArr[0]}): індекс = {idxB2}, порівнянь = {compB2}");

            int idxB3 = Search.BinarySearch(sortedArr, missingVal, cmp, out int compB3);
            Console.WriteLine($"3. Пошук відсутнього ({missingVal}): індекс = {idxB3}, порівнянь = {compB3}\n");

            Console.WriteLine("--- Демонстрація чотирьох поломок ---");
            // Поломка 1
            int broken1Idx = Broken.BinarySearch_Broken1(sortedArr, sortedArr[sortedArr.Length - 1], cmp, out int broken1Comp);
            Console.WriteLine($"Поломка 1 (while lo < hi): останній елемент {sortedArr[sortedArr.Length - 1]} -> індекс = {broken1Idx} (очікувано не знайдено -1)");

            // Поломка 2
            Broken.BinarySearch_Broken2(sortedArr, missingVal, cmp, out _, out bool timedOut);
            Console.WriteLine($"Поломка 2 (lo = mid): зациклення зафіксовано = {timedOut}");

            // Поломка 3
            int broken3Idx = Broken.BinarySearch_Broken3(sortedArr, X, cmp, out int broken3Comp);
            Console.WriteLine($"Поломка 3 (mid = (lo+hi)/2): пошук X -> індекс = {broken3Idx}, порівнянь = {broken3Comp} (на 100k працює коректно)");

            // Поломка 4
            int[] sampleTargets = new int[100];
            for (int i = 0; i < 100; i++) sampleTargets[i] = arr[rnd.Next(0, n)];
            int failedUnsorted = Broken.TestUnsortedSearch(arr, sampleTargets, cmp);
            Console.WriteLine($"Поломка 4 (невідсортований масив): з 100 наявних елементів повернено -1 у {failedUnsorted} випадках\n");

            // --- Завдання 3: Пошук межі ---
            Console.WriteLine($"=== Завдання 3: Пошук межі (b = 1: останній індекс X = {X}) ===");
            int lowerBoundIdx = Search.FindFirst(sortedArr, val => cmp(val, X) >= 0, out int compLB);
            int upperBoundIdx = Search.FindFirst(sortedArr, val => cmp(val, X) > 0, out int compUB);
            int lastIndexX = upperBoundIdx - 1;
            int countX = upperBoundIdx - lowerBoundIdx;

            Console.WriteLine($"Перше входження X = {X}: індекс {lowerBoundIdx} (порівнянь LB: {compLB})");
            Console.WriteLine($"Верхня межа X = {X}: індекс {upperBoundIdx} (порівнянь UB: {compUB})");
            Console.WriteLine($"Індекс останнього входження X: {lastIndexX}");
            Console.WriteLine($"Кількість елементів, рівних X: {countX}");

            // Пошук елементів у діапазоні [X; Y]
            int rangeStart = Search.FindFirst(sortedArr, val => cmp(val, X) >= 0, out _);
            int rangeEnd = Search.FindFirst(sortedArr, val => cmp(val, Y) > 0, out _);
            Console.WriteLine($"Кількість елементів у діапазоні [{X}; {Y}]: {rangeEnd - rangeStart}");

            // Пошук кратного 7 неузгодженою умовою
            int mult7Idx = Search.FindFirst(sortedArr, val => val % 7 == 0, out _);
            Console.WriteLine($"Пошук першого кратного 7 некоректною умовою: індекс = {mult7Idx}, значення = {(mult7Idx < n ? sortedArr[mult7Idx].ToString() : "немає")}\n");

            // --- Завдання 4: Варіант пошуку c = 1 ---
            Console.WriteLine("=== Завдання 4: Бінарний пошук без перевірки рівності в циклі (c = 1) ===");
            int idxNoEq1 = Search.BinarySearchNoEqInLoop(sortedArr, X, cmp, out int compNoEq1);
            int idxNoEq2 = Search.BinarySearchNoEqInLoop(sortedArr, sortedArr[0], cmp, out int compNoEq2);
            int idxNoEq3 = Search.BinarySearchNoEqInLoop(sortedArr, missingVal, cmp, out int compNoEq3);

            Console.WriteLine($"1. Пошук X = {X}: індекс = {idxNoEq1}, порівнянь = {compNoEq1}");
            Console.WriteLine($"2. Пошук першого елемента: індекс = {idxNoEq2}, порівнянь = {compNoEq2}");
            Console.WriteLine($"3. Пошук відсутнього ({missingVal}): індекс = {idxNoEq3}, порівнянь = {compNoEq3}\n");

            // --- Завдання 5: Заміри ---
            Console.WriteLine("=== Завдання 5: Таблиці порівнянь та вимірювання часу ===");
            Console.WriteLine("Таблиця 1: Кількість порівнянь при різних n:");
            Benchmark.RunComparisonsBenchmark(N);
            Console.WriteLine();

            Console.WriteLine("Таблиця 2: Точка окупності сортування (час у мс для n = 100 000):");
            Benchmark.RunTimeCutoffBenchmark(N);
            Console.WriteLine();

            // Крайові випадки
            RunEdgeCases();
        }

        private static void RunEdgeCases()
        {
            Console.WriteLine("=== Перевірка крайових випадків ===");
            Func<int, int, int> cmp = (a, b) => a.CompareTo(b);

            // Порожній масив
            int[] empty = new int[0];
            Search.BinarySearch(empty, 5, cmp, out _);
            Console.WriteLine("1. Порожній масив: опрацьовано без помилок.");

            // Масив з 1 елемента
            int[] one = new int[] { 42 };
            int resOne = Search.BinarySearch(one, 42, cmp, out _);
            Console.WriteLine($"2. Одиничний масив: знайдено за індексом {resOne}.");

            // Усі однакові елементи
            int[] allSame = new int[] { 5, 5, 5, 5, 5 };
            int lb = Search.FindFirst(allSame, val => cmp(val, 5) >= 0, out _);
            int ub = Search.FindFirst(allSame, val => cmp(val, 5) > 0, out _);
            Console.WriteLine($"3. Усі однакові елементи: перша межа = {lb}, остання межа = {ub}.");
        }
    }
}