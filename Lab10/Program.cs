using System;
using System.IO;
using System.Text;

namespace Lab10
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
            Console.WriteLine("ЛАБОРАТОРНА РОБОТА 10. Обробка рядків і вибір алгоритму");
            Console.WriteLine($"Варіант: N = {N}, K = {K}, a = {N % 3}, b = {N % 4}, c = {N % 5}");
            Console.WriteLine("==================================================\n");

            // 1. Завантаження або створення файлу української прози (>= 50 000 символів)
            string prosePath = "prose.txt";
            EnsureProseFileExists(prosePath, 55000);
            string prose = File.ReadAllText(prosePath, Encoding.UTF8);
            Console.WriteLine($"Файл прози '{prosePath}' завантажено: {prose.Length} символів.\n");

            // 2. Прогрівальний прогін перед замірами
            Benchmark.Warmup(prose);

            // 3. Запуск усіх завдань та замірів
            Benchmark.RunTask1Benchmarks(prose);
            Benchmark.RunTask2Benchmarks(prose);
            Benchmark.RunTask3Benchmarks(prose);
            Benchmark.RunTask4Benchmarks(N);
            Benchmark.RunTaskCBenchmarks(prose);

            // 4. Перевірка крайових випадків
            RunEdgeCases();
        }

        private static void EnsureProseFileExists(string path, int minLength)
        {
            if (File.Exists(path))
            {
                string existing = File.ReadAllText(path, Encoding.UTF8);
                if (existing.Length >= minLength) return;
            }

            string[] paragraphs = new string[]
            {
                "Тихий вечір спускався над старим Подолом, огортаючи вузькі вулички прозорим серпанком осіннього спокою. Старі каштани шелестіли пожовклим листям, а на ґанку дерев'яної садиби тихо потріскував ліхтар.",
                "Кожен алгоритм у програмуванні нагадує ретельно прокладену стежку крізь густий ліс даних: один мандрівник перевіряє кожне дерево по черзі, витрачаючи дні, а інший користується компасом і картою, знаходячи шлях за лічені хвилини.",
                "У старій бібліотеці пахло пергаментом, друкарською фарбою та стиглим аґрусом із саду. На полицях стояли томи з історії, філософії та математики, де кожна сторінка зберігала думки минулих поколінь.",
                "Річка повільно несла свої хвилі поміж очеретів, відбиваючи перші зірки на вечірньому небі. Десь далеко за лугом озвався соловей, і його мелодія розлилася над тихою водою.",
                "Розуміння складності обчислень відрізняє ремісника від інженера: коли обсяг тексту зростає вдесятеро, квадратичний алгоритм сповільнюється в сто разів, тоді як лінійний залишається миттєвим.",
                "Ранковий вітер приніс свіжість із гірських полонин, розвіявши туман над долиною. Сонце освітило дахи будинків, бруківку площі та старий кам'яний міст через річку.",
                "У кожному тексті прихована своя внутрішня закономірність розподілу літер, слів і пауз, яку можна дослідити точними вимірюваннями та продуманими структурами даних."
            };

            StringBuilder sb = new StringBuilder(minLength + 1000);
            int idx = 0;
            while (sb.Length < minLength)
            {
                sb.AppendLine(paragraphs[idx % paragraphs.Length]);
                idx++;
            }

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        private static void RunEdgeCases()
        {
            Console.WriteLine("=== ПЕРЕВІРКА КРАЙОВИХ ВИПАДКІВ ===");

            // 1. Порожній рядок (зав. 2, 3)
            int uEmptyN = StringAlgorithms.LongestUniqueSubstringNaive("");
            int uEmptyF = StringAlgorithms.LongestUniqueSubstringFast("");
            int idxEmpty = StringAlgorithms.NaiveIndexOf("", "тест", out _);
            Console.WriteLine($"1. Порожній рядок: Зав.2 (Naive={uEmptyN}, Fast={uEmptyF}), Зав.3 (IndexOf={idxEmpty})");

            // 2. Рядок з одного символу (зав. 2)
            int uOneN = StringAlgorithms.LongestUniqueSubstringNaive("Я");
            int uOneF = StringAlgorithms.LongestUniqueSubstringFast("Я");
            Console.WriteLine($"2. Рядок з 1 символу: Зав.2 (Naive={uOneN}, Fast={uOneF})");

            // 3. Усі символи однакові (зав. 2)
            string sameStr = new string('М', 50);
            int uSameN = StringAlgorithms.LongestUniqueSubstringNaive(sameStr);
            int uSameF = StringAlgorithms.LongestUniqueSubstringFast(sameStr);
            Console.WriteLine($"3. Усі символи однакові: Зав.2 (Naive={uSameN}, Fast={uSameF})");

            // 4. Текст із пробілами на початку, в кінці й подвійними всередині (зав. 2)
            string spacesStr = "  абв  гдеж  ";
            int uSpN = StringAlgorithms.LongestUniqueSubstringNaive(spacesStr);
            int uSpF = StringAlgorithms.LongestUniqueSubstringFast(spacesStr);
            Console.WriteLine($"4. Пробіли на краях і подвійні всередині: Зав.2 (Naive={uSpN}, Fast={uSpF})");

            // 5. Шаблон довший за текст (зав. 3)
            int idxLongPat = StringAlgorithms.NaiveIndexOf("слово", "дужедовгийшаблон", out int cLong);
            Console.WriteLine($"5. Шаблон довший за текст: Зав.3 (IndexOf={idxLongPat}, порівнянь={cLong})");

            // 6. Порожній шаблон (зав. 3)
            int idxEmptyPat = StringAlgorithms.NaiveIndexOf("слово", "", out int cEmptyPat);
            Console.WriteLine($"6. Порожній шаблон: Зав.3 (IndexOf={idxEmptyPat}, порівнянь={cEmptyPat})");

            // 7. Нуль елементів і один елемент (зав. 4)
            RequestInterval[] zeroReq = new RequestInterval[0];
            RequestInterval[] oneReq = new RequestInterval[] { new RequestInterval(5, 12) };
            Console.WriteLine($"7. 0 та 1 заявка (Зав.4): 0 ел. -> (Greedy={SchedulingAlgorithms.SolveGreedy(zeroReq).Count}, Brute={SchedulingAlgorithms.SolveBruteForce(zeroReq)}), " +
                              $"1 ел. -> (Greedy={SchedulingAlgorithms.SolveGreedy(oneReq).Count}, Brute={SchedulingAlgorithms.SolveBruteForce(oneReq)})");
        }
    }
}