using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCalculator.Calculator
{
    /// <summary>
    /// Provides a default calculator initialization for both tests and the console
    /// </summary>
    public static class DefaultCalculator
    {
        public static Calculator Default {  get
            {
                var delimiters = new List<string>() { ",", "\n" };
                var calculatorLogger = new CalculatorConsoleLogger();
                var options = new ConsoleCalculator.Models.CalculatorOptions() { LimitArgCount = -1, ThrowOnNegativeArguments = true, InvalidValueLimit = 1000, DisplayFormula = true};
                return new Calculator(delimiters, options, calculatorLogger);
            }
        }
    }
}
