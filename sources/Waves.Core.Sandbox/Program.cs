// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Waves.Core;
using Waves.Core.Sandbox;
using Waves.Core.Sandbox.Services.Interfaces;

var host = WavesBuilder.CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .Build();

var logger = host.Services.GetService<ILogger<Program>>();
logger?.LogInformation("Program started");

var service = host.Services.GetService<ISampleService>();
service?.SampleMethod();
logger?.LogInformation("All done!");

host.Run();
