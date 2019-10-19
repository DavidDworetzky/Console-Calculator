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
        /// Validation method which throws if our arguments do not meet certain criteria dependent on CalculatorOptions
        /// </summary>
        /// <param name="arguments"></param>
        private void ValidateArguments(IList<int> arguments)
        {
            //limit to x args
            var limitCount = _calculatorOptions.LimitArgCount;
            if (_calculatorOptions.LimitArgCount > 0 && arguments.Count > limitCount)
            {
                throw new ArgumentOutOfRangeException("Input", $"{arguments.Count} arguments provided when max is {limitCount}");
            }
            //validate and throw if negative arguments are provided
            var negativeArguments = arguments.Where(arg => arg < 0).ToList();
            if(_calculatorOptions.ThrowOnNegativeArguments)
            {
                var allNegativeArgs = negativeArguments.Select(a => a.ToString()).Aggregate(string.Empty, (x, y) => x + "," + y);
                if(negativeArguments.Count > 0)
                {
                    throw new ArgumentOutOfRangeException("Input", $"arguments: {allNegativeArgs} were negative when negative arguments are not allowed");
                }
            }
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

            //now parse as ints to validate and sum
            var parsedArgs = splitArgs.Select(arg =>
            {
                int output;
                //if args don't parse as ints, default to 0
                if (!Int32.TryParse(arg, out output))
                {
                    output = 0;
                }
                //if number is greater than threshold, default to 0
                if(output > _calculatorOptions.InvalidValueLimit && _calculatorOptions.InvalidValueLimit != 0)
                {
                    return 0;
                }
                return output;
            }).ToList();
            //Now validate them
            ValidateArguments(parsedArgs);
            //finally, return our sum
            var sum = parsedArgs.Sum(a => a);
            return sum;
        }
    }
}
