using System;
using System.Collections.Generic;
using System.Text;

namespace Lab05
{
    // === Завдання 4: Власний стек поверх масиву ===
    public class CustomCharStack
    {
        private char[] _array;
        private int _count;

        public CustomCharStack(int initialCapacity)
        {
            if (initialCapacity <= 0) initialCapacity = 3;
            _array = new char[initialCapacity];
            _count = 0;
        }

        public int Count => _count;
        public int Capacity => _array.Length;

        public void Push(char x)
        {
            if (_count == _array.Length)
            {
                Array.Resize(ref _array, _array.Length * 2);
            }
            _array[_count] = x;
            _count++;
        }

        public char Pop()
        {
            if (_count == 0)
            {
                throw new InvalidOperationException("Стек порожній!");
            }
            _count--;
            return _array[_count];
        }

        public char Peek()
        {
            if (_count == 0)
            {
                throw new InvalidOperationException("Стек порожній!");
            }
            return _array[_count - 1];
        }
    }

    // Клас заявки для Завдання 3
    public class Request
    {
        public int Id { get; set; }
        public int Weight { get; set; }
        public int ArrivalTime { get; set; }
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
            Console.WriteLine($"ЛАБОРАТОРНА РОБОТА 5. Алгоритми на стеку та черзі");
            Console.WriteLine($"Варіант: N = {N}, K = {K}, a = {N % 3}, b = {N % 4}, c = {N % 5}");
            Console.WriteLine("==================================================\n");

            // --- Завдання 1 ---
            Console.WriteLine("=== Завдання 1: Збалансованість дужок (Standart Stack) ===");
            string[] testBrackets = new string[]
            {
                "(a[b]{c})",
                "([)]",
                ")(",
                "((",
                "",
                "(((((((((((((((((" + "))))))))))))))))" // 17 відкривальних, 16 закривальних
            };

            foreach (string str in testBrackets)
            {
                bool isBalanced = Task1_CheckBrackets(str, out int errIndex);
                Console.WriteLine($"Рядок: \"{str}\" -> Збалансований: {isBalanced}, Індекс помилки: {errIndex}");
            }
            Console.WriteLine();

            // --- Завдання 2 ---
            Console.WriteLine("=== Завдання 2: Зворотний польський запис (RPN) ===");
            string[] rpnTests = new string[]
            {
                "3 4 + 2 *",
                "5 3 -",
                "8 2 /",
                "2 3 4 * +",
                "5 0 /",
                "5 4 * 10 7 % -" // Власний вираз для K = 17
            };

            foreach (string expr in rpnTests)
            {
                Console.Write($"Вираз: \"{expr}\" -> ");
                if (Task2_EvaluateRPN(expr, out double result, out string errorMsg))
                {
                    Console.WriteLine($"Результат: {result}");
                }
                else
                {
                    Console.WriteLine($"Помилка: {errorMsg}");
                }
            }
            Console.WriteLine();

            // --- Завдання 3 ---
            Console.WriteLine("=== Завдання 3: Модель обслуговування (Черга, c = 1) ===");
            Task3_SimulateQueue(N, K);
            Console.WriteLine();

            // --- Завдання 4 ---
            Console.WriteLine("=== Завдання 4: Перевірка власного стеку ===");
            int initCapacity = K % 4 + 2; // 17 % 4 + 2 = 3
            CustomCharStack customStack = new CustomCharStack(initCapacity);
            Console.WriteLine($"Початкова місткість: {customStack.Capacity}");

            char[] testChars = new char[] { 'A', 'B', 'C', 'D' };
            for (int i = 0; i < testChars.Length; i++)
            {
                customStack.Push(testChars[i]);
                Console.WriteLine($"Push('{testChars[i]}') -> Count: {customStack.Count}, Capacity: {customStack.Capacity}");
            }

            Console.WriteLine("\nПеревірка Завдання 1 на власному стеку:");
            foreach (string str in testBrackets)
            {
                bool isBalancedCustom = Task1_CheckBracketsCustomStack(str, out int errIndexCustom);
                Console.WriteLine($"Рядок: \"{str}\" -> Збалансований: {isBalancedCustom}, Індекс помилки: {errIndexCustom}");
            }
        }

        // --- Завдання 1: Стандартний Stack<T> ---
        public static bool Task1_CheckBrackets(string input, out int errorIndex)
        {
            Stack<int> stackIndices = new Stack<int>();
            Stack<char> stackChars = new Stack<char>();

            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];

                if (ch == '(' || ch == '[' || ch == '{')
                {
                    stackChars.Push(ch);
                    stackIndices.Push(i);
                }
                else if (ch == ')' || ch == ']' || ch == '}')
                {
                    if (stackChars.Count == 0)
                    {
                        errorIndex = i;
                        return false;
                    }

                    char top = stackChars.Pop();
                    stackIndices.Pop();

                    if ((ch == ')' && top != '(') ||
                        (ch == ']' && top != '[') ||
                        (ch == '}' && top != '{'))
                    {
                        errorIndex = i;
                        return false;
                    }
                }
            }

            if (stackChars.Count > 0)
            {
                errorIndex = input.Length;
                return false;
            }

            errorIndex = -1;
            return true;
        }

        // --- Завдання 1: Власний CustomCharStack ---
        public static bool Task1_CheckBracketsCustomStack(string input, out int errorIndex)
        {
            CustomCharStack stackChars = new CustomCharStack(3);
            Stack<int> stackIndices = new Stack<int>();

            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];

                if (ch == '(' || ch == '[' || ch == '{')
                {
                    stackChars.Push(ch);
                    stackIndices.Push(i);
                }
                else if (ch == ')' || ch == ']' || ch == '}')
                {
                    if (stackChars.Count == 0)
                    {
                        errorIndex = i;
                        return false;
                    }

                    char top = stackChars.Pop();
                    stackIndices.Pop();

                    if ((ch == ')' && top != '(') ||
                        (ch == ']' && top != '[') ||
                        (ch == '}' && top != '{'))
                    {
                        errorIndex = i;
                        return false;
                    }
                }
            }

            if (stackChars.Count > 0)
            {
                errorIndex = input.Length;
                return false;
            }

            errorIndex = -1;
            return true;
        }

        // --- Завдання 2: Обчислення RPN ---
        public static bool Task2_EvaluateRPN(string expression, out double result, out string errorMessage)
        {
            result = 0;
            errorMessage = "";

            if (string.IsNullOrWhiteSpace(expression))
            {
                errorMessage = "Порожній вираз!";
                return false;
            }

            string[] tokens = expression.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Stack<double> stack = new Stack<double>();

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out double num))
                {
                    stack.Push(num);
                }
                else if (token == "+" || token == "-" || token == "*" || token == "/" || token == "%")
                {
                    if (stack.Count < 2)
                    {
                        errorMessage = $"Забракло чисел для операції '{token}'";
                        return false;
                    }

                    double b = stack.Pop();
                    double a = stack.Pop();

                    if ((token == "/" || token == "%") && b == 0)
                    {
                        errorMessage = "Ділення або остача від ділення на нуль!";
                        return false;
                    }

                    double opResult = 0;
                    switch (token)
                    {
                        case "+": opResult = a + b; break;
                        case "-": opResult = a - b; break;
                        case "*": opResult = a * b; break;
                        case "/": opResult = a / b; break;
                        case "%": opResult = a % b; break;
                    }
                    stack.Push(opResult);
                }
                else
                {
                    errorMessage = $"Невідомий токен '{token}'";
                    return false;
                }
            }

            if (stack.Count != 1)
            {
                errorMessage = "Некоректний вираз (забагато чисел)";
                return false;
            }

            result = stack.Pop();
            return true;
        }

        // --- Завдання 3: Черга обслуговування (c = 1) ---
        public static void Task3_SimulateQueue(int N, int K)
        {
            int requestCount = 8 + K % 5; // 8 + 2 = 10
            Random rnd = new Random(N);

            Queue<Request> queue = new Queue<Request>();
            int currentArrivalTime = 0;

            for (int i = 1; i <= requestCount; i++)
            {
                currentArrivalTime += rnd.Next(0, 4); // Час надходження
                int weight = rnd.Next(1, 10);          // Обсяг роботи (1..9 хв)

                queue.Enqueue(new Request
                {
                    Id = i,
                    Weight = weight,
                    ArrivalTime = currentArrivalTime
                });
            }

            int currentTime = 0;
            int totalIdleTime = 0;
            int maxQueueLength = queue.Count;

            Console.WriteLine("----------------------------------------------------------------------------------");
            Console.WriteLine("| Заявка # | Вага (хв) | Час надходження | Час початку | Час завершення | Простій (хв) |");
            Console.WriteLine("----------------------------------------------------------------------------------");

            while (queue.Count > 0)
            {
                if (queue.Count > maxQueueLength)
                {
                    maxQueueLength = queue.Count;
                }

                Request req = queue.Dequeue();

                int idle = 0;
                if (currentTime < req.ArrivalTime)
                {
                    idle = req.ArrivalTime - currentTime;
                    totalIdleTime += idle;
                    currentTime = req.ArrivalTime;
                }

                int startTime = currentTime;
                int finishTime = startTime + req.Weight;
                currentTime = finishTime;

                Console.WriteLine($"| {req.Id,8} | {req.Weight,9} | {req.ArrivalTime,15} | {startTime,11} | {finishTime,14} | {idle,12} |");
            }

            Console.WriteLine("----------------------------------------------------------------------------------");
            Console.WriteLine($"Максимальна довжина черги: {maxQueueLength}");
            Console.WriteLine($"Загальний час простою обслуговувача: {totalIdleTime} хв");
        }
    }
}