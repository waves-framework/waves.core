// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Waves.Core;
using Waves.Core.Sandbox;
using Waves.Core.Sandbox.Services.Interfaces;

var host = WavesBuilder.CreateDefaultBuilder(args).Build();
var services = WavesBuilder.GetDefaultServices<Startup>();
var provider = services.BuildServiceProvider();

var service = provider.GetService<ISampleService>();
service?.SampleMethod();

var logger = provider.GetService<ILogger<Program>>();
logger?.LogWarning("All done");
