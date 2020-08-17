using System;
using System.Threading;
using System.Threading.Tasks;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.Tests.TestData;
using Waves.Core.Tests.TestData.Interfaces;

namespace Waves.Core.Sandbox
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var core = new Core();

            core.Start();

            core.RegisterInstance<ITestService>(new TestService());

            var service = core.GetInstance<ITestService>();

            core.WriteLog(new Message("Please, wait", "Waiting for 3 seconds...", "App", MessageType.Information));

            Thread.Sleep(3000);

            core.Stop();
        }
    }
}