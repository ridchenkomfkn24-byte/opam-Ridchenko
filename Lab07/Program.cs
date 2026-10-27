using System;
using System.Collections.Generic;
using System.Text;

namespace Lab07
{
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

        public override string ToString()
        {
            return $"{Name} ({Group}, {Score} б.)";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            int N = 21;
            int K = 17;

            Console.WriteLine("==================================================");
            Console.WriteLine($"ЛАБОРАТОРНА РОБОТА 7. Узагальнені алгоритми");
            Console.WriteLine($"Варіант: N = {N}, K = {K}, a = {N % 3}, b = {N % 4}, c = {N % 5}");
            Console.WriteLine("==================================================\n");

            // --- Підготовка наборів даних ---
            List<int> numbers = GenerateNumbers(20, N, K);
            List<string> words = GetSampleWords();
            List<Student> students = GetSampleStudents();

            Console.WriteLine($"1. Набір чисел (20 ел., [-{K}; {K}]): [{string.Join(", ", numbers)}]");
            Console.WriteLine($"2. Набір рядків ({words.Count} слів): [{string.Join(", ", words)}]");
            Console.WriteLine($"3. Набір студентів ({students.Count} осіб)\n");

            // --- Завдання 1: Filter<T> ---
            Console.WriteLine("=== Завдання 1: Відбір (Filter<T>) ===");
            // a = 0: Числа > K (17); Рядки > 5 символів; Студенти Score >= 75
            List<int> filteredNumbers = Algorithms.Filter(numbers, x => x > K);
            List<string> filteredWords = Algorithms.Filter(words, w => w.Length > 5);
            List<Student> filteredStudents = Algorithms.Filter(students, s => s.Score >= 75);

            Console.WriteLine($"Числа > {K} (Розмір: {filteredNumbers.Count}): [{string.Join(", ", filteredNumbers)}]");
            Console.WriteLine($"Рядки > 5 символів (Розмір: {filteredWords.Count}): [{string.Join(", ", filteredWords)}]");
            Console.WriteLine($"Студенти з балом >= 75 (Розмір: {filteredStudents.Count}):");
            foreach (var s in filteredStudents) Console.WriteLine($"   - {s}");
            Console.WriteLine();

            // --- Завдання 2: Map<TIn, TOut> ---
            Console.WriteLine("=== Завдання 2: Перетворення (Map<TIn, TOut>) ===");
            // b = 1: 
            // Числа -> остача від ділення на K % 7 + 2 = 17 % 7 + 2 = 5
            // Рядки -> верхній регістр
            // Студенти -> рядок "Прізвище (бал)"
            int modVal = K % 7 + 2;
            List<int> mappedNumbers = Algorithms.Map(numbers, x => Math.Abs(x) % modVal);
            List<string> mappedWords = Algorithms.Map(words, w => w.ToUpper());
            List<string> mappedStudents = Algorithms.Map(students, s => $"{s.Name} ({s.Score})");

            Console.WriteLine($"Числа (модуль остачі на {modVal}): [{string.Join(", ", mappedNumbers)}]");
            Console.WriteLine($"Рядки у верхньому регістрі: [{string.Join(", ", mappedWords)}]");
            Console.WriteLine($"Студенти у форматі 'Прізвище (бал)': [{string.Join(", ", mappedStudents)}]");
            Console.WriteLine();

            // --- Завдання 3: Aggregate ---
            Console.WriteLine("=== Завдання 3: Чотири агрегації (c = 1) ===");
            // c = 1: 1.Сума, 2.Добуток, 3.Максимум, 4.Найбільший за абсолютною величиною елемент
            int sum = Algorithms.Aggregate(numbers, 0, (acc, x) => acc + x);
            long product = Algorithms.Aggregate(numbers, 1L, (acc, x) => acc * x);
            int maxVal = numbers.Count > 0 
                ? Algorithms.Aggregate(numbers, numbers[0], (acc, x) => x > acc ? x : acc)
                : 0;
            int maxAbsVal = numbers.Count > 0
                ? Algorithms.Aggregate(numbers, numbers[0], (acc, x) => Math.Abs(x) > Math.Abs(acc) ? x : acc)
                : 0;

            Console.WriteLine($"1. Сума елементів: {sum}");
            Console.WriteLine($"2. Добуток елементів: {product}");
            Console.WriteLine($"3. Максимум: {maxVal}");
            Console.WriteLine($"4. Найбільший за модулем елемент: {maxAbsVal}");
            Console.WriteLine();

            // --- Завдання 4: GroupBy ---
            Console.WriteLine("=== Завдання 4: Групування (GroupBy) ===");
            // 1. Студенти за групою
            var byGroup = Algorithms.GroupBy(students, s => s.Group);
            Console.WriteLine("1. Студенти за групою:");
            foreach (var g in byGroup) Console.WriteLine($"   Група '{g.Key}': {g.Value.Count} осіб");

            // 2. Студенти за першою літерою прізвища
            var byFirstLetter = Algorithms.GroupBy(students, s => s.Name[0]);
            Console.WriteLine("2. Студенти за першою літерою прізвища:");
            foreach (var g in byFirstLetter) Console.WriteLine($"   Буква '{g.Key}': {g.Value.Count} осіб");

            // 3. Студенти за діапазоном балу (0-59, 60-74, 75-89, 90-100)
            var byScoreRange = Algorithms.GroupBy(students, s => GetScoreRange(s.Score));
            Console.WriteLine("3. Студенти за діапазоном балів:");
            foreach (var g in byScoreRange) Console.WriteLine($"   Діапазон '{g.Key}': {g.Value.Count} осіб");

            // 4. Рядки за довжиною
            var wordsByLength = Algorithms.GroupBy(words, w => w.Length);
            Console.WriteLine("4. Рядки за довжиною:");
            foreach (var g in wordsByLength) Console.WriteLine($"   Довжина {g.Key}: {g.Value.Count} слів");
            Console.WriteLine();

            // --- Завдання 5: Захоплення змінних ---
            Console.WriteLine("=== Завдання 5: Несподіванки захоплення змінних ===");
            RunTask5ClosurePitfalls(K);
        }

        private static string GetScoreRange(int score)
        {
            if (score < 60) return "0–59";
            if (score <= 74) return "60–74";
            if (score <= 89) return "75–89";
            return "90–100";
        }

        private static List<int> GenerateNumbers(int count, int seed, int K)
        {
            Random rnd = new Random(seed);
            List<int> list = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(rnd.Next(-K, K + 1));
            }
            return list;
        }

        private static List<string> GetSampleWords()
        {
            return new List<string>
            {
                "алгоритм", "код", "c-sharp", "функція", "делегат", 
                "типи", "узагальнення", "веб-сервер", "о-го-го", "і-я", 
                "масив", "стек", "черга"
            };
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

        private static void RunTask5ClosurePitfalls(int K)
        {
            // Ситуація 1
            int threshold = K;
            Func<int, bool> check = x => x > threshold;
            threshold = 1000;
            Console.WriteLine($"1. Змінна після лямбди: check(100) = {check(100)} (очікували true для threshold={K})");

            // Ситуація 2
            List<Func<int>> actionsFor = new List<Func<int>>();
            for (int i = 0; i < 3; i++)
            {
                actionsFor.Add(() => i);
            }
            Console.Write("2. Цикл for: ");
            foreach (var act in actionsFor) Console.Write($"{act()} ");
            Console.WriteLine();

            // Ситуація 3
            List<Func<int>> actionsForeach = new List<Func<int>>();
            int[] arr = new int[] { 0, 1, 2 };
            foreach (var item in arr)
            {
                actionsForeach.Add(() => item);
            }
            Console.Write("3. Цикл foreach: ");
            foreach (var act in actionsForeach) Console.Write($"{act()} ");
            Console.WriteLine();
        }
    }
}