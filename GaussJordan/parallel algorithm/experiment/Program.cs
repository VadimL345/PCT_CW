using System;

class Program
{
    static void Main(string[] args)
    {  
        PerformanceComparator comparator = new PerformanceComparator();
        comparator.RunComparisonStudy();
        
        Console.WriteLine("\nДослідження завершено.");
        Console.ReadKey();
    }
}