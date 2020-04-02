using System;
using Fluid.Core.Base;
using Fluid.Core.Services;

namespace Fluid.Core.Sandbox
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var core = new Core();
            core.Start();
            core.Stop();
        }
    }
}