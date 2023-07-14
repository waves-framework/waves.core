// See https://aka.ms/new-console-template for more information

using Autofac;
using Microsoft.Extensions.Logging;
using Waves.Core;
using Waves.Core.Sandbox.Services;
using Waves.Core.Services.Interfaces;

var core = new WavesCore();
await core.StartAsync();
var container = await core.BuildContainerAsync();
var provider = container.Resolve<IWavesServiceProvider>();

var logger = await provider.GetInstanceAsync<ILogger<Program>>().ConfigureAwait(false);
logger.LogInformation("Logger successfully resolved");

var service = await provider.GetInstanceAsync<SampleConfigurableService>().ConfigureAwait(false);
if (service != null)
{
    logger.LogInformation("Value from test service: {Value}", service.TestValue);
}

Console.ReadLine();
