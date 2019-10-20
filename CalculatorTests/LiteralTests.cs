using ConsoleCalculator.Calculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorTests
{
    /// <summary>
    /// More tests around newline specific test scenarios. Using \n character literal instead of Environment.Newline
    /// </summary>
    [TestClass]
    public class LiteralTests
    {
        private Calculator _calculator;
        private static string Newline = "" + '\n';
        public LiteralTests()
        {
            var delimiters = new List<string>() { ",", "" + '\n' };
            var calculatorLogger = new CalculatorConsoleLogger();
            var options = new ConsoleCalculator.Models.CalculatorOptions() { LimitArgCount = -1, ThrowOnNegativeArguments = true, InvalidValueLimit = 1000, DisplayFormula = true };
            _calculator = new Calculator(delimiters, options, calculatorLogger);
        }
        [TestMethod]
        public void AddsWithMultipleDelimiters()
        {
            string input = $"1{Newline}2,3";
            var output = _calculator.Add(input);
            Assert.AreEqual(output, 6);
        }

        [TestMethod]
        public void UseCustomDelimiterOfOneCharacter()
        {
            string input = $"//#{Newline}2#5";
            var output = _calculator.Add(input);
            Assert.AreEqual(output, 7);
        }
        [TestMethod]
        public void UseCustomDelimiterOfOneCharacterNewline()
        {
            string input = $"//,{Newline}2,ff,100";
            var output = _calculator.Add(input);
            Assert.AreEqual(output, 102);
        }
        [TestMethod]
        public void UseCustomDelimiterOfAnyLength()
        {
            string input = $"//[***]{Newline}11***22***33";
            var output = _calculator.Add(input);
            Assert.AreEqual(output, 66);
        }
        [TestMethod]
        public void MultipleDelimitersOfAnyLength()
        {
            string input = $"//[*][!!][r9r]{Newline}11r9r22*hh*33!!44";
            var output = _calculator.Add(input);
            Assert.AreEqual(output, 110);
        }

        //multiplication
        [TestMethod]
        public void MultipliesWithMultipleDelimiters()
        {
            string input = $"1{Newline}2,3";
            var output = _calculator.Multiply(input);
            Assert.AreEqual(output, 6);
        }

        //subtraction
        [TestMethod]
        public void SubtractsWithMultipleDelimiters()
        {
            string input = $"1{Newline}2,3";
            var output = _calculator.Subtract(input);
            Assert.AreEqual(output, -4);
        }

        //division
        [TestMethod]
        public void DividesWithMultipleDelimiters()
        {
            string input = $"4{Newline}2,1";
            var output = _calculator.Divide(input);
            Assert.AreEqual(output, 2);
        }
    }
}
