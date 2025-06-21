using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class PerformanceComparator
{
    private readonly GaussJordanInverter _inverter = new GaussJordanInverter();

    public void RunComparisonStudy()
    {
        int[] sizes = { 500, 1000, 1500, 2000 };
        const int warmUpRuns = 5;
        const int measurementRuns = 20;

        var results = new List<(int Size, double SeqTime, double ParTime)>();

        Console.WriteLine("===== ЗАПУСК ДОСЛІДЖЕННЯ ЕФЕКТИВНОСТІ ПАРАЛЕЛЬНИХ ОБЧИСЛЕНЬ =====");
        Console.WriteLine($"Налаштування: Прогрів = {warmUpRuns} запусків, Дослідження = {measurementRuns} запусків.");

        foreach (var n in sizes)
        {
            Console.WriteLine($"\n--- Дослідження для матриці {n}x{n} ---");
            var matrix = GenerateRandomMatrix(n);

            Console.WriteLine($"-> Тестування послідовного алгоритму...");
            double seqAvgTime = MeasureAlgorithm(matrix, _inverter.Invert, warmUpRuns, measurementRuns);

            Console.WriteLine($"-> Тестування паралельного алгоритму...");
            double parAvgTime = MeasureAlgorithm(matrix, _inverter.InvertParallel, warmUpRuns, measurementRuns);

            results.Add((n, seqAvgTime, parAvgTime));
            GC.Collect();
        }

        PrintResultsTable(results);
    }

    private double MeasureAlgorithm(double[,] matrix, Func<double[,], double[,]> algorithm, int warmUp, int measure)
    {
        for (int i = 0; i < warmUp; i++)
        {
            algorithm(matrix);
        }

        var timings = new List<double>();
        var stopwatch = new Stopwatch();
        for (int i = 0; i < measure; i++)
        {
            stopwatch.Restart();
            algorithm(matrix);
            stopwatch.Stop();
            timings.Add(stopwatch.Elapsed.TotalMilliseconds);
        }
        return timings.Average();
    }
    
    private void PrintResultsTable(List<(int Size, double SeqTime, double ParTime)> results)
    {
        Console.WriteLine("\n\n" + new string('=', 70));
        Console.WriteLine("Підсумкова таблиця результатів");
        Console.WriteLine(new string('=', 70));
        Console.WriteLine($"| {"Розмір матриці",-18} | {"Час послідовний, мкс",-22} | {"Час паралельний, мкс",-22} |");
        Console.WriteLine(new string('-', 70));
        foreach (var res in results)
        {
            double seqMicro = res.SeqTime * 1000;
            double parMicro = res.ParTime * 1000;
            Console.WriteLine($"| {res.Size + "x" + res.Size,-18} | {seqMicro,-22:F0} | {parMicro,-22:F0} |");
        }
        Console.WriteLine(new string('-', 70));
    }
    
    private double[,] GenerateRandomMatrix(int n)
    {
        double[,] matrix = new double[n, n];
        var random = new Random();
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                matrix[i, j] = random.NextDouble() * 10;
        return matrix;
    }
}