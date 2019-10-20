using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCalculator.Calculator
{
    public interface ICalculatorLogger
    {
        void LogFormula(IEnumerable<string> arguments, string operation);
    }
}
