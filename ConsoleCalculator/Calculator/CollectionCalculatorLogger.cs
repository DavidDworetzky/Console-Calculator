using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ConsoleCalculator.Calculator
{
    /// <summary>
    /// For testing calculator logging in unit tests
    /// </summary>
    public class CollectionCalculatorLogger : ICalculatorLogger
    {
        public CollectionCalculatorLogger(List<string> output)
        {
            _output = output;
        }
        private List<string> _output { get; set; }

        public void LogFormula(IEnumerable<string> arguments, string operation)
        {
            StringBuilder sb = new StringBuilder();
            bool afterInitial = false;
            foreach (var arg in arguments)
            {
                sb.Append(afterInitial ? operation + arg : arg);
                afterInitial = true;
            }
            _output.Add(sb.ToString());
        }
    }
}
