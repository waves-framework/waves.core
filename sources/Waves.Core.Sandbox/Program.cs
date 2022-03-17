using Microsoft.Extensions.Logging;
using Waves.Core;
using Waves.Core.Sandbox.Services;

var core = new WavesCore();
await core.StartAsync();
await core.BuildContainerAsync();

var logger = await core.GetInstanceAsync<ILogger<Program>>();
logger.LogInformation($"Program started");

var service = await core.GetInstanceAsync<SampleConfigurableService>();
var value = service.TestValue;

logger.LogInformation($"Test value: {value}");
logger.LogInformation($"Press ENTER to exit...");

Console.ReadLine();
