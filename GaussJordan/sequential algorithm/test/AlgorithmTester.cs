using System;
using System.Text;

public class AlgorithmTester
{
    private readonly GaussJordanInverter _inverter = new GaussJordanInverter();

    // Головний метод, що послідовно запускає всі заплановані тести.
    public void RunAllTests()
    {
        Console.WriteLine("===== ЗАПУСК ТЕСТУВАННЯ АЛГОРИТМУ =====");

        TestBasicExample();
        TestSpecialMatrices();
        TestSingularMatrix();

        Console.WriteLine("\n===== ТЕСТУВАННЯ ЗАВЕРШЕНО =====");
    }

    // --- Методи для кожного тестового сценарію ---

    public void TestBasicExample()
    {
        Console.WriteLine("\n--- Тест 1: Тестування на базовому прикладі ---");
        double[,] matrix = {
            { 1, 2, 3 },
            { 0, 1, 4 },
            { 5, 6, 0 }
        };
        RunSingleTest(matrix);
    }

    public void TestSpecialMatrices()
    {
        Console.WriteLine("\n--- Тест 2: Тестування на особливих типах матриць ---");
        
        Console.WriteLine("\n* Підтест 2.1: Одинична матриця *");
        double[,] identityMatrix = {
            { 1, 0, 0 },
            { 0, 1, 0 },
            { 0, 0, 1 }
        };
        RunSingleTest(identityMatrix);

        Console.WriteLine("\n* Підтест 2.2: Діагональна матриця *");
        double[,] diagonalMatrix = {
            { 2, 0, 0 },
            { 0, 5, 0 },
            { 0, 0, 4 }
        };
        RunSingleTest(diagonalMatrix);
    }

    public void TestSingularMatrix()
    {
        Console.WriteLine("\n--- Тест 3: Тестування надійності (обробка сингулярної матриці) ---");
        double[,] singularMatrix = {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 5, 7, 9 }
        };
        RunSingleTest(singularMatrix);
    }
    

    // Виконує повний цикл тестування для однієї матриці.
    private void RunSingleTest(double[,] matrix)
    {
        double[,] inverse = _inverter.Invert(matrix);

        if (inverse != null)
        {
            // Якщо інверсія успішна, множимо матриці для перевірки
            double[,] identityCheck = MultiplyMatrices(matrix, inverse);
            
            // Виводимо всі три матриці в одному рядку
            PrintMatricesSideBySide(
                ("Початкова A", matrix),
                ("Обернена A^-1", inverse),
                ("Перевірка A * A^-1", identityCheck)
            );
            
            // Автоматична верифікація результату
            VerifyAndPrintCheckResult(identityCheck);
        }
        else
        {
            // Обробка випадку з сингулярною матрицею
            PrintMatricesSideBySide(("Початкова матриця A", matrix));
            Console.WriteLine("\nРезультат: Матриця є сингулярною. Алгоритм коректно ідентифікував цей випадок.");
        }
    }

    // --- Допоміжні методи ---

    // Перевіряє, чи є матриця одиничною з заданим допуском, та виводить результат.
    private void VerifyAndPrintCheckResult(double[,] matrix)
    {
        const double epsilon = 1e-9; // Допуск (епсилон) для порівняння
        int n = matrix.GetLength(0);
        bool isIdentity = true;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                double target = (i == j) ? 1.0 : 0.0; // Цільове значення (1 або 0)
                if (Math.Abs(matrix[i, j] - target) > epsilon)
                {
                    isIdentity = false;
                    break;
                }
            }
            if (!isIdentity) break;
        }

        if (isIdentity)
        {
            Console.WriteLine("Перевірка коректності: УСПІХ (результат близький до одиничної матриці)");
        }
        else
        {
            Console.WriteLine("Перевірка коректності: ПОМИЛКА (результат НЕ є одиничною матрицею)");
        }
    }
    
    private double[,] MultiplyMatrices(double[,] matrixA, double[,] matrixB)
    {
        int rA = matrixA.GetLength(0);
        int cA = matrixA.GetLength(1);
        int rB = matrixB.GetLength(0);
        int cB = matrixB.GetLength(1);

        if (cA != rB)
        {
            throw new InvalidOperationException("Неможливо перемножити матриці: кількість стовпців першої матриці не дорівнює кількості рядків другої.");
        }

        double[,] result = new double[rA, cB];

        for (int i = 0; i < rA; i++)
        {
            for (int j = 0; j < cB; j++)
            {
                double sum = 0;
                for (int k = 0; k < cA; k++)
                {
                    sum += matrixA[i, k] * matrixB[k, j];
                }
                result[i, j] = sum;
            }
        }
        return result;
    }

    private void PrintMatricesSideBySide(params (string title, double[,] matrix)[] matrices)
    {
        if (matrices.Length == 0) return;

        const int cellWidth = 10; // Ширина комірки для одного числа
        int n = matrices[0].matrix.GetLength(0); // Кількість рядків
        var titles = new StringBuilder();
        var separator = "   |   ";

        // Формуємо рядок із заголовками
        foreach (var (title, matrix) in matrices)
        {
            int matrixWidth = matrix.GetLength(1) * cellWidth;
            titles.Append(title.PadRight(matrixWidth)).Append(separator);
        }
        Console.WriteLine(titles.ToString());

        // Виводимо матриці порядно
        for (int i = 0; i < n; i++)
        {
            var line = new StringBuilder();
            foreach (var (_, matrix) in matrices)
            {
                int cols = matrix.GetLength(1);
                for (int j = 0; j < cols; j++)
                {
                    line.AppendFormat($"{{0,{cellWidth}:F4}}", matrix[i, j]);
                }
                line.Append(separator);
            }
            Console.WriteLine(line.ToString());
        }
    }
}