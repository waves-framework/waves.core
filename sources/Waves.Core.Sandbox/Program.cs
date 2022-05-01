// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Logging;
using Waves.Core;
using Waves.Core.Sandbox.Services;

var core = new WavesCore();
await core.StartAsync();
await core.BuildContainerAsync();

var logger = await core.GetInstanceAsync<ILogger<Program>>().ConfigureAwait(false);
logger.LogInformation("Logger successfully resolved");

var service = await core.GetInstanceAsync<SampleConfigurableService>().ConfigureAwait(false);
if (service != null)
{
    logger.LogInformation($"Value from test service: {service.TestValue}");
}

Console.ReadLine();
