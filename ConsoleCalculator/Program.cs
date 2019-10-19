using System;
using System.Collections.Generic;

namespace ConsoleCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            //We choose ? as a terminator character since \n is a delimiter
            var promptRunner = new PromptRunner('?');
            var calculator = DefaultCalculator.Default;

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
