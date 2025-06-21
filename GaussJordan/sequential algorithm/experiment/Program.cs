using System;

class Program
{
    static void Main(string[] args)
    {
        // Створюємо екземпляр тестера продуктивності і запускаємо дослідження
        PerformanceTester tester = new PerformanceTester();
        tester.RunPerformanceStudy();
        
        Console.WriteLine("\nДослідження завершено. Натисніть будь-яку клавішу для виходу.");
        Console.ReadKey();
    }
}