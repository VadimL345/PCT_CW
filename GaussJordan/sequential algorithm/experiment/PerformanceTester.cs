using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class PerformanceTester
{
    private readonly GaussJordanInverter _inverter = new GaussJordanInverter();

    //Запускає детальне дослідження продуктивності для одного розміру матриці
    public void RunPerformanceStudy()
    {
        // --- Налаштування експерименту ---
        const int matrixSize = 1500; // Розмір матриці для дослідження
        const int warmUpRuns = 5;    // Кількість запусків для "прогріву"
        const int measurementRuns = 20; // Кількість запусків для вимірювання

        Console.WriteLine("===== ЗАПУСК ДОСЛІДЖЕННЯ ПРОДУКТИВНОСТІ =====");
        Console.WriteLine($"Налаштування: Розмір матриці = {matrixSize}x{matrixSize}, Прогрів = {warmUpRuns} запусків, Дослідження = {measurementRuns} запусків.");
        
        // 1. Створення єдиної матриці для всіх тестів
        Console.WriteLine("\nГенерація тестової матриці...");
        double[,] matrix = GenerateRandomMatrix(matrixSize);
        Console.WriteLine("Матрицю згенеровано.");

        // 2. Фаза "прогріву"
        Console.WriteLine($"\nВиконується {warmUpRuns} запусків для прогріву...");
        for (int i = 0; i < warmUpRuns; i++)
        {
            _inverter.Invert(matrix);
            Console.Write($"Прогрів {i + 1}/{warmUpRuns}... ");
        }
        Console.WriteLine("\nПрогрів завершено.");

        // 3. Фаза вимірювання
        Console.WriteLine($"\nВиконується {measurementRuns} запусків для дослідження...");
        List<double> executionTimes = new List<double>();
        Stopwatch stopwatch = new Stopwatch();

        for (int i = 0; i < measurementRuns; i++)
        {
            stopwatch.Restart(); // Скидаємо і запускаємо таймер
            
            _inverter.Invert(matrix); // Виконуємо інверсію
            
            stopwatch.Stop();
            double elapsedMs = stopwatch.Elapsed.TotalMilliseconds;
            executionTimes.Add(elapsedMs);
            
            Console.WriteLine($"Запуск {i + 1}/{measurementRuns}: {elapsedMs:F3} мс");
        }
        Console.WriteLine("Вимірювання завершено.");

        // 4. Аналіз та виведення результатів
        PrintResults(executionTimes, matrixSize);
    }
    
    // Виводить підсумкову статистику за результатами вимірювань.
    private void PrintResults(List<double> times, int matrixSize)
    {
        Console.WriteLine("\n" + new string('=', 40));
        Console.WriteLine("РЕЗУЛЬТАТИ ДОСЛІДЖЕННЯ");
        Console.WriteLine(new string('=', 40));
        Console.WriteLine($"Розмір матриці:         {matrixSize}x{matrixSize}");
        Console.WriteLine($"Кількість вимірювань:   {times.Count}");
        Console.WriteLine(new string('-', 40));
        Console.WriteLine($"Мінімальний час:        {times.Min():F3} мс");
        Console.WriteLine($"Максимальний час:       {times.Max():F3} мс");
        Console.WriteLine($"Середній час виконання: {times.Average():F3} мс");
        Console.WriteLine(new string('=', 40));
    }

    // Генерує квадратну матрицю заданого розміру із випадковими значеннями.
    private double[,] GenerateRandomMatrix(int n)
    {
        double[,] matrix = new double[n, n];
        Random random = new Random();
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                matrix[i, j] = random.NextDouble() * 10;
            }
        }
        return matrix;
    }
}