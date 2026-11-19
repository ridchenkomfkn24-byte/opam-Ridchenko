using System;
using System.Collections.Generic;

namespace Lab10
{
    public struct RequestInterval
    {
        public int Start { get; }
        public int End { get; }

        public RequestInterval(int start, int end)
        {
            Start = start;
            End = end;
        }

        public int Duration => End - Start;

        public override string ToString() => $"[{Start}; {End})";
    }

    public static class SchedulingAlgorithms
    {
        // 1. Жадібний алгоритм (b = 1): обираємо заявку, що закінчується раніше
        public static List<RequestInterval> SolveGreedy(RequestInterval[] requests)
        {
            List<RequestInterval> selected = new List<RequestInterval>();
            if (requests.Length == 0) return selected;

            RequestInterval[] sorted = (RequestInterval[])requests.Clone();
            Array.Sort(sorted, (a, b) =>
            {
                int cmpEnd = a.End.CompareTo(b.End);
                return cmpEnd != 0 ? cmpEnd : a.Start.CompareTo(b.Start);
            });

            int lastEnd = -1;
            for (int i = 0; i < sorted.Length; i++)
            {
                if (sorted[i].Start >= lastEnd)
                {
                    selected.Add(sorted[i]);
                    lastEnd = sorted[i].End;
                }
            }
            return selected;
        }

        // 2. Повний перебір (еталон): знаходить максимальну кількість заявок без перетинів
        public static int SolveBruteForce(RequestInterval[] requests)
        {
            if (requests.Length == 0) return 0;

            RequestInterval[] sorted = (RequestInterval[])requests.Clone();
            Array.Sort(sorted, (a, b) =>
            {
                int cmpStart = a.Start.CompareTo(b.Start);
                return cmpStart != 0 ? cmpStart : a.End.CompareTo(b.End);
            });

            return Backtrack(sorted, 0, -1);
        }

        private static int Backtrack(RequestInterval[] sorted, int idx, int lastEnd)
        {
            if (idx == sorted.Length)
            {
                return 0;
            }

            // Варіант 1: пропустити поточну заявку
            int best = Backtrack(sorted, idx + 1, lastEnd);

            // Варіант 2: взяти заявку, якщо вона не перетинається з попередньою
            if (sorted[idx].Start >= lastEnd)
            {
                int take = 1 + Backtrack(sorted, idx + 1, sorted[idx].End);
                if (take > best)
                {
                    best = take;
                }
            }

            return best;
        }

        // 3. Неправильне правило 1: «хто почався раніше»
        public static int SolveWrongEarliestStart(RequestInterval[] requests)
        {
            if (requests.Length == 0) return 0;
            RequestInterval[] sorted = (RequestInterval[])requests.Clone();
            Array.Sort(sorted, (a, b) => a.Start.CompareTo(b.Start));

            int count = 0;
            int lastEnd = -1;
            for (int i = 0; i < sorted.Length; i++)
            {
                if (sorted[i].Start >= lastEnd)
                {
                    count++;
                    lastEnd = sorted[i].End;
                }
            }
            return count;
        }

        // 4. Неправильне правило 2: «хто коротший»
        public static int SolveWrongShortestDuration(RequestInterval[] requests)
        {
            if (requests.Length == 0) return 0;
            RequestInterval[] sorted = (RequestInterval[])requests.Clone();
            Array.Sort(sorted, (a, b) => a.Duration.CompareTo(b.Duration));

            List<RequestInterval> chosen = new List<RequestInterval>();
            for (int i = 0; i < sorted.Length; i++)
            {
                bool overlaps = false;
                for (int j = 0; j < chosen.Count; j++)
                {
                    if (sorted[i].Start < chosen[j].End && chosen[j].Start < sorted[i].End)
                    {
                        overlaps = true;
                        break;
                    }
                }
                if (!overlaps)
                {
                    chosen.Add(sorted[i]);
                }
            }
            return chosen.Count;
        }

        // Демонстрація контрприкладів до двох хибних правил
        public static void DemonstrateWrongRuleCounterexamples()
        {
            // Контрприклад 1: «Хто почався раніше»
            RequestInterval[] case1 = new RequestInterval[]
            {
                new RequestInterval(0, 40),
                new RequestInterval(2, 10),
                new RequestInterval(12, 20),
                new RequestInterval(22, 30)
            };
            int opt1 = SolveGreedy(case1).Count;
            int wrong1 = SolveWrongEarliestStart(case1);
            Console.WriteLine("Контрприклад 1 (правило «хто почався раніше»):");
            Console.WriteLine("  Заявки: [0; 40), [2; 10), [12; 20), [22; 30)");
            Console.WriteLine($"  Оптимум (раннє завершення): {opt1} заявки | Правило «почався раніше»: {wrong1} заявка\n");

            // Контрприклад 2: «Хто коротший»
            RequestInterval[] case2 = new RequestInterval[]
            {
                new RequestInterval(0, 10),
                new RequestInterval(10, 20),
                new RequestInterval(8, 12)
            };
            int opt2 = SolveGreedy(case2).Count;
            int wrong2 = SolveWrongShortestDuration(case2);
            Console.WriteLine("Контрприклад 2 (правило «хто коротший»):");
            Console.WriteLine("  Заявки: [0; 10), [10; 20), [8; 12)");
            Console.WriteLine($"  Оптимум (раннє завершення): {opt2} заявки | Правило «хто коротший»: {wrong2} заявка\n");
        }
    }
}