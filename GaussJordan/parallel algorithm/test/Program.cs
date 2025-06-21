using System;

class Program
{
    static void Main(string[] args)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");


        ParallelCorrectnessTester tester = new ParallelCorrectnessTester();
        tester.RunCorrectnessTests();
        
        Console.WriteLine("\nТестування коректності завершено.");
        Console.ReadKey();
    }
}