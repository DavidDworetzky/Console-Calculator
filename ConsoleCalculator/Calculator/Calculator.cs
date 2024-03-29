﻿using ConsoleCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ConsoleCalculator.Utilities;

namespace ConsoleCalculator.Calculator
{
    /// <summary>
    /// Calculator object that takes a string and performs mathematical operations on it
    /// </summary>
    public class Calculator
    {
        private readonly Dictionary<string, Func<List<int>, int>> reducers = new Dictionary<string, Func<List<int>, int>>()
        {
            {"+",  System.Linq.Enumerable.Sum},
            {"-",  Utilities.FunctionalUtilities.Subtract },
            {"*", Utilities.FunctionalUtilities.Multiply },
            {"/", Utilities.FunctionalUtilities.Divide }
        };
        /// <summary>
        /// Calculator takes delimiters and options
        /// </summary>
        /// <param name="delimiter"></param>
        public Calculator(List<string> delimiters, CalculatorOptions calculatorOptions)
        {
            _delimiters = delimiters;
            _calculatorOptions = calculatorOptions;
        }

        public Calculator(List<string> delimiters, CalculatorOptions calculatorOptions, ICalculatorLogger logger) : this(delimiters, calculatorOptions)
        {
            _logger = logger;
        }
        private List<string> _delimiters;
        private CalculatorOptions _calculatorOptions;
        private ICalculatorLogger _logger;

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
                var extraDelimiters = captureParts.SelectMany(filter).Where(d => d.Length > 0).ToList();
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
            var filters = new List<Func<string, List<string>>>() { StringUtilities.FilterSlashes, StringUtilities.FilterSlashesAndSplitBrackets };
            int index = 0;
            foreach(var pattern in patterns)
            {
                //continue trying to find matches until we have exhausted patterns
                var delimiters = GetCustomDelimitersFromPattern(pattern, input, filters[index]);
                if(delimiters.Item1.Count != 0)
                {
                    //if we found custom delimeters, strip out any newline or newline/carriage returns at the beginning of the string
                    var strippedInput = delimiters.Item2.TrimStart('\r').TrimStart('\n');
                    return Tuple.Create(delimiters.Item1, strippedInput);
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
        /// Add Operation
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Add(string input)
        {
            return Operation(input, "+");
        }
        /// <summary>
        /// Subtract Operation
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Subtract(string input)
        {
            return Operation(input, "-");
        }
        /// <summary>
        /// Multiply Operation
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Multiply(string input)
        {
            return Operation(input, "*");
        }
        /// <summary>
        /// Divide Operation
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Divide(string input)
        {
            return Operation(input, "/");
        }
        /// <summary>
        /// Perform an operation on a list of input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private int Operation(string input, string op)
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
            //adjust input to be input after delimiter tokens are taken away
            input = customDelims.Item2;
            //split our input by delimiter
            var splitArgs = SplitByDelimiters(input, delimiters).ToList();

            //now parse as ints to validate and sum
            var parsedArgs = splitArgs.Select(arg =>
            {
                return ParseTokenAsInt(arg, _calculatorOptions.InvalidValueLimit);
            }).ToList();
            //Now validate them
            ValidateArguments(parsedArgs);
            //log formula for calculation if we have a logger and have logging enabled
            if(_calculatorOptions.DisplayFormula)
            {
                _logger?.LogFormula(parsedArgs.Select(arg => arg.ToString()), op);
            }
            var reducer = reducers[op];
            //finally, return our operation
            var sum = reducer(parsedArgs);
            return sum;
        }
    }
}
