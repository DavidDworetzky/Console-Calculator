using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleCalculator.Calculator;

namespace ConsoleCalculator
{
    class Program
    { 
        static void Main(string[] args)
        {
            //We choose ? as a terminator character
            var promptRunner = new PromptRunner('?');
            var calculator = DefaultCalculator.Default;

            //start a read until we reach a terminator character. 
            promptRunner.StartReadlineToTerminator((input) =>
            {
                //newline
                Console.WriteLine();
                //calculate our sum
                int output = calculator.Add(input);
                //then output
                Console.WriteLine(output);
            });
        }

    }
}
