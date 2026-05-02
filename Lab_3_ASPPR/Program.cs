using Lab_1_part_C_asppr;
using Lab_1_part_D_asppr;
using System.Security.Cryptography;

int choice = 0;
Matrix? optimalMatrix = null;
Matrix? withoutZeros = null;

void ReverseMatrix()
{
    Matrix matrix = InputManager.InputMatrix();
    matrix.InitializeHeaders();

    if (matrix.Rows != matrix.Columns)
    {
        Console.WriteLine("Помилка! Матриця повинна бути квадратною.");
        return;
    }

    InverseMatrixCalculator eliminator = new InverseMatrixCalculator();
    List<Matrix> iterations = eliminator.Eliminate(matrix);

    int iterationsCount = 1;
    foreach (Matrix m in iterations)
    {
        Console.WriteLine($"\nРозв\'язок для {iterationsCount} елементу дiагоналi:");
        m.PrintMatrix();
        iterationsCount++;
    }
}

void SystemSolution()
{
    Matrix matrix = InputManager.InputMatrix();
    double[] constants = InputManager.InputConstants();
    matrix.InitializeHeaders();

    InverseMatrixCalculator eliminator = new InverseMatrixCalculator();
    List<Matrix> iterations = eliminator.Eliminate(matrix);

    SystemCalculator systemCalculator = new SystemCalculator();
    double[] solutions = systemCalculator.Calculate(matrix, constants);

    int iterationsCount = 1;
    foreach (Matrix m in iterations)
    {
        Console.WriteLine($"\nРозв\'язок для {iterationsCount} елементу дiагоналi:");
        m.PrintMatrix();
        iterationsCount++;
    }

    int solutionIndex = 1;
    Console.WriteLine("\nРозв\'язок системи:");
    foreach (double n in solutions)
    {
        Console.WriteLine($"X[{solutionIndex}]: " + n);
        solutionIndex++;
    }
}

void RankMatrix()
{
    Matrix matrix = InputManager.InputMatrix();
    matrix.InitializeHeaders();

    RankCalculator rankCalculator = new RankCalculator();

    List<Matrix> iterations = rankCalculator.CalculateRank(matrix);

    int iterationsCount = 1;
    foreach (Matrix m in iterations)
    {
        Console.WriteLine($"\nРозв\'язок для {iterationsCount} елементу дiагоналi:");
        m.PrintMatrix();
        iterationsCount++;
    }

    Console.WriteLine("\nРанг матрицi: " + matrix.Rank);
}

int FindRowWithNegativeB(Matrix matrix)
{
    for (int i = 0; i < matrix.Rows - 1; i++)
    {
        if (matrix[i, matrix.Columns - 1] < 0)
        {
            return i;
        }
    }
    return -1;
}

int FindNegativeInRow(Matrix matrix, int row)
{
    for (int j = 0; j < matrix.Columns - 1; j++)
    {
        if (matrix[row, j] < 0)
        {
            return j;
        }
    }
    return -1;
}

int FindPositiveInRow(Matrix matrix, int row)
{
    for (int j = 0; j < matrix.Columns - 1; j++)
    {
        if (matrix[row, j] > 0)
        {
            return j;
        }
    }
    return -1;
}

bool IsLastColumnNegative(Matrix matrix)
{
    for (int i = 0; i < matrix.Rows - 1; i++)
    {
        if (matrix[i, matrix.Columns - 1] < 0)
        {
            return true;
        }
    }
    return false;
}

double[] GetResultXdouble(Matrix matrix)
{
    int maxIndex = 0;
    var allHeaders = matrix.RowHeaders.Concat(matrix.ColumnHeaders);
    foreach (var header in allHeaders)
    {
        if (header.StartsWith("x"))
        {
            if (int.TryParse(header.Substring(1), out int index))
            {
                if (index > maxIndex) maxIndex = index;
            }
        }
    }
    if (maxIndex == 0) return new double[0];
    double[] xValues = new double[maxIndex];
    for (int i = 0; i < matrix.Rows - 1; i++)
    {
        string header = matrix.RowHeaders[i];
        if (header.StartsWith("x"))
        {
            if (int.TryParse(header.Substring(1), out int index))
            {
                xValues[index - 1] = matrix[i, matrix.Columns - 1];
            }
        }
    }
    return xValues;
}

double[] GetResultXdoubleForFindFractial(Matrix matrix)
{
    double[] xValues = new double[matrix.Rows];

    for (int i = 0; i < matrix.Rows - 1; i++)
    {
        string header = matrix.RowHeaders[i];
        if (header.StartsWith("x"))
        {
            xValues[i] = matrix[i, matrix.Columns - 1];
        }
    }

    return xValues;
}

string GetResultX(Matrix matrix)
{
    double[] xValues = GetResultXdouble(matrix);
    string result = "X = (" + string.Join("; ", xValues.Select(v => v.ToString("F5"))) + ")";
    return result;
}

double[] GetResultUdouble(Matrix matrix)
{
    int maxIndex = 0;
    var allHeaders = matrix.DualRowHeaders.Concat(matrix.DualColumnHeaders);
    foreach (var header in allHeaders)
    {
        if (header.StartsWith("u"))
        {
            if (int.TryParse(header.Substring(1), out int index))
            {
                if (index > maxIndex) maxIndex = index;
            }
        }
    }
    if (maxIndex == 0) return new double[0];
    double[] uValues = new double[maxIndex];
    for (int i = 0; i < matrix.Columns - 1; i++)
    {
        string header = matrix.DualColumnHeaders[i];
        if (header.StartsWith("u"))
        {
            if (int.TryParse(header.Substring(1), out int index))
            {
                uValues[index - 1] = matrix[matrix.Rows - 1, i];
            }
        }
    }
    return uValues;
}

string GetResultU(Matrix matrix)
{
    double[] uValues = GetResultUdouble(matrix);
    string result = "U = (" + string.Join("; ", uValues.Select(v => v.ToString("F5"))) + ")";
    return result;
}

int OptionalMinNotNegative(Matrix matrix, int col)
{
    int r = -1;
    double minValue = double.MaxValue;
    int lastCol = matrix.Columns - 1;

    for (int i = 0; i < matrix.Rows - 1; i++)
    {
        double element = matrix[i, col];
        double freeTerm = matrix[i, lastCol];

        if (element > 0)
        {
            double ratio = freeTerm / element;

            if (ratio < minValue)
            {
                minValue = ratio;
                r = i;
            }
        }
    }

    return r;
}

int MinNotNegative(Matrix matrix, int col)
{
    int r = -1;
    double minValue = double.MaxValue;
    int lastCol = matrix.Columns - 1;

    for (int i = 0; i < matrix.Rows - 1; i++)
    {
        double element = matrix[i, col];
        double freeTerm = matrix[i, lastCol];

        if (element != 0)
        {
            double ratio = freeTerm / element;

            if (ratio >= 0 && ratio < minValue)
            {
                minValue = ratio;
                r = i;
            }
        }
    }

    return r;
}

int FindNegativeInZRow(Matrix matrix)
{
    int lastRow = matrix.Rows - 1;
    for (int j = 0; j < matrix.Columns - 1; j++)
    {
        if (matrix[lastRow, j] < 0) return j;
    }
    return -1;
}

Matrix DeleteZeroRows()
{
    ModifiedMatrixCalculator eliminator = new ModifiedMatrixCalculator();
    Matrix matrix = InputManager.InputMatrix();
    matrix.InitializeHeaders();

    Console.Write("Вкажiть нульовi рядки: ");
    string? input = Console.ReadLine();
    string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    int[] zeroRows = new int[parts.Length];

    for (int i = 0; i < parts.Length; i++)
    {
        if (int.TryParse(parts[i], out int rowIndex))
        {
            zeroRows[i] = rowIndex - 1;
        }
        else
        {
            Console.WriteLine($"Помилка! '{parts[i]}' не є числом. Спробуйте ще раз.");
            return null;
        }
    }

    for (int i = 0; i < zeroRows.Length; i++)
    {
        if (zeroRows[i] < 0 || zeroRows[i] >= matrix.Rows)
        {
            Console.WriteLine($"Помилка! Рядок {zeroRows[i] + 1} виходить за межi матрицi");
            return null;
        }

        if (matrix.RowHeaders[zeroRows[i]] != "Z")
        {
            matrix.RowHeaders[zeroRows[i]] = "0";
        }
        else
        {
            Console.WriteLine("Помилка! Неможливо змінити рядок Z.");
            return null;
        }
    }

    matrix.UpdateYHeaders();

    int zeroRowIteration = 0;

    while (zeroRows.Count() > zeroRowIteration)
    {
        int column = FindPositiveInRow(matrix, zeroRows[zeroRowIteration]);

        int r = MinNotNegative(matrix, column);

        eliminator.Calculate(matrix, r, column);
        matrix.SwapHeaders(r, column);

        if (matrix.ColumnHeaders[column] == "0")
        {
            matrix = matrix.FilterColumn(column);
            zeroRowIteration++;
        }
        Console.WriteLine($"\nПромiжна таблиця: (елемент {matrix.ColumnHeaders[column]}, {matrix.RowHeaders[r]})");
        matrix.PrintMatrix();
    }

    return matrix;
}

Matrix DualInputDeleteZeroRows(Matrix matrix)
{
    ModifiedMatrixCalculator eliminator = new ModifiedMatrixCalculator();

    Console.Write("Вкажiть нульовi рядки: ");
    string? input = Console.ReadLine();
    string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    int[] zeroRows = new int[parts.Length];

    for (int i = 0; i < parts.Length; i++)
    {
        if (int.TryParse(parts[i], out int rowIndex))
        {
            zeroRows[i] = rowIndex - 1;
        }
        else
        {
            Console.WriteLine($"Помилка! '{parts[i]}' не є числом. Спробуйте ще раз.");
            return null;
        }
    }

    for (int i = 0; i < zeroRows.Length; i++)
    {
        if (zeroRows[i] < 0 || zeroRows[i] >= matrix.Rows)
        {
            Console.WriteLine($"Помилка! Рядок {zeroRows[i] + 1} виходить за межi матрицi");
            return null;
        }

        if (matrix.RowHeaders[zeroRows[i]] != "Z")
        {
            matrix.RowHeaders[zeroRows[i]] = "0";
        }
        else
        {
            Console.WriteLine("Помилка! Неможливо змінити рядок Z.");
            return null;
        }
    }

    matrix.UpdateYHeaders();

    int zeroRowIteration = 0;

    while (zeroRows.Count() > zeroRowIteration)
    {
        int column = FindPositiveInRow(matrix, zeroRows[zeroRowIteration]);

        int r = MinNotNegative(matrix, column);

        eliminator.Calculate(matrix, r, column);
        matrix.SwapHeaders(r, column);
        matrix.SwapDualHeaders(r, column);

        if (matrix.ColumnHeaders[column] == "0")
        {
            matrix.AddOperationULog(r, column);
            Console.WriteLine("U log:");
            foreach (string[,] log in matrix.OperationsULog)
            {
                for (int i = 0; i < log.GetLength(0); i++)
                {
                    for (int j = 0; j < log.GetLength(1); j++)
                    {
                        Console.Write(log[i, j]);
                        if (j < log.GetLength(1) - 1) Console.Write(" ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine(); // separate logs visually
            }
            matrix = matrix.FilterColumn(column);
            zeroRowIteration++;
        }
        Console.WriteLine($"\nПромiжна таблиця: (елемент {matrix.ColumnHeaders[column]}, {matrix.RowHeaders[r]})");
        matrix.PrintMatrixWithDualHeaders();
    }

    return matrix;
}

Matrix FindOptimalSolution()
{
    ModifiedMatrixCalculator optimalSolution = new ModifiedMatrixCalculator();

    Matrix matrix = optimalMatrix.Clone();

    while (true)
    {
        int s = FindNegativeInZRow(matrix);

        if (s == -1)
        {
            Console.WriteLine("Оптимальний розв'язок знайдено");
            return matrix;
        }

        int r = OptionalMinNotNegative(matrix, s);

        if (r == -1)
        {
            Console.WriteLine("Цiльова функцiя необмежена ");
            return null;
        }

        matrix = optimalSolution.Calculate(matrix, r, s);

        matrix.SwapHeaders(r, s);

        Console.WriteLine($"\nПромiжна таблиця: (елемент {matrix.ColumnHeaders[s]}, {matrix.RowHeaders[r]}) ");
        matrix.PrintMatrix();
    }
}

Matrix InputFindOptimalSolution(Matrix matrix)
{
    ModifiedMatrixCalculator optimalSolution = new ModifiedMatrixCalculator();

    while (true)
    {
        int s = FindNegativeInZRow(matrix);

        if (s == -1)
        {
            Console.WriteLine("Оптимальний розв'язок знайдено");
            return matrix;
        }

        int r = OptionalMinNotNegative(matrix, s);

        if (r == -1)
        {
            Console.WriteLine("Цiльова функцiя необмежена ");
            return null;
        }

        matrix = optimalSolution.Calculate(matrix, r, s);

        matrix.SwapHeaders(r, s);

        Console.WriteLine($"\nПромiжна таблиця: (елемент {matrix.ColumnHeaders[s]}, {matrix.RowHeaders[r]}) ");
        matrix.PrintMatrix();
    }
}

Matrix DualInputFindOptimalSolution(Matrix matrix)
{
    ModifiedMatrixCalculator optimalSolution = new ModifiedMatrixCalculator();

    while (true)
    {
        int s = FindNegativeInZRow(matrix);

        if (s == -1)
        {
            Console.WriteLine("Оптимальний розв'язок знайдено");
            return matrix;
        }

        int r = OptionalMinNotNegative(matrix, s);

        if (r == -1)
        {
            Console.WriteLine("Цiльова функцiя необмежена ");
            return null;
        }

        matrix = optimalSolution.Calculate(matrix, r, s);

        matrix.SwapHeaders(r, s);
        matrix.SwapDualHeaders(r, s);

        Console.WriteLine($"\nПромiжна таблиця: (елемент {matrix.ColumnHeaders[s]}, {matrix.RowHeaders[r]}) ");
        matrix.PrintMatrixWithDualHeaders();
    }
}

Matrix FindReferenceSolution()
{
    Matrix matrix = InputManager.InputMatrix();
    if (matrix == null) return null;

    matrix.InitializeHeaders();

    ModifiedMatrixCalculator referenceSolution = new ModifiedMatrixCalculator();

    while (IsLastColumnNegative(matrix))
    {
        int targetRow = FindRowWithNegativeB(matrix);

        if (targetRow == -1)
        {
            break;
        }

        int s = FindNegativeInRow(matrix, targetRow);

        if (s == -1)
        {
            Console.WriteLine("Система обмежень є суперечливою");
            return null;
        }

        int r = MinNotNegative(matrix, s);

        if (r == -1)
        {
            Console.WriteLine("Неможливо знайти розв\'язувальний рядок");
            return null;
        }

        matrix = referenceSolution.Calculate(matrix, r, s);
        matrix.SwapHeaders(r, s);
        Console.WriteLine("\nПромiжна таблиця");
        matrix.PrintMatrix();
    }

    Console.WriteLine("Опорний розв\'язок знайдено");
    matrix.PrintMatrix();
    return matrix;
}

Matrix InputFindReferenceSolution(Matrix matrix)
{
    if (matrix == null) return null;

    ModifiedMatrixCalculator referenceSolution = new ModifiedMatrixCalculator();

    while (IsLastColumnNegative(matrix))
    {
        int targetRow = FindRowWithNegativeB(matrix);

        if (targetRow == -1)
        {
            break;
        }

        int s = FindNegativeInRow(matrix, targetRow);

        if (s == -1)
        {
            Console.WriteLine("Система обмежень є суперечливою");
            return null;
        }

        int r = MinNotNegative(matrix, s);

        if (r == -1)
        {
            Console.WriteLine("Неможливо знайти розв\'язувальний рядок");
            return null;
        }

        matrix = referenceSolution.Calculate(matrix, r, s);
        matrix.SwapHeaders(r, s);
        Console.WriteLine("\nПромiжна таблиця");
        matrix.PrintMatrix();
    }

    Console.WriteLine("Опорний розв\'язок знайдено");
    return matrix;
}

Matrix DualInputFindReferenceSolution(Matrix matrix)
{
    if (matrix == null) return null;

    ModifiedMatrixCalculator referenceSolution = new ModifiedMatrixCalculator();

    while (IsLastColumnNegative(matrix))
    {
        int targetRow = FindRowWithNegativeB(matrix);

        if (targetRow == -1)
        {
            break;
        }

        int s = FindNegativeInRow(matrix, targetRow);

        if (s == -1)
        {
            Console.WriteLine("Система обмежень є суперечливою");
            return null;
        }

        int r = MinNotNegative(matrix, s);

        if (r == -1)
        {
            Console.WriteLine("Неможливо знайти розв\'язувальний рядок");
            return null;
        }

        matrix = referenceSolution.Calculate(matrix, r, s);
        matrix.SwapHeaders(r, s);
        matrix.SwapDualHeaders(r, s);
        Console.WriteLine("\nПромiжна таблиця");
        matrix.PrintMatrixWithDualHeaders();
    }

    Console.WriteLine("Опорний розв\'язок знайдено");
    return matrix;
}

void ShowReferenceSolution()
{
    Matrix referenceSolution = FindReferenceSolution();

    optimalMatrix = referenceSolution;

    string X = GetResultX(referenceSolution);
    Console.WriteLine("\nОпорний розв\'язок:");
    Console.WriteLine(X);
}

void InputShowReferenceSolution(Matrix matrix)
{
    optimalMatrix = InputFindReferenceSolution(matrix);

    string X = GetResultX(matrix);
    Console.WriteLine("\nОпорний розв\'язок:");
    Console.WriteLine(X);
}

void ShowOptimalSolution()
{
    if (optimalMatrix == null)
    {
        Console.WriteLine("Спочатку знайдіть опорний розв\'язок.");
        return;
    }
    Matrix optimalSolution = FindOptimalSolution();
    if (optimalSolution != null)
    {
        string X = GetResultX(optimalSolution);
        Console.WriteLine("\nОптимальний розв\'язок:");
        Console.WriteLine(X);
        optimalSolution.ShowMaxZ();
    }
}

double[] GetRowValues(Matrix matrix, int row)
{
    double[] rowValues = new double[matrix.Columns];

    for (int i = 0; i < matrix.Columns; i++)
    {
        rowValues[i] = matrix[row, i];
    }

    return rowValues;
}

double[] GetSСonstraints(double[] xRowValues)
{
    for (int i = 0; i < xRowValues.Length - 1; i++)
    {
        xRowValues[i] = -1 * (xRowValues[i] - Math.Floor(xRowValues[i]));
    }

    xRowValues[xRowValues.Length - 1] = -Math.Abs(xRowValues[xRowValues.Length - 1] - Math.Floor(xRowValues[xRowValues.Length - 1]));

    return xRowValues;
}

void FindIntSolution()
{
    int counter = 1;
    Matrix matrix = InputManager.InputMatrix();
    if (matrix == null) return;

    matrix.InitializeHeaders();

    while (true)
    {
        Console.WriteLine($"\nПОШУК ЦIЛОЧИСЕЛЬНОГО РОЗВ\'ЯЗКУ (крок {counter}):");

        Console.WriteLine("Пошук опорного розв\'язку...");    
        matrix = InputFindReferenceSolution(matrix);
        matrix.PrintMatrix();
        Console.WriteLine(GetResultX(matrix));
        
        Console.WriteLine("\nПошук оптимального розв\'язку...");
        matrix = InputFindOptimalSolution(matrix);
        matrix.PrintMatrix();
        Console.WriteLine(GetResultX(matrix));
        matrix.ShowMaxZ();

        double[] xValues = GetResultXdoubleForFindFractial(matrix);

        if (xValues.All(v => Math.Abs(v - Math.Round(v)) < 1e-9))
        {
            Console.WriteLine("\nЦiлочисельний розв\'язок знайдено:");
            Console.WriteLine(GetResultX(matrix));
            matrix.ShowMaxZ();
            break;
        }

        int maxFractialRow = GomoryAlgorithm.GetXWithMaxFractialPart(xValues);
        Console.WriteLine($"\nРядок з найбiльшою дробовою частиною: {matrix.RowHeaders[maxFractialRow]}");
        
        double[] xRowValues = GetRowValues(matrix, maxFractialRow);
        double[] sConstraints = GetSСonstraints(xRowValues);

        Console.WriteLine("\nДодаткове обмеження:");
        matrix = matrix.AddSRow(sConstraints);
        Console.WriteLine($"s{matrix.SRowIndex} = " + string.Join("; ", sConstraints.Select(v => v.ToString("F2"))));
        
        Console.WriteLine($"\nПромiжна таблиця з додатковим обмеженням s{matrix.SRowIndex}:");
        matrix.PrintMatrix();

        counter++;
    }
}

void FindDoubleSolution()
{
    Matrix matrix = InputManager.InputMatrix();

    if (matrix == null) return;

    matrix.InitializeHeaders();
    matrix.InitializeDualHeaders();

    Console.WriteLine("Чи є нуль-рядки? (y/n):");
    bool choice = Console.ReadLine()?.Trim().ToLower() == "y" ? true : false;

    if (choice)
    {
        matrix = DualInputDeleteZeroRows(matrix);
    }

    Console.WriteLine("\nПочаткова таблиця:");
    matrix.PrintMatrixWithDualHeaders();

    Console.WriteLine("\nПошук опорного розв\'язку...");
    matrix = DualInputFindReferenceSolution(matrix);
    if (matrix == null) return;
    matrix.PrintMatrixWithDualHeaders();
    Console.WriteLine(GetResultX(matrix));
    Console.WriteLine(matrix.GetFinalUValue());

    Console.WriteLine("\nПошук оптимального розв\'язку...");
    matrix = DualInputFindOptimalSolution(matrix);
    if (matrix == null) return;
    matrix.PrintMatrixWithDualHeaders();
    Console.WriteLine(GetResultX(matrix));
    Console.WriteLine(matrix.GetFinalUValue());

    matrix.ShowMinW();
}

Matrix AddOnes(Matrix matrix)
{
    double[,] newArray = new double[matrix.Rows + 1, matrix.Columns + 1];

    for (int i = 0; i < matrix.Rows; i++)
    {
        for (int j = 0; j < matrix.Columns; j++)
        {
            newArray[i, j] = matrix[i, j];
        }
    }

    for (int i = 0; i < matrix.Rows + 1; i++)
    {
        newArray[i, matrix.Columns] = 1;
    }

    for (int j = 0; j < matrix.Columns + 1; j++)
    {
        newArray[matrix.Rows, j] = -1;
    }

    newArray[matrix.Rows, matrix.Columns] = 0;

    Matrix newMatrix = new Matrix(newArray);
    newMatrix.AdjustmentNumber = matrix.AdjustmentNumber;

    return newMatrix;
}

int[] BottomPrice(Matrix matrix)
{
    int minValue = int.MaxValue;
    int[] arrValue = new int[matrix.Rows];
    int[] indexes = new int[2];

    for (int i = 0; i < matrix.Rows; i++)
    {
        for (int j = 0; j < matrix.Columns; j++)
        {
            if (matrix[i, j] < minValue)
            {
                minValue = (int)matrix[i, j];
                indexes[0] = i;
                indexes[1] = j;
            }
        }
        arrValue[i] = minValue;
        minValue = int.MaxValue;
    }

    return indexes;
}

int[] TopPrice(Matrix matrix)
{
    int maxValue = int.MinValue;
    int[] arrValue = new int[matrix.Columns];
    int[] indexes = new int[2];

    for (int j = 0; j < matrix.Columns; j++)
    {
        for (int i = 0; i < matrix.Rows; i++)
        {
            if (matrix[i, j] > maxValue)
            {
                maxValue = (int)matrix[i, j];
                indexes[0] = i;
                indexes[1] = j;
            }
        }
        arrValue[j] = maxValue;
        maxValue = int.MinValue;
    }

    return indexes;
}

int FindMinElement(Matrix matrix)
{
    int minValue = int.MaxValue;
    for (int i = 0; i < matrix.Rows; i++)
    {
        for (int j = 0; j < matrix.Columns; j++)
        {
            if (matrix[i, j] < minValue)
            {
                minValue = (int)matrix[i, j];
            }
        }
    }
    return minValue;
}

Matrix DelNegtiveElements(Matrix matrix)
{
    double[,] newArray = new double[matrix.Rows, matrix.Columns];

    int minValue = FindMinElement(matrix);
    minValue = Math.Abs(minValue);

    for (int i = 0; i < matrix.Rows; i++)
    {
        for (int j = 0; j < matrix.Columns; j++)
        {
            newArray[i, j] = matrix[i, j] + minValue;
        }
    }

    Matrix newMatrix = new Matrix(newArray);
    newMatrix.AdjustmentNumber = minValue;

    return newMatrix;
}

int GetStrategy(double value, double[] strategies)
{
    double accumulated = 0;

    for (int i = 0; i < strategies.Length; i++)
    {
        accumulated += strategies[i];

        if (value <= accumulated)
        {
            return i;
        }
    }

    return strategies.Length - 1;
}

void ModelingResult(double[,] randomNumbers, double[] X, double[] Y, Matrix matrix)
{
    string rowTemplate = "{0,-10} | {1,-10} | {2,-10} | {3,-10} | {4,-10} | {5,-10} | {6,-10} | {7,-10}";

    Console.WriteLine(rowTemplate, "Номер", "Випадкове", "Стратегiя", "Випадкове", "Стратегiя", "Виграш A", "Накопич.", "Середнiй");
    Console.WriteLine(rowTemplate, "Партiї", "число A", "гравця A", "число B", "гравця B", "", "виграш A", "виграш A");
    Console.WriteLine(new string('-', 101));

    double cumulativeWinA = 0;
    double averageWinA = 0;
    int gamesCount = randomNumbers.GetLength(0);

    double[] newX = new double[X.Length];
    double[] newY = new double[Y.Length];

    for (int i = 0; i < gamesCount; i++)
    {
        int strategyA = GetStrategy(randomNumbers[i, 0], X);
        int strategyB = GetStrategy(randomNumbers[i, 1], Y);

        string strategyAString = matrix.RowHeaders[strategyA];
        string strategyBString = matrix.ColumnHeaders[strategyB];

        for (int j = 0; j < X.Length; j++)
        {
            if (strategyA == j)
            {
                newX[j] += 1;
            }
        }

        for (int j = 0; j < Y.Length; j++)
        {
            if (strategyB == j)
            {
                newY[j] += 1;
            }
        }

        double winA = matrix[strategyA, strategyB];
        cumulativeWinA += winA;
        averageWinA = Math.Round(cumulativeWinA / (i + 1), 5);
        Console.WriteLine(rowTemplate, i + 1, randomNumbers[i, 0], strategyAString, randomNumbers[i, 1], strategyBString, winA, cumulativeWinA, averageWinA);
    }

    for (int i = 0; i < newX.Length; i++)
    {
        newX[i] = newX[i] / gamesCount;
    }

    for (int i = 0; i < newY.Length; i++)
    {
        newY[i] = newY[i] / gamesCount;
    }

    string resultX = "X = (" + string.Join("; ", newX.Select(v => v.ToString("F5"))) + ")";
    string resultY = "Y = (" + string.Join("; ", newY.Select(v => v.ToString("F5"))) + ")";
    Console.WriteLine("\nПiдсумковi стратегії:");
    Console.WriteLine(resultX);
    Console.WriteLine(resultY);
    Console.WriteLine($"v = {averageWinA}");
}

void SolveMatrixGame()
{
    Matrix matrix = InputManager.InputMatrix();

    if (FindMinElement(matrix) < 0)
    {
        matrix = DelNegtiveElements(matrix);
    }

    int[] bottomPrice = BottomPrice(matrix);
    int[] topPrice = TopPrice(matrix);

    Console.WriteLine($"Нижня цiна гри {matrix[bottomPrice[0],bottomPrice[1]]} (рядок {bottomPrice[0]}, стовпець {bottomPrice[1]})");
    Console.WriteLine($"Верхня цiна гри {matrix[topPrice[0], topPrice[1]]} (рядок {topPrice[0]}, стовпець {topPrice[1]})");

    if (bottomPrice != topPrice)
    {
        Console.WriteLine("Гра не має чистої стратегiї.");

        matrix = AddOnes(matrix);
        matrix.InitializeHeaders();
        matrix.InitializeDualHeaders();
        Matrix initialMatrix = matrix.Clone();

        for (int i = 0; i < initialMatrix.Rows; i++)
        {
            for (int j = 0; j < initialMatrix.Columns; j++)
            {
                if (i == j)
                {
                    initialMatrix.SwapHeaders(i, j);
                }
            }
        }

        Console.WriteLine("\nПочаткова таблиця для розв\'язання гри:");
        matrix.PrintMatrixWithDualHeaders();

        Console.WriteLine("Розв\'язання симплекс методом...");
        matrix = DualInputFindOptimalSolution(matrix);
        matrix.PrintMatrixWithDualHeaders();
        Console.WriteLine(GetResultX(matrix));
        Console.WriteLine(GetResultU(matrix));
        matrix.ShowMinW();

        double v = matrix[matrix.Rows - 1, matrix.Columns - 1];
        v = 1/v;
        v = v - matrix.AdjustmentNumber;
        double[] newX = GetResultXdouble(matrix).Select(x => x * v).ToArray();
        double[] newU = GetResultUdouble(matrix).Select(u => u * v).ToArray();
        Console.WriteLine("Стратегiї:");
        Console.WriteLine("Xo = (" + string.Join("; ", newU.Select(u => u.ToString("F5"))) + ")");
        Console.WriteLine("Yo = (" + string.Join("; ", newX.Select(x => x.ToString("F5"))) + ")");
        Console.WriteLine($"Цiна гри: {v}");

        Console.Write("Чи потрiбне моделювання? (y/n):");
        bool choice = Console.ReadLine()?.Trim().ToLower() == "y" ? true : false;

        if (choice)
        {
            Console.Write("\nВведiть кiлькiсть партiй: ");
            int gamesCount = int.TryParse(Console.ReadLine(), out int count) ? count : 0;

            if (gamesCount > 0)
            {
                byte[] bytes = new byte[8];
                double[,] randomNumbers = new double[gamesCount,2];

                for (int i = 0; i < gamesCount; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        RandomNumberGenerator.Fill(bytes);
                        ulong ul = BitConverter.ToUInt64(bytes, 0);
                        double r = (double)ul / ulong.MaxValue;
                        randomNumbers[i, j] = Math.Round(Math.Abs(r % 1), 5);
                    }
                }
                Console.WriteLine("Результати моделювання:\n");

                ModelingResult(randomNumbers, newU, newX, initialMatrix);
            }
        }
    }
    else
    {
        matrix.InitializeHeaders();
        matrix.InitializeDualHeaders();
        matrix.PrintMatrixWithDualHeaders();
        Console.WriteLine($"Гра має чисту стратегiю з цiною {bottomPrice}.");
    }
}

while (true)
{
    Console.WriteLine("\nОберiть дiю:");
    Console.WriteLine("1 - Пошук оберненої матрицi");
    Console.WriteLine("2 - Пошук розв\'язку системи рiвнянь");
    Console.WriteLine("3 - Пошук рангу матрицi");
    Console.WriteLine("4 - Пошук опорного розв\'зку");
    Console.WriteLine("5 - Пошук оптимального розв\'зку");
    Console.WriteLine("6 - Видалення нульових рядкiв");
    Console.WriteLine("7 - Пошук цiлочисельного розв\'язку");
    Console.WriteLine("8 - Пошук розв\'язку для двоїстої задачi");
    Console.WriteLine("9 - Розв\'язання матричної гри");
    Console.WriteLine("0 - Вихiд");
    Console.Write("Ваш вибiр: ");
    try
    {
        choice = int.Parse(Console.ReadLine());
    }
    catch (FormatException)
    {
        Console.WriteLine("Помилка! Введено некоректне число. Спробуйте ще раз.");
        continue;
    }

    switch (choice)
    {
        case 0:
            return;
        case 1:
            ReverseMatrix();
            break;
        case 2:
            SystemSolution();
            break;
        case 3:
            RankMatrix();
            break;
        case 4:
            ShowReferenceSolution();
            break;
        case 5:
            ShowOptimalSolution();
            break;
        case 6:
            withoutZeros = DeleteZeroRows().Clone();
            Console.WriteLine("Знайти опорний розв\'язок? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLower() == "y")
            {
                InputShowReferenceSolution(withoutZeros);
            }
            else
            {
                Console.WriteLine("Повернення до головного меню.\n");
            }
            break;
        case 7:
            FindIntSolution();
            break;
        case 8:
            FindDoubleSolution();
            break;
        case 9:
            SolveMatrixGame();
            break;
        default:
            Console.WriteLine("Помилка! Введено некоректний вибiр. Спробуйте ще раз.");
            continue;
    }
}