using System.Globalization;
using System.Threading.Tasks;
using Pastel;

namespace Waves.Core.Plugins.Services.Log.Console
{
    /// <summary>
    /// Console service.
    /// </summary>
    [WavesService(
        "4601BBD2-4351-411B-B700-D96E57F58B87",
        typeof(IWavesLogService))]
    public class Service : WavesLogServiceBase
    {
        /// <inheritdoc />
        public override Task InitializeAsync()
        {
            if (IsInitialized)
            {
                return Task.CompletedTask;
            }

            IsInitialized = true;

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override Task WriteLogAsync(string text)
        {
            System.Console.WriteLine(text);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override Task WriteLogAsync(IWavesMessageObject message)
        {
            var label = message.Type.ToDescription();
            var labelColor = Colors.GetColor(message.Type);
            var sender = message.Sender?.ToString();

            if (message.Sender == null)
            {
                sender = "Logger";
            }

            System.Console.WriteLine(
                "[{0}] [{1}]\t{2}: {3}",
                message.DateTime
                    .ToString(CultureInfo.CurrentCulture)
                    .Pastel(Colors.ConsoleDateTimeColor),
                label.Pastel(labelColor),
                sender.Pastel(Colors.ConsoleSenderColor),
                message.ToString().Pastel(Colors.ConsoleMessageColor));

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return "Console Log Service";
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TODO: your code for release managed resources.
            }

            // TODO: your code for release unmanaged resources.
        }
    }
}
