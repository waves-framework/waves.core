using System;

namespace Fluid.Core.Sandbox
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var core = new Core();
            core.Start();

            Console.WriteLine("Write \"stop\" to stop core working:");

            var word = Console.ReadLine();

            if (word != null && word.Equals("stop")) core.Stop();
        }
    }
}