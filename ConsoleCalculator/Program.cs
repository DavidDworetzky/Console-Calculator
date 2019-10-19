using System;
using System.Collections.Generic;

namespace ConsoleCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var calculatorOptions = new Models.CalculatorOptions() { LimitArgCount = -1 };
            var promptRunner = new PromptRunner();
            var delimiters = new List<char>() { ',', '\n' };
            var calculator = new Calculator(delimiters, calculatorOptions);
            promptRunner.Start(() =>
            {
                string input = Console.ReadLine();
                int output = calculator.Sum(input);
                Console.WriteLine(output);
            });
        }
    }
}
