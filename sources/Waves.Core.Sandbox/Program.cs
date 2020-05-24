using System;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Sandbox
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var core = new Core();
            core.Start();

            //var property1 = (IProperty) new Property<int>("name", 1, true, true);
            //var property2 = (IProperty) property1.Clone();
            //var equals = property1.Equals(property2);

            var configuration = (IConfiguration) core.Configuration.Clone();
            var equals = configuration.Equals(core.Configuration);

            Console.WriteLine("Write \"stop\" to stop core working:");

            var word = Console.ReadLine();

            if (word != null && word.Equals("stop")) core.Stop();
        }
    }
}