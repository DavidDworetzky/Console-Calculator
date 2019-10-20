using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ConsoleCalculator.Utilities
{
    /// <summary>
    /// Contains reducers for Calculator
    /// </summary>
    public static class FunctionalUtilities
    {
        public static int Subtract(List<int> numbers)
        {
            if(numbers.Count == 0)
            {
                return 0;
            }
            var initial = numbers[0];
            var restSum = numbers.GetRange(1, numbers.Count - 1).Sum();
            return initial - restSum;
        }

        public static int Multiply(List<int> numbers)
        {
            return numbers.Aggregate(1, (x, y) => x * y);
        }

        public static int Divide(List<int> numbers)
        {
            if (numbers.Count == 0)
            {
                return 0;
            }
            var initial = numbers[0];
            var restNumbers = numbers.GetRange(1, numbers.Count - 1);
            foreach(var number in restNumbers)
            {
                initial = initial / number;
            }
            return initial;
        }
    }
}
