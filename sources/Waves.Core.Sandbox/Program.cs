using System.Threading;
using System.Threading.Tasks;
using Waves.Core.Base;
using Waves.Core.Base.Enums;

namespace Waves.Core.Sandbox
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var core = new Core();

            core.Start();

            core.WriteLogMessage(new Message("Please, wait", "Waiting for 3 seconds...", "App", MessageType.Information));

            Thread.Sleep(3000);

            core.Stop();
        }
    }
}