using ConsoleCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleCalculator
{
    /// <summary>
    /// Calculator object that takes a string and performs mathematical operations on it
    /// </summary>
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
        /// Method to parse our token as an integer
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private static int ParseTokenAsInt(string arg, int invalidValueLimit)
        {
            int output;
            //if args don't parse as ints, default to 0
            if (!Int32.TryParse(arg, out output))
            {
                output = 0;
            }
            //if number is greater than threshold, default to 0
            if (output > invalidValueLimit && invalidValueLimit != 0)
            {
                return 0;
            }
            return output;
        }

        /// <summary>
        /// Method for splitting our string with multiple delimiters
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static IEnumerable<string> SplitByDelimiters(string input, List<char> delimiters)
        {
            List<string> allSubstrings = new List<string>();
            allSubstrings.Add(input);

            //keep splitting until we are done with all delimiters
            foreach(var delimiter in delimiters)
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
        /// Gets the custom delimiter from the front of the input string, returns output and delimiter
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static Tuple<char, string> GetCustomDelimiterAndInput(string input)
        {
            //first pattern type located in subsection #6
            string pattern1 = @"^[\/]{2}\S";
            var match = Regex.Match(input, pattern1);
            if(match.Success)
            {
                //get captures, get delimiter character, then return tuple and resulting string
                string capturePart = match.Captures.First().Value;
                var extraDelimiter = capturePart.Replace("//", string.Empty);
                char delimiterResult = extraDelimiter[0];
                //get input string without capture group
                string inputPart = input.Replace(capturePart, string.Empty);
                return Tuple.Create(delimiterResult, inputPart);
            }
            else
            {
                //default case, no extra delimiters
                return Tuple.Create('\n', input);
            }
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
            //first, get custom delimiter(s)
            var delimiters = new List<char>();
            delimiters.AddRange(_delimiters);
            var customDelims = GetCustomDelimiterAndInput(input);
            //if we have consumed delimiter tokens in the string, add our delimiter to list of delimiters
            if(customDelims.Item2 != input)
            {
                delimiters.Add(customDelims.Item1);
            }
            //split our input by delimiter
            var splitArgs = SplitByDelimiters(input, delimiters).ToList();

            //now parse as ints to validate and sum
            var parsedArgs = splitArgs.Select(arg =>
            {
                return ParseTokenAsInt(arg, _calculatorOptions.InvalidValueLimit);
            }).ToList();
            //Now validate them
            ValidateArguments(parsedArgs);
            //finally, return our sum
            var sum = parsedArgs.Sum(a => a);
            return sum;
        }
    }
}
