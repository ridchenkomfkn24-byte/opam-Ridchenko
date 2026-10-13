using System;

namespace Lab02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int n = 21;
            int k = 17;
            
            Console.WriteLine($"=== Варіант: N = {n}, K = {k} ===");
            Console.WriteLine();

            // Заповнення початкового масиву
            Random rnd = new Random(n);
            int[] array = new int[16];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rnd.Next(0, 6); // [0; 5] inclusive
            }

            // --- Завдання 1 ---
            Console.WriteLine("--- Завдання 1: Агрегація ---");
            PrintArray("Початковий масив", array);
            Task1_Aggregation(array);
            Console.WriteLine();

            // --- Завдання 2 ---
            Console.WriteLine("--- Завдання 2: Фільтрація ---");
            int threshold = k % 5; // 17 % 5 = 2
            int[] filtered = Task2_Filter(array, threshold);
            PrintArray($"Фільтрований масив (елементи > {threshold})", filtered);
            Console.WriteLine();

            // --- Завдання 3 ---
            Console.WriteLine("--- Завдання 3: Серії ---");
            Task3_Series(array);
            Console.WriteLine();

            // --- Завдання 4 ---
            Console.WriteLine("--- Завдання 4: Перестановка на місці ---");
            // Копіюємо масив, щоб не псувати оригінал для наочності
            int[] arrayForShift = CopyArray(array);
            PrintArray("До зсуву", arrayForShift);
            Task4_ShiftLeft(arrayForShift);
            PrintArray("Після циклічного зсуву вліво", arrayForShift);
            Console.WriteLine();

            // --- Завдання 5 ---
            Console.WriteLine("--- Завдання 5: Обробка матриці ---");
            int[,] matrix = GenerateMatrix(5, 4, n);
            PrintMatrix(matrix);
            Task5_MatrixProcessing(matrix);
            Console.WriteLine();

            // --- Завдання 6: Крайові випадки ---
            Console.WriteLine("--- Завдання 6: Перевірка крайових випадків ---");
            RunEdgeCasesTests();
        }

        #region Helper Methods
        static void PrintArray(string label, int[] arr)
        {
            Console.Write($"{label}: ");
            if (arr == null || arr.Length == 0)
            {
                Console.WriteLine("[] (порожній)");
                return;
            }
            Console.Write("[");
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i] + (i < arr.Length - 1 ? ", " : ""));
            }
            Console.WriteLine("]");
        }

        static int[] CopyArray(int[] source)
        {
            if (source == null) return null;
            int[] destination = new int[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                destination[i] = source[i];
            }
            return destination;
        }

        static int[,] GenerateMatrix(int rows, int cols, int seed)
        {
            Random rnd = new Random(seed);
            int[,] matrix = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = rnd.Next(0, 6);
                }
            }
            return matrix;
        }

        static void PrintMatrix(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            Console.WriteLine("Матриця:");
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write($"{matrix[i, j],4}");
                }
                Console.WriteLine();
            }
        }
        #endregion

        #region Tasks Implementation

        // Завдання 1: Агрегація за один прохід
        static void Task1_Aggregation(int[] arr)
        {
            if (arr == null || arr.Length == 0)
            {
                Console.WriteLine("Помилка: Масив порожній. Результат агрегації не визначений.");
                return;
            }

            int sum = 0;
            int min = arr[0];
            int max = arr[0];
            int minIndex = 0;
            int maxIndex = 0;
            int zeroCount = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                int val = arr[i];
                sum += val;

                if (val < min)
                {
                    min = val;
                    minIndex = i;
                }

                // Вказуємо '>', щоб зберегти індекс ПЕРШОГО входження максимуму
                if (val > max)
                {
                    max = val;
                    maxIndex = i;
                }

                if (val == 0)
                {
                    zeroCount++;
                }
            }

            double average = (double)sum / arr.Length;

            Console.WriteLine($"Сума: {sum}");
            Console.WriteLine($"Середнє арифметичне: {average:F2}");
            Console.WriteLine($"Мінімум: {min} (індекс: {minIndex})");
            Console.WriteLine($"Максимум: {max} (індекс першого входження: {maxIndex})");
            Console.WriteLine($"Кількість нулів: {zeroCount}");
        }

        // Завдання 2: Фільтрація в новий масив (елементи > threshold)
        static int[] Task2_Filter(int[] arr, int threshold)
        {
            if (arr == null || arr.Length == 0)
            {
                return new int[0];
            }

            // Прохід 1: підрахунок кількості відповідних елементів
            int count = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] > threshold)
                {
                    count++;
                }
            }

            // Створення масиву точного розміру
            int[] result = new int[count];

            // Прохід 2: заповнення відфільтрованого масиву
            int index = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] > threshold)
                {
                    result[index] = arr[i];
                    index++;
                }
            }

            return result;
        }

        // Завдання 3: Найдовша серія однакових елементів
        static void Task3_Series(int[] arr)
        {
            if (arr == null || arr.Length == 0)
            {
                Console.WriteLine("Помилка: Масив порожній. Серії відсутні.");
                return;
            }

            int bestValue = arr[0];
            int bestLength = 1;
            int bestStartIndex = 0;

            int currentLength = 1;
            int currentStartIndex = 0;

            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] == arr[i - 1])
                {
                    currentLength++;
                }
                else
                {
                    if (currentLength > bestLength)
                    {
                        bestLength = currentLength;
                        bestValue = arr[i - 1];
                        bestStartIndex = currentStartIndex;
                    }
                    currentLength = 1;
                    currentStartIndex = i;
                }
            }

            // Окреме перевірочне збереження для серії, що закінчується на останньому елементі
            if (currentLength > bestLength)
            {
                bestLength = currentLength;
                bestValue = arr[arr.Length - 1];
                bestStartIndex = currentStartIndex;
            }

            Console.WriteLine($"Найдовша серія:");
            Console.WriteLine($"Значення: {bestValue}, Довжина: {bestLength}, Початковий індекс: {bestStartIndex}");
        }

        // Завдання 4: Перестановка на місці (циклічний зсув вліво)
        static void Task4_ShiftLeft(int[] arr)
        {
            if (arr == null || arr.Length <= 1)
            {
                // Порожній або одноелементний масив не потребує змін
                return;
            }

            int temp = arr[0];
            for (int i = 0; i < arr.Length - 1; i++)
            {
                arr[i] = arr[i + 1];
            }
            arr[arr.Length - 1] = temp;
        }

        // Завдання 5: Обробка матриці
        static void Task5_MatrixProcessing(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            int maxRowSum = int.MinValue;
            int bestRowIndex = -1;

            Console.WriteLine("Сума елементів кожного рядка:");
            for (int i = 0; i < rows; i++)
            {
                int rowSum = 0;
                for (int j = 0; j < cols; j++)
                {
                    rowSum += matrix[i, j];
                }
                Console.WriteLine($"  Рядок {i}: сума = {rowSum}");

                if (rowSum > maxRowSum)
                {
                    maxRowSum = rowSum;
                    bestRowIndex = i;
                }
            }

            Console.WriteLine("\nМаксимум кожного стовпця:");
            for (int j = 0; j < cols; j++)
            {
                int colMax = matrix[0, j];
                for (int i = 1; i < rows; i++)
                {
                    if (matrix[i, j] > colMax)
                    {
                        colMax = matrix[i, j];
                    }
                }
                Console.WriteLine($"  Стовпець {j}: макс = {colMax}");
            }

            Console.WriteLine($"\nНомер рядка з найбільшою сумою: {bestRowIndex} (сума: {maxRowSum})");
        }

        // Завдання 6: Прогонка крайових випадків
        static void RunEdgeCasesTests()
        {
            Console.WriteLine("=== ТЕСТ 1: Порожній масив ===");
            int[] empty = new int[0];
            Task1_Aggregation(empty);
            int[] filteredEmpty = Task2_Filter(empty, 2);
            PrintArray("Завдання 2 (порожній)", filteredEmpty);
            Task3_Series(empty);
            Task4_ShiftLeft(empty);
            PrintArray("Завдання 4 (порожній)", empty);

            Console.WriteLine("\n=== ТЕСТ 2: Один елемент ===");
            int[] single = new int[] { 4 };
            PrintArray("Вхідний масив", single);
            Task1_Aggregation(single);
            int[] filteredSingle = Task2_Filter(single, 2);
            PrintArray("Завдання 2 (один елемент > 2)", filteredSingle);
            Task3_Series(single);
            Task4_ShiftLeft(single);
            PrintArray("Завдання 4 (після зсуву)", single);

            Console.WriteLine("\n=== ТЕСТ 3: Усі елементи однакові ===");
            int[] same = new int[] { 3, 3, 3, 3, 3 };
            PrintArray("Вхідний масив", same);
            Task1_Aggregation(same);
            Task3_Series(same);
        }
        #endregion
    }
}