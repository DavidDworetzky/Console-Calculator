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
        /// Calculator takes delimiters and options
        /// </summary>
        /// <param name="delimiter"></param>
        public Calculator(List<string> delimiters, CalculatorOptions calculatorOptions)
        {
            _delimiters = delimiters;
            _calculatorOptions = calculatorOptions;
        }
        private List<string> _delimiters;
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
        private static IEnumerable<string> SplitByDelimiters(string input, List<string> delimiters)
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
        /// Filters out slashes for delimiter matching
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static List<string> FilterSlashes(string str)
        {
            return new List<string>() { str.Replace("//", string.Empty) };
        }

        /// <summary>
        /// Filters out slashes and splits brackets for delimiter matching
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static List<string> FilterSlashesAndSplitBrackets(string str)
        {
            return str.Replace("//", String.Empty).Replace("[", string.Empty).Split("]").ToList();
        }

        /// <summary>
        /// Method for returning custom delimiters from a regex pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private static Tuple<List<string>,string>  GetCustomDelimitersFromPattern(string pattern, string input, Func<string, List<string>> filter)
        {
            var matches = Regex.Matches(input, pattern);
            if (matches.Count > 0)
            {
                //get captures, get delimiter character, then return tuple and resulting string
                var captureParts = matches[0].Captures.Select(c => c.Value);
                //now filter based off of our filter function
                var extraDelimiters = captureParts.SelectMany(filter).ToList();
                var match = matches[0];

                //rest of string should be after match group
                var remaining = input.Substring(match.Index + match.Length);
                return Tuple.Create(extraDelimiters, remaining);
            }
            else
            {
                //default case, no extra delimiters
                return Tuple.Create(new List<string>(), input);
            }
        }

        /// <summary>
        /// Gets the custom delimiter from the front of the input string, returns output and delimiter
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static Tuple<List<string>, string> GetCustomDelimiterAndInput(string input)
        {
            //regex patterns
            var patterns = new List<string>() { @"^[\/]{2}(?![\[])\S{1}", @"^[\/]{2}[\[]\S*[\]]" };
            //filters on match groups for getting delimiters
            var filters = new List<Func<string, List<string>>>() { FilterSlashes, FilterSlashesAndSplitBrackets };
            int index = 0;
            foreach(var pattern in patterns)
            {
                //continue trying to find matches until we have exhausted patterns
                var delimiters = GetCustomDelimitersFromPattern(pattern, input, filters[index]);
                if(delimiters.Item1.Count != 0)
                {
                    return delimiters;
                }
                index++;
            }

            return Tuple.Create(new List<string>(), input);
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
            var delimiters = new List<string>();
            delimiters.AddRange(_delimiters);
            var customDelims = GetCustomDelimiterAndInput(input);
            //if we have consumed delimiter tokens in the string, add our delimiter to list of delimiters
            if(customDelims.Item2 != input)
            {
                delimiters.AddRange(customDelims.Item1);
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
