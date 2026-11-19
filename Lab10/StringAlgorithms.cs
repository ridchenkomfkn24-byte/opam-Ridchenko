using System;
using System.Collections.Generic;
using System.Text;

namespace Lab10
{
    public static class StringAlgorithms
    {
        // ============================================================
        // ЗАВДАННЯ 1: Три пастки рядків
        // ============================================================

        // Пастка 1.1: конкатенація через += у циклі
        public static string ConcatWithPlusEquals(int count)
        {
            string s = "";
            for (int i = 0; i < count; i++)
            {
                s += 'а';
            }
            return s;
        }

        // Пастка 1.1: конкатенація через StringBuilder
        public static string ConcatWithStringBuilder(int count)
        {
            StringBuilder sb = new StringBuilder(count);
            for (int i = 0; i < count; i++)
            {
                sb.Append('а');
            }
            return sb.ToString();
        }

        // Пастка 1.2: порівняння без урахування регістру через ToLower()
        public static int CountEqualToLower(string[] wordsA, string[] wordsB)
        {
            int matches = 0;
            for (int i = 0; i < wordsA.Length; i++)
            {
                if (wordsA[i].ToLower() == wordsB[i].ToLower())
                {
                    matches++;
                }
            }
            return matches;
        }

        // Пастка 1.2: порівняння без урахування регістру через OrdinalIgnoreCase
        public static int CountEqualOrdinalIgnoreCase(string[] wordsA, string[] wordsB)
        {
            int matches = 0;
            for (int i = 0; i < wordsA.Length; i++)
            {
                if (string.Equals(wordsA[i], wordsB[i], StringComparison.OrdinalIgnoreCase))
                {
                    matches++;
                }
            }
            return matches;
        }

        // Пастка 1.3: сортування слів з літерою 'ґ' (Ordinal проти CurrentCulture)
        public static void DemonstrateCultureTrap()
        {
            string[] words = new string[]
            {
                "ганок", "ґава", "година", "ґрунт", "гуска",
                "дерево", "ґудзик", "дорога", "ґрати", "дума",
                "аґрус", "берег", "брова", "ґедзь", "дім",
                "заґратований", "леґінь", "обґрунтування", "реґіон", "яблуко"
            };

            string[] ordinalSorted = (string[])words.Clone();
            Array.Sort(ordinalSorted, (x, y) => string.Compare(x, y, StringComparison.Ordinal));

            string[] cultureSorted = (string[])words.Clone();
            Array.Sort(cultureSorted, (x, y) => string.Compare(x, y, StringComparison.CurrentCulture));

            Console.WriteLine("Порівняння сортування слів із літерою 'ґ':");
            Console.WriteLine($"  Ordinal:        {string.Join(", ", ordinalSorted)}");
            Console.WriteLine($"  CurrentCulture: {string.Join(", ", cultureSorted)}");

            int diffCount = 0;
            for (int i = 0; i < words.Length; i++)
            {
                if (!string.Equals(ordinalSorted[i], cultureSorted[i], StringComparison.Ordinal))
                {
                    diffCount++;
                }
            }
            Console.WriteLine($"  Кількість позицій, де порядок відрізняється: {diffCount} з {words.Length}");
        }

        // ============================================================
        // ЗАВДАННЯ 2 (a = 0): Найдовший підрядок без повторів символів
        // ============================================================

        // Наївний алгоритм O(n^2): перебираємо всі підрядки s[i..j] і перевіряємо на унікальність
        public static int LongestUniqueSubstringNaive(string s)
        {
            int maxLen = 0;
            int n = s.Length;

            for (int i = 0; i < n; i++)
            {
                for (int j = i; j < n; j++)
                {
                    if (AllCharsUnique(s, i, j))
                    {
                        int len = j - i + 1;
                        if (len > maxLen)
                        {
                            maxLen = len;
                        }
                    }
                }
            }
            return maxLen;
        }

        private static bool AllCharsUnique(string s, int start, int end)
        {
            for (int x = start; x <= end; x++)
            {
                for (int y = x + 1; y <= end; y++)
                {
                    if (s[x] == s[y])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        // Швидкий алгоритм O(n): ковзне вікно зі змінною довжиною + HashSet<char>
        public static int LongestUniqueSubstringFast(string s)
        {
            HashSet<char> window = new HashSet<char>();
            int maxLen = 0;
            int left = 0;

            for (int right = 0; right < s.Length; right++)
            {
                char current = s[right];
                while (window.Contains(current))
                {
                    window.Remove(s[left]);
                    left++;
                }
                window.Add(current);

                int len = right - left + 1;
                if (len > maxLen)
                {
                    maxLen = len;
                }
            }
            return maxLen;
        }

        // ============================================================
        // ЗАВДАННЯ 3: Наївний пошук підрядка з лічильником порівнянь
        // ============================================================

        public static int NaiveIndexOf(string text, string pat, out int comparisons)
        {
            comparisons = 0;
            if (pat.Length == 0) return 0;
            if (pat.Length > text.Length) return -1;

            int limit = text.Length - pat.Length;
            for (int i = 0; i <= limit; i++)
            {
                int j = 0;
                while (j < pat.Length)
                {
                    comparisons++;
                    if (text[i + j] != pat[j])
                    {
                        break;
                    }
                    j++;
                }

                if (j == pat.Length)
                {
                    return i;
                }
            }
            return -1;
        }

        // ============================================================
        // ДОДАТКОВО (c = 1): Масив на 33 позиції проти Dictionary<char, int>
        // ============================================================

        public static int[] CountLettersArray33(string text)
        {
            int[] counts = new int[33];
            for (int i = 0; i < text.Length; i++)
            {
                int idx = GetUkrainianLetterIndex(text[i]);
                if (idx >= 0)
                {
                    counts[idx]++;
                }
            }
            return counts;
        }

        public static Dictionary<char, int> CountLettersDictionary(string text)
        {
            Dictionary<char, int> dict = new Dictionary<char, int>(33);
            for (int i = 0; i < text.Length; i++)
            {
                char lower = NormalizeUkrainianChar(text[i]);
                if (lower != '\0')
                {
                    if (dict.ContainsKey(lower))
                    {
                        dict[lower]++;
                    }
                    else
                    {
                        dict[lower] = 1;
                    }
                }
            }
            return dict;
        }

        // Перетворення української літери в індекс 0..32 без використання ToLower()
        private static int GetUkrainianLetterIndex(char ch)
        {
            switch (ch)
            {
                case 'а': case 'А': return 0;
                case 'б': case 'Б': return 1;
                case 'в': case 'В': return 2;
                case 'г': case 'Г': return 3;
                case 'ґ': case 'Ґ': return 4;
                case 'д': case 'Д': return 5;
                case 'е': case 'Е': return 6;
                case 'є': case 'Є': return 7;
                case 'ж': case 'Ж': return 8;
                case 'з': case 'З': return 9;
                case 'и': case 'И': return 10;
                case 'і': case 'І': return 11;
                case 'ї': case 'Ї': return 12;
                case 'й': case 'Й': return 13;
                case 'к': case 'К': return 14;
                case 'л': case 'Л': return 15;
                case 'м': case 'М': return 16;
                case 'н': case 'Н': return 17;
                case 'о': case 'О': return 18;
                case 'п': case 'П': return 19;
                case 'р': case 'Р': return 20;
                case 'с': case 'С': return 21;
                case 'т': case 'Т': return 22;
                case 'у': case 'У': return 23;
                case 'ф': case 'Ф': return 24;
                case 'х': case 'Х': return 25;
                case 'ц': case 'Ц': return 26;
                case 'ч': case 'Ч': return 27;
                case 'ш': case 'Ш': return 28;
                case 'щ': case 'Щ': return 29;
                case 'ь': case 'Ь': return 30;
                case 'ю': case 'Ю': return 31;
                case 'я': case 'Я': return 32;
                default: return -1;
            }
        }

        private static char NormalizeUkrainianChar(char ch)
        {
            int idx = GetUkrainianLetterIndex(ch);
            if (idx < 0) return '\0';
            const string alphabet = "абвгґдеєжзиіїйклмнопрстуфхцчшщьюя";
            return alphabet[idx];
        }
    }
}