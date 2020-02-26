using System;

namespace Fluid.Core.Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Utils.Serialization.Json.WriteToFile("1.asd", new object());
        }
    }
}
