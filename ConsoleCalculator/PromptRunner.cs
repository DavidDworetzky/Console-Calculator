using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCalculator
{
    public class PromptRunner
    {
        public void Start(Action action)
        {
            while(true)
            {
                action();
            }
        }
    }
}
