using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleCalculator;
using System.Collections.Generic;

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
            var output = _calculator.Sum(input);
            Assert.AreEqual(output, 20);
        }

        [TestMethod]
        public void TwoIntegerSum()
        {
            string input = "1,2";
            var output = _calculator.Sum(input);
            Assert.AreEqual(output, 3);
        }
        
        [TestMethod]
        public void TwoIntegerSumAboveLimit()
        {
            string input = "2,1001,6";
            var output = _calculator.Sum(input);
            Assert.AreEqual(output, 8);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void TwoIntegerSumWithNegative()
        {
            string input = "4,-3";
            var output = _calculator.Sum(input);
            Assert.AreEqual(output, 1);
        }
        [TestMethod]
        public void InvalidNumbersConvertedToZero()
        {
            string input = "5,tytyt";
            var output = _calculator.Sum(input);
            Assert.AreEqual(output, 5);
        }

        [TestMethod]
        public void MoreThanTwoArgumentsDoesNotThrowException()
        {
            string input = "5,6,7";
            var output = _calculator.Sum(input);
            Assert.AreEqual(output, 18);
        }
        [TestMethod]
        public void MultipleArgumentsSumTo78()
        {
            string input = "1,2,3,4,5,6,7,8,9,10,11,12";
            var output = _calculator.Sum(input);
            Assert.AreEqual(output, 78);
        }

        [TestMethod]
        public void AddsWithMultipleDelimiters()
        {
            string input = "1\n2,3";
            var output = _calculator.Sum(input);
            Assert.AreEqual(output, 6);
        }

        [TestMethod]
        public void UseCustomDelimiterOfOneCharacter()
        {
            string input = "//#\n2#5";
            var output = _calculator.Sum(input);
            Assert.AreEqual(output, 7);
        }
        [TestMethod]
        public void UseCustomDelimiterOfOneCharacterNewline()
        {
            string input = "//,\n2,ff,100";
            var output = _calculator.Sum(input);
            Assert.AreEqual(output, 102);
        }
    }
}
