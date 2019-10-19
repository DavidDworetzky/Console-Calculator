using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsoleCalculator;

namespace CalculatorTests
{
    [TestClass]
    public class CalculatorTests
    {
        public CalculatorTests()
        {
            var options = new ConsoleCalculator.Models.CalculatorOptions() { LimitArgCount = 2 };
            _calculator = new Calculator(',', options);
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
            string input = "1,5000";
            var output = _calculator.Sum(input);
            Assert.AreEqual(output, 5001);
        }

        [TestMethod]
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
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void MoreThanTwoArgumentsThrowsException()
        {
            string input = "5,6,7";
            var output = _calculator.Sum(input);
            Assert.AreEqual(output, 18);
        }
    }
}
