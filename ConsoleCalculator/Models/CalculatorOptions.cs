using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCalculator.Models
{
    public class CalculatorOptions
    {
        /// <summary>
        /// A positive arg count limit limits the number of numbers we can pass in to sum
        /// </summary>
        public int LimitArgCount { get; set; }

        /// <summary>
        /// Throws an exception when negative arguments are passed in
        /// </summary>
        public bool ThrowOnNegativeArguments { get; set; }

        /// <summary>
        /// Values greater than this value are invalid and will be zero
        /// </summary>
        public int InvalidValueLimit { get; set; }

        /// <summary>
        /// Display formula for calculator calculations
        /// </summary>
        public bool DisplayFormula { get; set; }
    }
}
