using System;
using System.Net.Mime;
using Waves.Core.Sandbox.Samples;
using Waves.Core.Sandbox.Samples.Interfaces;

namespace Waves.Core.Sandbox
{
    /// <summary>
    ///     Program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Get current sample class.
        /// </summary>
        public static ISample Sample { get; } = new CoreLaunchSample();

        /// <summary>
        ///     Main.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static void Main(string[] args)
        {
            Sample.Execute();
            Console.ReadLine();
        }
    }
}
