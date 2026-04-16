using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_1_part_C_asppr
{
    internal static class InputManager
    {
        public static Matrix AddZeroRowHeaders(Matrix matrix, int row)
        {
            string[] newRowHeaders = new string[matrix.Rows + 1];
            for (int i = 0; i < row; i++)
            {
                newRowHeaders[i] = matrix.RowHeaders[i];
            }
            newRowHeaders[row] = "0";
            for (int i = row; i < matrix.Rows; i++)
            {
                newRowHeaders[i + 1] = matrix.RowHeaders[i];
            }
            matrix.RowHeaders = newRowHeaders;
            return matrix;
        }

        public static Matrix InputMatrix()
        {
            int rows, cols;
            double[,] data;

            while (true)
            {
                try
                {
                    Console.Write("Введiть кiлькiсть рядкiв: ");
                    rows = int.Parse(Console.ReadLine());
                    Console.Write("Введiть кiлькiсть стовпцiв: ");
                    cols = int.Parse(Console.ReadLine());
                    data = new double[rows, cols];
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Помилка! Введено некоректне число. Спробуйте ще раз.");
                    continue;
                }
            }

            Console.WriteLine($"Введiть рядок матрицi:");
            for (int i = 0; i < rows; i++)
            {
                while (true)
                {
                    Console.Write($"Рядок {i + 1}: ");
                    string input = Console.ReadLine();
                    string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != cols)
                    {
                        Console.WriteLine($"Помилка! Ви ввели {parts.Length} чисел, а потрiбно {cols}. Спробуйте ще раз.");
                        continue;
                    }
                    try
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            data[i, j] = double.Parse(parts[j].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                        }
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Помилка! Введено некоректне число. Спробуйте ще раз.");
                    }
                }
            }
            Matrix matrix = new Matrix(data);

            return matrix;
        }

        public static double[] InputConstants()
        {
            int constNum;
            double[] constants;

            Console.Write("Введiть кiлькiсть констант: ");
            while (true)
            {
                try
                {
                    constNum = int.Parse(Console.ReadLine());
                    constants = new double[constNum];
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Помилка! Введено некоректне число. Спробуйте ще раз.");
                    continue;
                }
            }
            for (int i = 0; i < constNum; i++)
            {
                while (true)
                {
                    Console.Write($"Константа {i + 1}: ");
                    string input = Console.ReadLine();
                    try
                    {
                        constants[i] = double.Parse(input.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Помилка! Введено некоректне число. Спробуйте ще раз.");
                    }
                }
            }

            return constants;
        }
    }
}
