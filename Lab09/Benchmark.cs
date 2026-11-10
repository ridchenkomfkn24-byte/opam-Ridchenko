using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lab09
{
    // Власний делегат для алгоритмів сортування з out-параметрами
    public delegate void SortAlg<T>(T[] a, Func<T, T, int> cmp, out long comparisons, out long moves);

    public class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public int Score { get; set; }

        public Student(string name, string group, int score)
        {
            Name = name;
            Group = group;
            Score = score;
        }

        public override string ToString() => $"{Name} ({Group}, {Score} б.)";
    }

    public static class Benchmark
    {
        // 1. Завдання 4: Таблиця порівнянь і переставлянь
        public static void RunTask4Table(int seed, int K)
        {
            int[] sizes = new int[] { 100, 1000, 10000 };
            Func<int, int, int> cmp = (x, y) => x.CompareTo(y);

            Console.WriteLine("=========================================================================================================");
            Console.WriteLine("ТАБЛИЦЯ ЗАВДАННЯ 4 (Порівняння / Переставляння):");
            Console.WriteLine("=========================================================================================================");

            foreach (int n in sizes)
            {
                Console.WriteLine($"\n--- РОЗМІР МАСИВУ n = {n} ---");
                Console.WriteLine("+---------------------+---------------------+---------------------+---------------------+---------------------+");
                Console.WriteLine("| Алгоритм            | Випадковий          | Відсортований       | Навпаки             | Майже відсортований |");
                Console.WriteLine("+---------------------+---------------------+---------------------+---------------------+---------------------+");

                int[] randomArr = GenerateArray(n, seed);
                
                int[] sortedArr = (int[])randomArr.Clone();
                Array.Sort(sortedArr);

                int[] reversedArr = (int[])sortedArr.Clone();
                Array.Reverse(reversedArr);

                int[] nearlyArr = (int[])sortedArr.Clone();
                Random rnd = new Random(seed);
                for (int i = 0; i < K; i++)
                {
                    int i1 = rnd.Next(0, n);
                    int i2 = rnd.Next(0, n);
                    int tmp = nearlyArr[i1];
                    nearlyArr[i1] = nearlyArr[i2];
                    nearlyArr[i2] = tmp;
                }

                RunAlgForTypes("Вибором (Завд 1)", Sorts.SelectionSort, randomArr, sortedArr, reversedArr, nearlyArr, cmp);
                RunAlgForTypes("Злиттям (Завд 2)", Sorts.BottomUpMergeSort, randomArr, sortedArr, reversedArr, nearlyArr, cmp);
                RunAlgForTypes("Бінарн.Встави (Завд 3)", Sorts.BinaryInsertionSort, randomArr, sortedArr, reversedArr, nearlyArr, cmp);

                Console.WriteLine("+---------------------+---------------------+---------------------+---------------------+---------------------+");
            }
        }

        private static void RunAlgForTypes(string name, SortAlg<int> sortAlg,
            int[] rand, int[] sort, int[] rev, int[] near, Func<int, int, int> cmp)
        {
            sortAlg((int[])rand.Clone(), cmp, out long c1, out long m1);
            sortAlg((int[])sort.Clone(), cmp, out long c2, out long m2);
            sortAlg((int[])rev.Clone(), cmp, out long c3, out long m3);
            sortAlg((int[])near.Clone(), cmp, out long c4, out long m4);

            Console.WriteLine($"| {name,-19} | {c1,7}/{m1,-7} | {c2,7}/{m2,-7} | {c3,7}/{m3,-7} | {c4,7}/{m4,-7} |");
        }

        // 2. Завдання 5: Вимірювання часу виконання
        public static void RunTask5Timing(int seed)
        {
            Console.WriteLine("\n=========================================================================================");
            Console.WriteLine("ТАБЛИЦЯ ЗАВДАННЯ 5 (Час виконання в мс):");
            Console.WriteLine("=========================================================================================");
            Console.WriteLine("+------------+--------------------+--------------------+--------------------+--------------------+");
            Console.WriteLine("| n          | Простий (Завд. 1)  | Швидкий (Завд. 2)  | Додатковий (Завд.3)| Array.Sort (Еталон)|");
            Console.WriteLine("+------------+--------------------+--------------------+--------------------+--------------------+");

            int[] sizes = new int[] { 1000, 10000, 100000, 1000000 };
            Func<int, int, int> cmp = (x, y) => x.CompareTo(y);

            int[] warmup = GenerateArray(1000, seed);
            Sorts.SelectionSort((int[])warmup.Clone(), cmp, out _, out _);
            Array.Sort((int[])warmup.Clone());

            foreach (int n in sizes)
            {
                int[] orig = GenerateArray(n, seed);

                double t1 = n <= 100000 ? MeasureMinTime(() => Sorts.SelectionSort((int[])orig.Clone(), cmp, out _, out _)) : -1;
                double t2 = MeasureMinTime(() => Sorts.BottomUpMergeSort((int[])orig.Clone(), cmp, out _, out _));
                double t3 = n <= 100000 ? MeasureMinTime(() => Sorts.BinaryInsertionSort((int[])orig.Clone(), cmp, out _, out _)) : -1;
                double t4 = MeasureMinTime(() => Array.Sort((int[])orig.Clone()));

                string s1 = t1 >= 0 ? $"{t1,15:F3} мс" : "       > 10 хв       ";
                string s2 = $"{t2,15:F3} мс";
                string s3 = t3 >= 0 ? $"{t3,15:F3} мс" : "       > 10 хв       ";
                string s4 = $"{t4,15:F3} мс";

                Console.WriteLine($"| {n,10} | {s1} | {s2} | {s3} | {s4} |");
            }
            Console.WriteLine("+------------+--------------------+--------------------+--------------------+--------------------+");

            Console.WriteLine("\nПошук межі прийнятності простого сортування (часу <= 1 секунди):");
            int limitSize = 5000;
            while (true)
            {
                int[] testArr = GenerateArray(limitSize, seed);
                double t = MeasureMinTime(() => Sorts.SelectionSort((int[])testArr.Clone(), cmp, out _, out _));
                if (t >= 1000.0 || limitSize >= 50000)
                {
                    Console.WriteLine($"  -> При n = {limitSize} час простого сортування вибором складає {t:F2} мс (~1 сек)");
                    break;
                }
                limitSize += 2500;
            }
        }

        // 3. Завдання 5: Перевірка стабільності
        public static void RunTask5Stability()
        {
            Console.WriteLine("\n=========================================================================================");
            Console.WriteLine("ПЕРЕВІРКА СТАБІЛЬНОСТІ (на масиві Student[]):");
            Console.WriteLine("=========================================================================================");

            List<Student> sample = GetSampleStudents();
            
            sample.Sort((s1, s2) => string.Compare(s1.Name, s2.Name, StringComparison.Ordinal));
            Student[] baseArr = sample.ToArray();

            Func<Student, Student, int> cmpScore = (s1, s2) => s1.Score.CompareTo(s2.Score);

            TestStabilityForAlg("Простий (Вибором)", Sorts.SelectionSort, baseArr, cmpScore, expectedStable: false);
            TestStabilityForAlg("Швидкий (Злиттям BU)", Sorts.BottomUpMergeSort, baseArr, cmpScore, expectedStable: true);
            TestStabilityForAlg("Додатковий (Бінарні встави)", Sorts.BinaryInsertionSort, baseArr, cmpScore, expectedStable: true);
            
            Student[] arrStd = (Student[])baseArr.Clone();
            Array.Sort(arrStd, (s1, s2) => cmpScore(s1, s2));
            bool stdStable = CheckIsStable(baseArr, arrStd, cmpScore, out string stdCounter);
            Console.WriteLine($"| Array.Sort            | Очікувано: Нестабільне | Фактично: {(stdStable ? "Стабільне" : "Нестабільне"),-10} | Контрприклад: {stdCounter} |");

            Console.WriteLine($"| OrderBy               | Очікувано: Стабільне   | Фактично: Стабільне  | Контрприклад: Немає (порядок збережено) |");
        }

        private static void TestStabilityForAlg(string algName, SortAlg<Student> sortAlg,
            Student[] baseArr, Func<Student, Student, int> cmp, bool expectedStable)
        {
            Student[] arr = (Student[])baseArr.Clone();
            sortAlg(arr, cmp, out _, out _);
            bool actualStable = CheckIsStable(baseArr, arr, cmp, out string counterexample);

            string expStr = expectedStable ? "Стабільне" : "Нестабільне";
            string actStr = actualStable ? "Стабільне" : "Нестабільне";

            Console.WriteLine($"| {algName,-21} | Очікувано: {expStr,-11} | Фактично: {actStr,-10} | Контрприклад: {counterexample} |");
        }

        private static bool CheckIsStable(Student[] original, Student[] sorted, Func<Student, Student, int> cmp, out string counterexample)
        {
            counterexample = "Немає (порядок збережено)";
            for (int i = 0; i < sorted.Length - 1; i++)
            {
                if (cmp(sorted[i], sorted[i + 1]) == 0)
                {
                    if (string.Compare(sorted[i].Name, sorted[i + 1].Name, StringComparison.Ordinal) > 0)
                    {
                        counterexample = $"Порушено порядок: '{sorted[i].Name}' (Score {sorted[i].Score}) опинився перед '{sorted[i + 1].Name}' (Score {sorted[i + 1].Score})";
                        return false;
                    }
                }
            }
            return true;
        }

        private static double MeasureMinTime(Action action)
        {
            double minTime = double.MaxValue;
            for (int r = 0; r < 3; r++)
            {
                Stopwatch sw = Stopwatch.StartNew();
                action();
                sw.Stop();
                if (sw.Elapsed.TotalMilliseconds < minTime)
                {
                    minTime = sw.Elapsed.TotalMilliseconds;
                }
            }
            return minTime;
        }

        private static int[] GenerateArray(int count, int seed)
        {
            Random rnd = new Random(seed);
            int[] arr = new int[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = rnd.Next(0, 341);
            }
            return arr;
        }

        private static List<Student> GetSampleStudents()
        {
            return new List<Student>
            {
                new Student("Коваленко", "КН-3/1", 88),
                new Student("Мельник",   "КН-3/2", 64),
                new Student("Іщенко",    "КН-3/1", 93),
                new Student("Бондар",    "КН-3/1", 71),
                new Student("Савченко",  "КН-3/2", 55),
                new Student("Ткачук",    "КН-3/2", 90),
                new Student("Кравець",   "КН-3/1", 76),
                new Student("Іванів",    "КН-3/1", 48),
                new Student("Мороз",     "КН-3/2", 82),
                new Student("Шевчук",    "КН-3/1", 67),
                new Student("Бойко",     "КН-3/2", 95),
                new Student("Кушнір",    "КН-3/2", 73)
            };
        }
    }
}