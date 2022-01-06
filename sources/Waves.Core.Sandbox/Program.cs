// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Waves.Core;
using Waves.Core.Sandbox;
using Waves.Core.Sandbox.Services.Interfaces;

Console.WriteLine("Hello, World!");

var configuration = WavesBuilder.CreateDefaultBuilder().Build();
var services = WavesBuilder.GetDefaultServices<Startup>();
var provider = services.BuildServiceProvider();

var service = provider.GetService<ISampleService>();
service?.SampleMethod();
