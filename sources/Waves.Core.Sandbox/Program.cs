// See https://aka.ms/new-console-template for more information

using Waves.Core.Base;

Console.WriteLine("Hello world!");

var configuration = new WavesConfiguration();
configuration.AddProperty(new WavesProperty<string>("Name1", "Value1"));
configuration.AddProperty(new WavesProperty<string>("Name2", "Value3"));
configuration.AddProperty(new WavesProperty<string>("Name4", "Value5"));
configuration.AddProperty(new WavesProperty<string>("Name6", "Value7"));
configuration.RemoveProperty("Name4");
configuration.Clone();
