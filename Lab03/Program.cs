using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab03
{
    class Program
    {
        static void Main(string[] args)
        {
            // Налаштування кодування UTF-8 для консолі
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            int N = 21;
            int K = 17;
            int S = K % 20 + 10; // 27

            Console.WriteLine("=== Завдання 1: Вкладена структура (Максимальна глибина папок) ===");
            Console.Write("Введіть шлях до папки: ");
            string dirPath = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(dirPath) || !Directory.Exists(dirPath))
            {
                Console.WriteLine("Помилка: Вказана папка не існує!");
            }
            else
            {
                int maxFolderDepth = Task1_MaxDirectoryDepth(dirPath);
                Console.WriteLine($"Максимальна глибина вкладеності папок: {maxFolderDepth}");
            }

            Console.WriteLine("\n=== Завдання 2: Розбиття навпіл (Пошук максимуму масиву) ===");
            int[] array16 = GenerateRandomArray(16, N);
            Console.WriteLine("Масив (16 елементів): " + string.Join(", ", array16));

            int maxRecursionDepth = 0;
            int maxVal = Task2_FindMax(array16, 0, array16.Length - 1, ref maxRecursionDepth, 1);
            Console.WriteLine($"Максимум масиву: {maxVal}");
            Console.WriteLine($"Максимальна глибина рекурсії: {maxRecursionDepth} (log2(16) = 4)");

            Console.WriteLine($"\n=== Завдання 3: Перебір із поверненням (Підмножини із сумою S = {S}) ===");
            int[] numbers10 = GenerateRandomPositiveArray(10, N);
            Console.WriteLine("Масив (10 чисел [1; 9]): " + string.Join(", ", numbers10));
            Console.WriteLine($"Усі підмножини із сумою {S}:");
            int subsetCount = 0;
            Task3_SubsetsWithSum(numbers10, 0, 0, S, new List<int>(), ref subsetCount);
            Console.WriteLine($"Загальна кількість підмножин: {subsetCount}");

            Console.WriteLine("\n=== Завдання 4: Трасування (Пошук максимуму для 4 елементів) ===");
            int[] array4 = GenerateRandomArray(4, N);
            Console.WriteLine("Масив (4 елементи): " + string.Join(", ", array4));
            Task4_TraceMax(array4, 0, array4.Length - 1, 0);

            Console.WriteLine("\n=== Завдання 5: Ітеративна версія (Пошук максимуму циклом) ===");
            int iterativeMax = Task5_IterativeMax(array16);
            Console.WriteLine($"Максимум (ітеративно): {iterativeMax}");
            Console.WriteLine($"Результати збігаються: {maxVal == iterativeMax}");
        }

        static int[] GenerateRandomArray(int size, int seed)
        {
            Random rnd = new Random(seed);
            int[] arr = new int[size];
            for (int i = 0; i < size; i++)
            {
                arr[i] = rnd.Next(-20, 21);
            }
            return arr;
        }

        static int[] GenerateRandomPositiveArray(int size, int seed)
        {
            Random rnd = new Random(seed);
            int[] arr = new int[size];
            for (int i = 0; i < size; i++)
            {
                arr[i] = rnd.Next(1, 10);
            }
            return arr;
        }

        public static int Task1_MaxDirectoryDepth(string dirPath)
        {
            int maxDepth = 0;
            try
            {
                string[] subDirs = Directory.GetDirectories(dirPath);
                foreach (string subDir in subDirs)
                {
                    int currentDepth = 1 + Task1_MaxDirectoryDepth(subDir);
                    if (currentDepth > maxDepth)
                    {
                        maxDepth = currentDepth;
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Пропускаємо папки, до яких немає доступу
            }
            return maxDepth;
        }

        public static int Task2_FindMax(int[] a, int lo, int hi, ref int maxDepth, int currentDepth)
        {
            if (currentDepth > maxDepth)
                maxDepth = currentDepth;

            if (lo == hi)
            {
                return a[lo];
            }

            int mid = lo + (hi - lo) / 2;
            int leftMax = Task2_FindMax(a, lo, mid, ref maxDepth, currentDepth + 1);
            int rightMax = Task2_FindMax(a, mid + 1, hi, ref maxDepth, currentDepth + 1);

            return Math.Max(leftMax, rightMax);
        }

        public static void Task3_SubsetsWithSum(int[] arr, int index, int currentSum, int targetSum, List<int> currentSubset, ref int totalCount)
        {
            if (currentSum == targetSum)
            {
                totalCount++;
                Console.WriteLine("{" + string.Join(", ", currentSubset) + "}");
                return;
            }

            if (currentSum > targetSum || index >= arr.Length)
            {
                return;
            }

            currentSubset.Add(arr[index]);
            Task3_SubsetsWithSum(arr, index + 1, currentSum + arr[index], targetSum, currentSubset, ref totalCount);

            currentSubset.RemoveAt(currentSubset.Count - 1);

            Task3_SubsetsWithSum(arr, index + 1, currentSum, targetSum, currentSubset, ref totalCount);
        }

        public static int Task4_TraceMax(int[] a, int lo, int hi, int depth)
        {
            string indent = "".PadLeft(depth * 2);

            if (lo == hi)
            {
                Console.WriteLine($"{indent}enter Max({lo}..{hi}) -> {a[lo]}");
                return a[lo];
            }

            Console.WriteLine($"{indent}enter Max({lo}..{hi})");

            int mid = lo + (hi - lo) / 2;
            int leftMax = Task4_TraceMax(a, lo, mid, depth + 1);
            int rightMax = Task4_TraceMax(a, mid + 1, hi, depth + 1);
            int result = Math.Max(leftMax, rightMax);

            Console.WriteLine($"{indent}exit  Max({lo}..{hi}) -> {result}");
            return result;
        }

        public static int Task5_IterativeMax(int[] a)
        {
            int max = a[0];
            for (int i = 1; i < a.Length; i++)
            {
                if (a[i] > max)
                {
                    max = a[i];
                }
            }
            return max;
        }
    }
}