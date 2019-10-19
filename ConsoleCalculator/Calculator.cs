using ConsoleCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleCalculator
{
    public class Calculator
    {
        /// <summary>
        /// Calculator takes a delimiter and options
        /// </summary>
        /// <param name="delimiter"></param>
        public Calculator(char delimiter, CalculatorOptions calculatorOptions)
        {
            _delimiter = delimiter;
            _calculatorOptions = calculatorOptions;
        }
        private char _delimiter;
        private CalculatorOptions _calculatorOptions;
        /// <summary>
        /// Sum Operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Sum(string input)
        {
            //split our input by delimiter
            var splitArgs = input.Split(_delimiter).ToList();
            //limit to x args
            var limitCount = _calculatorOptions.LimitArgCount;
            if(_calculatorOptions.LimitArgCount > 0 && splitArgs.Count > limitCount)
            {
                throw new ArgumentOutOfRangeException("Input", $"{splitArgs.Count} arguments provided when max is {limitCount}");
            }
            //now parse as ints to sum
            var parsedArgs = splitArgs.Select(arg =>
            {
                int output;
                //if args don't parse as ints, default to 0
                if (!Int32.TryParse(arg, out output))
                {
                    output = 0;
                }
                return output;
            });
            //finally, return our sum
            var sum = parsedArgs.Sum(a => a);
            return sum;
        }
    }
}
