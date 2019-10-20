using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleCalculator;
using System.Collections.Generic;
using ConsoleCalculator.Calculator;
using System;

namespace CalculatorTests
{
    [TestClass]
    public class CalculatorTests
    {
        public CalculatorTests()
        {
            _calculator = DefaultCalculator.Default;
        }
        private Calculator _calculator;

        [TestMethod]
        public void SingleIntegerSum()
        {
            string input = "20";
            var output = _calculator.Add(input);
            Assert.AreEqual(output, 20);
        }

        [TestMethod]
        public void TwoIntegerSum()
        {
            string input = "1,2";
            var output = _calculator.Add(input);
            Assert.AreEqual(output, 3);
        }
        
        [TestMethod]
        public void TwoIntegerSumAboveLimit()
        {
            string input = "2,1001,6";
            var output = _calculator.Add(input);
            Assert.AreEqual(output, 8);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void TwoIntegerSumWithNegative()
        {
            string input = "4,-3";
            var output = _calculator.Add(input);
            Assert.AreEqual(output, 1);
        }
        [TestMethod]
        public void InvalidNumbersConvertedToZero()
        {
            string input = "5,tytyt";
            var output = _calculator.Add(input);
            Assert.AreEqual(output, 5);
        }

        [TestMethod]
        public void MoreThanTwoArgumentsDoesNotThrowException()
        {
            string input = "5,6,7";
            var output = _calculator.Add(input);
            Assert.AreEqual(output, 18);
        }
        [TestMethod]
        public void MultipleArgumentsSumTo78()
        {
            string input = "1,2,3,4,5,6,7,8,9,10,11,12";
            var output = _calculator.Add(input);
            Assert.AreEqual(output, 78);
        }

        [TestMethod]
        public void AddsWithMultipleDelimiters()
        {
            string input = $"1{Environment.NewLine}2,3";
            var output = _calculator.Add(input);
            Assert.AreEqual(output, 6);
        }

        [TestMethod]
        public void UseCustomDelimiterOfOneCharacter()
        {
            string input = $"//#{Environment.NewLine}2#5";
            var output = _calculator.Add(input);
            Assert.AreEqual(output, 7);
        }
        [TestMethod]
        public void UseCustomDelimiterOfOneCharacterNewline()
        {
            string input = $"//,{Environment.NewLine}2,ff,100";
            var output = _calculator.Add(input);
            Assert.AreEqual(output, 102);
        }
        [TestMethod]
        public void UseCustomDelimiterOfAnyLength()
        {
            string input = $"//[***]{Environment.NewLine}11***22***33";
            var output = _calculator.Add(input);
            Assert.AreEqual(output, 66);
        }
        [TestMethod]
        public void MultipleDelimitersOfAnyLength()
        {
            string input = $"//[*][!!][r9r]{Environment.NewLine}11r9r22*hh*33!!44";
            var output = _calculator.Add(input);
            Assert.AreEqual(output, 110);
        }

        //multiplication
        [TestMethod]
        public void MultipliesWithMultipleDelimiters()
        {
            string input = $"1{Environment.NewLine}2,3";
            var output = _calculator.Multiply(input);
            Assert.AreEqual(output, 6);
        }

        //subtraction
        [TestMethod]
        public void SubtractsWithMultipleDelimiters()
        {
            string input = $"1{Environment.NewLine}2,3";
            var output = _calculator.Subtract(input);
            Assert.AreEqual(output, -4);
        }

        //division
        [TestMethod]
        public void DividesWithMultipleDelimiters()
        {
            string input = $"4{Environment.NewLine}2,1";
            var output = _calculator.Divide(input);
            Assert.AreEqual(output, 2);
        }
    }
}
