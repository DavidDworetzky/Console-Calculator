using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCalculator
{
    /// <summary>
    /// Utility method for running prompts from the console
    /// </summary>
    public class PromptRunner
    {

        public PromptRunner(char terminator)
        {
            _terminator = terminator;
        }

        private char _terminator;

        /// <summary>
        /// Added so we could move to the next calculation without conflicting with /n delimiter
        /// </summary>
        private string ReadToTerminator()
        {
            string totalInput = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();
                if (key.KeyChar != _terminator)
                {
                    totalInput += key.KeyChar;
                }
            } while (key.KeyChar != _terminator);
            return totalInput;
        }

        public void Start(Action action)
        {
            while (true)
            {
                action();
            }
        }

        public void StartReadlineToTerminator(Action<string> action)
        {
            while (true)
            {
                action(ReadToTerminator());
            }
        }
    }
}
