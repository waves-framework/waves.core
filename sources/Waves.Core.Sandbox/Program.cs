// See https://aka.ms/new-console-template for more information

using Waves.Core;
using Waves.Core.Base;
using Waves.Core.Sandbox.Services;

var core = new WavesCore();
await core.StartAsync();
await core.BuildContainer();

var service = await core.GetInstanceAsync<SampleConfigurableService>();
var value = service.TestValue;

Console.ReadLine();
