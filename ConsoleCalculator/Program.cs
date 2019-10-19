using System;
using System.Collections.Generic;

namespace ConsoleCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var calculatorOptions = new Models.CalculatorOptions() { LimitArgCount = -1 };
            //We choose ? as a terminator character since \n is a delimiter
            var promptRunner = new PromptRunner('?');
            var delimiters = new List<char>() { ',', '\n' };
            var calculator = new Calculator(delimiters, calculatorOptions);

            //start a read until we reach a terminator character. 
            promptRunner.StartReadlineToTerminator((input) =>
            {
                int output = calculator.Sum(input);
                //newline
                Console.WriteLine();
                //then output
                Console.WriteLine(output);
            });
        }

    }
}
