using System;

public class ParallelCorrectnessTester
{
    private readonly GaussJordanInverter _inverter = new GaussJordanInverter();

    public void RunCorrectnessTests()
    {
        Console.WriteLine("===== ЗАПУСК ТЕСТУВАННЯ КОРЕКТНОСТІ ПАРАЛЕЛЬНОГО АЛГОРИТМУ =====");
        
        // Набір великих розмірів для тестування
        int[] sizes = { 100, 500, 1000 }; 

        foreach (int n in sizes)
        {
            Console.WriteLine($"\n--- Тест для матриці {n}x{n} ---");
            Console.WriteLine("Генерація матриці...");
            double[,] matrix = GenerateRandomMatrix(n);

            Console.WriteLine("Обчислення послідовним методом (еталон)...");
            double[,] sequentialResult = _inverter.Invert(matrix);

            Console.WriteLine("Обчислення паралельним методом...");
            double[,] parallelResult = _inverter.InvertParallel(matrix);

            Console.WriteLine("Порівняння результатів...");
            if (sequentialResult == null && parallelResult == null) {
                 Console.WriteLine("Результат: УСПІХ (Обидва алгоритми коректно визначили, що матриця сингулярна)");
            } else if (sequentialResult == null || parallelResult == null) {
                Console.WriteLine("Результат: ПОМИЛКА (Алгоритми дали різний результат щодо сингулярності)");
            } else {
                bool areEqual = AreMatricesEqual(sequentialResult, parallelResult, 1e-9);
                Console.WriteLine($"Результат: {(areEqual ? "УСПІХ" : "ПОМИЛКА")} (Матриці " + (areEqual ? "" : "НЕ ") + "збігаються)");
            }
        }
    }

    private bool AreMatricesEqual(double[,] matrixA, double[,] matrixB, double epsilon)
    {
        int rows = matrixA.GetLength(0);
        int cols = matrixA.GetLength(1);
        if (rows != matrixB.GetLength(0) || cols != matrixB.GetLength(1)) return false;

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                if (Math.Abs(matrixA[i, j] - matrixB[i, j]) > epsilon) return false;
            }
        }
        return true;
    }

    private double[,] GenerateRandomMatrix(int n)
    {
        double[,] matrix = new double[n, n];
        Random random = new Random();
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                matrix[i, j] = random.NextDouble() * 10;
            }
        }
        return matrix;
    }
}