using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCalculator
{
    /// <summary>
    /// Provides a default calculator initialization for both tests and the console
    /// </summary>
    public static class DefaultCalculator
    {
        public static Calculator Default {  get
            {
                var delimiters = new List<char>() { ',', '\n' };
                var options = new ConsoleCalculator.Models.CalculatorOptions() { LimitArgCount = -1, ThrowOnNegativeArguments = true, InvalidValueLimit = 1000};
                return new Calculator(delimiters, options);
            }
        }
    }
}
