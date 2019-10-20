using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCalculator.Calculator
{
    public class CalculatorConsoleLogger : ICalculatorLogger
    {
        public void LogFormula(IEnumerable<string> arguments, string operation)
        {
            StringBuilder sb = new StringBuilder();
            bool afterInitial = false;
            foreach(var arg in arguments)
            {
                sb.Append(afterInitial ? operation + arg : arg);
                afterInitial = true;
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
