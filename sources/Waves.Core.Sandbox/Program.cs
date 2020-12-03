using System.Threading;
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
            
            core.WriteLog(new WavesMessage("Please, wait", "Waiting for 3 seconds...", "App", WavesMessageType.Information));

            Thread.Sleep(3000);

            core.Stop();
        }
    }
}