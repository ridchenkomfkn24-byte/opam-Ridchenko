using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lab06
{
    // Клас студента для Завдання 2
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
            Console.WriteLine($"ЛАБОРАТОРНА РОБОТА 6. Алгоритми підрахунку та групування");
            Console.WriteLine($"Варіант: N = {N}, K = {K}, a = {N % 3}, b = {N % 4}, c = {N % 5}");
            Console.WriteLine("==================================================\n");

            // --- Завдання 1 ---
            Console.WriteLine("=== Завдання 1: Частоти слів у тексті (a = 0) ===");
            string text = "Алгоритм описує послідовність дій. Алгоритм можна записати словами, " +
                         "а можна записати кодом. Кодом алгоритм стає тоді, коли його " +
                         "виконує машина, а не людина. Одна задача — багато алгоритмів.";

            Console.WriteLine($"Текст: \"{text}\"\n");
            int threshold = K % 3 + 2; // 17 % 3 + 2 = 4
            Task1_WordFrequencies(text, threshold);
            Console.WriteLine();

            // --- Завдання 2 ---
            Console.WriteLine("=== Завдання 2: Групування за навчальною групою (b = 1) ===");
            List<Student> students = GetSampleStudents();
            Task2_GroupByAcademicGroup(students);
            Console.WriteLine();

            // --- Завдання 3 ---
            Console.WriteLine("=== Завдання 3: Вилучення дублікатів двома способами ===");
            Task3_RemoveDuplicates(N, K);
            Console.WriteLine();

            // --- Завдання 4 ---
            Console.WriteLine("=== Завдання 4: Елементи першого набору, відсутні в другому (c = 1) ===");
            Task4_DifferenceOfSets(N, K);
            Console.WriteLine();

            // --- Завдання 5 ---
            Console.WriteLine("=== Завдання 5: Замір виграшу швидкості (Stopwatch) ===");
            Task5_Benchmark();
        }

        // --- Завдання 1: Частоти слів ---
        public static void Task1_WordFrequencies(string text, int minFrequency)
        {
            Dictionary<string, int> freqDict = new Dictionary<string, int>();
            string cleanText = RemovePunctuation(text);
            string[] words = cleanText.ToLower().Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                string w = words[i];
                if (freqDict.ContainsKey(w))
                {
                    freqDict[w]++;
                }
                else
                {
                    freqDict[w] = 1;
                }
            }

            Console.WriteLine($"Загальна кількість унікальних слів: {freqDict.Count}");
            Console.WriteLine($"Слова з частотою >= {minFrequency}:");

            foreach (KeyValuePair<string, int> pair in freqDict)
            {
                if (pair.Value >= minFrequency)
                {
                    Console.WriteLine($"  - '{pair.Key}': {pair.Value} разів");
                }
            }
        }

        private static string RemovePunctuation(string text)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                if (!char.IsPunctuation(ch) && ch != '—')
                {
                    sb.Append(ch);
                }
                else
                {
                    sb.Append(' ');
                }
            }
            return sb.ToString();
        }

        // --- Завдання 2: Групування студентів ---
        public static void Task2_GroupByAcademicGroup(List<Student> students)
        {
            Dictionary<string, List<Student>> groupDict = new Dictionary<string, List<Student>>();

            for (int i = 0; i < students.Count; i++)
            {
                Student s = students[i];
                if (!groupDict.ContainsKey(s.Group))
                {
                    groupDict[s.Group] = new List<Student>();
                }
                groupDict[s.Group].Add(s);
            }

            foreach (KeyValuePair<string, List<Student>> entry in groupDict)
            {
                string groupName = entry.Key;
                List<Student> list = entry.Value;

                double sum = 0;
                for (int j = 0; j < list.Count; j++)
                {
                    sum += list[j].Score;
                }
                double avg = list.Count > 0 ? sum / list.Count : 0;

                Console.WriteLine($"Група: {groupName} | Кількість: {list.Count} | Середній бал: {avg:F2}");
            }
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

        // --- Завдання 3: Вилучення дублікатів ---
        public static void Task3_RemoveDuplicates(int N, int K)
        {
            int maxVal = K % 10 + 6; // 17 % 10 + 6 = 13
            Random rnd = new Random(N);
            List<int> origList = new List<int>(25);

            for (int i = 0; i < 25; i++)
            {
                origList.Add(rnd.Next(1, maxVal + 1));
            }

            Console.WriteLine($"Вихідний список (25 елементів [1; {maxVal}]):");
            Console.WriteLine($"  [{string.Join(", ", origList)}]");

            // 1. Без збереження порядку через HashSet
            HashSet<int> setUnordered = new HashSet<int>(origList);
            List<int> resultUnordered = new List<int>(setUnordered);

            // 2. Зі збереженням порядку
            HashSet<int> seen = new HashSet<int>();
            List<int> resultOrdered = new List<int>();

            for (int i = 0; i < origList.Count; i++)
            {
                if (seen.Add(origList[i]))
                {
                    resultOrdered.Add(origList[i]);
                }
            }

            Console.WriteLine($"Спосіб 1 (HashSet конструктор): [{string.Join(", ", resultUnordered)}]");
            Console.WriteLine($"Спосіб 2 (Збереження порядку):   [{string.Join(", ", resultOrdered)}]");
        }

        // --- Завдання 4: Порівняння двох наборів ---
        public static void Task4_DifferenceOfSets(int N, int K)
        {
            int maxVal = 20 + K; // 20 + 17 = 37
            Random rnd1 = new Random(N);
            Random rnd2 = new Random(N + 100);

            List<int> listA = new List<int>(30);
            List<int> listB = new List<int>(30);

            for (int i = 0; i < 30; i++)
            {
                listA.Add(rnd1.Next(1, maxVal + 1));
                listB.Add(rnd2.Next(1, maxVal + 1));
            }

            // Спосіб 1: Через HashSet
            HashSet<int> setA = new HashSet<int>(listA);
            HashSet<int> setB = new HashSet<int>(listB);
            setA.ExceptWith(setB);
            List<int> hashSetResult = new List<int>(setA);

            // Спосіб 2: Ручний через вкладені цикли (без Hash/Dict)
            List<int> manualResult = new List<int>();

            for (int i = 0; i < listA.Count; i++)
            {
                int val = listA[i];

                // Перевіряємо, чи є в listB
                bool inB = false;
                for (int j = 0; j < listB.Count; j++)
                {
                    if (listB[j] == val)
                    {
                        inB = true;
                        break;
                    }
                }

                if (!inB)
                {
                    // Перевіряємо, чи немає вже у manualResult (дублікати)
                    bool alreadyAdded = false;
                    for (int k = 0; k < manualResult.Count; k++)
                    {
                        if (manualResult[k] == val)
                        {
                            alreadyAdded = true;
                            break;
                        }
                    }

                    if (!alreadyAdded)
                    {
                        manualResult.Add(val);
                    }
                }
            }

            // Перевірка збігу двох результатів
            bool resultsMatch = CompareUnorderedLists(hashSetResult, manualResult);

            Console.WriteLine($"HashSet метод (A \\ B): [{string.Join(", ", hashSetResult)}]");
            Console.WriteLine($"Ручний метод (A \\ B):   [{string.Join(", ", manualResult)}]");
            Console.WriteLine($"Результати двох методів збігаються: {resultsMatch}");
        }

        private static bool CompareUnorderedLists(List<int> a, List<int> b)
        {
            if (a.Count != b.Count) return false;

            for (int i = 0; i < a.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < b.Count; j++)
                {
                    if (a[i] == b[j])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) return false;
            }
            return true;
        }

        // --- Завдання 5: Заміри продуктивності ---
        public static void Task5_Benchmark()
        {
            Stopwatch sw = new Stopwatch();

            // Прогрівальний прогін (Warmup)
            HashSet<int> warmupSet = new HashSet<int>();
            List<int> warmupList = new List<int>();
            for (int i = 0; i < 1000; i++)
            {
                warmupSet.Add(i);
                warmupList.Add(i);
            }
            warmupSet.Contains(500);
            warmupList.Contains(500);

            // --- Операція 1: 10 000 перевірок наявності в наборі зі 100 000 елементів ---
            int size1 = 100000;
            List<int> listSearch = new List<int>(size1);
            HashSet<int> setSearch = new HashSet<int>();

            for (int i = 0; i < size1; i++)
            {
                listSearch.Add(i);
                setSearch.Add(i);
            }

            sw.Restart();
            for (int i = 0; i < 10000; i++)
            {
                listSearch.Contains(-999);
            }
            sw.Stop();
            double timeListSearch = sw.Elapsed.TotalMilliseconds;

            sw.Restart();
            for (int i = 0; i < 10000; i++)
            {
                setSearch.Contains(-999);
            }
            sw.Stop();
            double timeSetSearch = sw.Elapsed.TotalMilliseconds;

            // --- Операція 2: Підрахунок частот 50 000 слів ---
            int wordCount = 50000;
            string[] wordsData = new string[wordCount];
            Random rnd = new Random(21);
            for (int i = 0; i < wordCount; i++)
            {
                wordsData[i] = "word_" + rnd.Next(1, 1000);
            }

            // Список (два паралельних списки)
            sw.Restart();
            List<string> keysList = new List<string>();
            List<int> valuesList = new List<int>();

            for (int i = 0; i < wordCount; i++)
            {
                string w = wordsData[i];
                int index = -1;
                for (int j = 0; j < keysList.Count; j++)
                {
                    if (keysList[j] == w)
                    {
                        index = j;
                        break;
                    }
                }

                if (index != -1)
                {
                    valuesList[index]++;
                }
                else
                {
                    keysList.Add(w);
                    valuesList.Add(1);
                }
            }
            sw.Stop();
            double timeListFreq = sw.Elapsed.TotalMilliseconds;

            // Словник
            sw.Restart();
            Dictionary<string, int> dictFreq = new Dictionary<string, int>();
            for (int i = 0; i < wordCount; i++)
            {
                string w = wordsData[i];
                if (dictFreq.ContainsKey(w))
                {
                    dictFreq[w]++;
                }
                else
                {
                    dictFreq[w] = 1;
                }
            }
            sw.Stop();
            double timeDictFreq = sw.Elapsed.TotalMilliseconds;

            // --- Операція 3: Вилучення дублікатів із 20 000 чисел ---
            int numCount = 20000;
            int[] numsData = new int[numCount];
            for (int i = 0; i < numCount; i++)
            {
                numsData[i] = rnd.Next(1, 5000);
            }

            // Список
            sw.Restart();
            List<int> listUnique = new List<int>();
            for (int i = 0; i < numCount; i++)
            {
                if (!listUnique.Contains(numsData[i]))
                {
                    listUnique.Add(numsData[i]);
                }
            }
            sw.Stop();
            double timeListDup = sw.Elapsed.TotalMilliseconds;

            // Множина (HashSet)
            sw.Restart();
            HashSet<int> setUnique = new HashSet<int>();
            for (int i = 0; i < numCount; i++)
            {
                setUnique.Add(numsData[i]);
            }
            sw.Stop();
            double timeSetDup = sw.Elapsed.TotalMilliseconds;

            // --- Операція 4: Спільні елементи двох наборів по 5 000 елементів ---
            int setSize = 5000;
            List<int> listA_bench = new List<int>(setSize);
            List<int> listB_bench = new List<int>(setSize);

            for (int i = 0; i < setSize; i++)
            {
                listA_bench.Add(rnd.Next(1, 10000));
                listB_bench.Add(rnd.Next(1, 10000));
            }

            // Список (вкладені цикли)
            sw.Restart();
            List<int> listIntersect = new List<int>();
            for (int i = 0; i < listA_bench.Count; i++)
            {
                int val = listA_bench[i];
                bool found = false;
                for (int j = 0; j < listB_bench.Count; j++)
                {
                    if (listB_bench[j] == val)
                    {
                        found = true;
                        break;
                    }
                }
                if (found && !listIntersect.Contains(val))
                {
                    listIntersect.Add(val);
                }
            }
            sw.Stop();
            double timeListIntersect = sw.Elapsed.TotalMilliseconds;

            // Множина (HashSet)
            sw.Restart();
            HashSet<int> setA_bench = new HashSet<int>(listA_bench);
            HashSet<int> setB_bench = new HashSet<int>(listB_bench);
            setA_bench.IntersectWith(setB_bench);
            sw.Stop();
            double timeSetIntersect = sw.Elapsed.TotalMilliseconds;

            // Таблиця результатів
            Console.WriteLine("+--------------------------------------------------+------------+------------+----------------+");
            Console.WriteLine("| Що міряємо                                       | Список, мс | Словник/   | У скільки разів|");
            Console.WriteLine("|                                                  |            | Множина,мс | швидше         |");
            Console.WriteLine("+--------------------------------------------------+------------+------------+----------------+");
            Console.WriteLine($"| 10 000 перевірок наявності у 100 000 елементах   | {timeListSearch,10:F2} | {timeSetSearch,10:F2} | {(timeListSearch / Math.Max(0.01, timeSetSearch)),14:F1} |");
            Console.WriteLine($"| Підрахунок частот 50 000 слів                    | {timeListFreq,10:F2} | {timeDictFreq,10:F2} | {(timeListFreq / Math.Max(0.01, timeDictFreq)),14:F1} |");
            Console.WriteLine($"| Вилучення дублікатів із 20 000 чисел              | {timeListDup,10:F2} | {timeSetDup,10:F2} | {(timeListDup / Math.Max(0.01, timeSetDup)),14:F1} |");
            Console.WriteLine($"| Спільні елементи двох наборів по 5 000 елементів | {timeListIntersect,10:F2} | {timeSetIntersect,10:F2} | {(timeListIntersect / Math.Max(0.01, timeSetIntersect)),14:F1} |");
            Console.WriteLine("+--------------------------------------------------+------------+------------+----------------+");
        }
    }
}