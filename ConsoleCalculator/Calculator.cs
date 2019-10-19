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
        public Calculator(List<char> delimiters, CalculatorOptions calculatorOptions)
        {
            _delimiters = delimiters;
            _calculatorOptions = calculatorOptions;
        }
        private List<char> _delimiters;
        private CalculatorOptions _calculatorOptions;

        /// <summary>
        /// Method for splitting our string with multiple delimiters
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private IEnumerable<string> SplitByDelimiters(string input)
        {
            List<string> allSubstrings = new List<string>();
            allSubstrings.Add(input);

            //keep splitting until we are done with all delimiters
            foreach(var delimiter in _delimiters)
            {
                List<string> substrings = new List<string>();
                //only split the substrings if delimiter is contained
                if(input.Contains(delimiter))
                {
                    foreach(var substring in allSubstrings)
                    {
                        substrings.AddRange(substring.Split(delimiter));
                    }
                    allSubstrings = substrings;
                }
            }
            return allSubstrings;
        }
        /// <summary>
        /// Sum Operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Sum(string input)
        {
            //split our input by delimiter
            var splitArgs = SplitByDelimiters(input).ToList();
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
