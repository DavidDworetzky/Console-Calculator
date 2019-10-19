using System;

namespace ConsoleCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var calculatorOptions = new Models.CalculatorOptions() { LimitArgCount = 2 };
            var promptRunner = new PromptRunner();
            var calculator = new Calculator(',', calculatorOptions);
            promptRunner.Start(() =>
            {
                string input = Console.ReadLine();
                int output = calculator.Sum(input);
                Console.WriteLine(output);
            });
        }
    }
}
