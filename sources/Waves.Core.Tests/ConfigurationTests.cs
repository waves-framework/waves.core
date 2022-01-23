using System;
using System.Collections.Generic;
using System.IO;
using Waves.Core.Base;
using Waves.Core.Extensions;
using Waves.Core.Tests.Objects;
using Xunit;

namespace Waves.Core.Tests;

/// <summary>
/// Configuration tests.
/// </summary>
public class ConfigurationTests
{
    private readonly string _defaultConfigPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? throw new InvalidOperationException(), "config");

    /// <summary>
    /// Creates new instance of <see cref="ConfigurationTests"/>.
    /// </summary>
    public ConfigurationTests()
    {
        if (!Directory.Exists(_defaultConfigPath))
        {
            return;
        }

        var files = Directory.GetFiles(_defaultConfigPath);
        foreach (var file in files)
        {
            File.Delete(file);
        }
    }

    /// <summary>
    /// Gets properties test data.
    /// </summary>
    public static IEnumerable<object[]> PropertiesTestData
    {
        get
        {
            yield return new object[] { "String", "TestString", "TestString1" };
            yield return new object[] { "Guid", Guid.NewGuid(), Guid.NewGuid() };
            yield return new object[] { "Int32", 123, 456 };
            yield return new object[] { "Double", 123.4d, 456.7d };
            yield return new object[] { "Class", new TestObject(), new TestObject() };
        }
    }

    /// <summary>
    /// Tests that "AddProperty" method does not throws exceptions.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="value">Value.</param>
    /// <param name="newValue">New value (not used in this test).</param>
    [Theory]
    [MemberData(nameof(PropertiesTestData))]
#pragma warning disable xUnit1026
    public void AddProperty_DoesNotThrowsExceptions(string name, object value, object newValue)
    {
        var configuration = new WavesConfiguration();
        configuration.AddProperty(name, value);
    }

    /// <summary>
    /// Tests that "RemoveProperty" method does not throws exceptions.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="value">Value.</param>
    /// <param name="newValue">New value (not used in this test).</param>
    [Theory]
    [MemberData(nameof(PropertiesTestData))]
#pragma warning disable xUnit1026
    public void RemoveProperty_DoesNotThrowsExceptions(string name, object value, object newValue)
    {
        var configuration = new WavesConfiguration();
        configuration.AddProperty(name, value);
        configuration.RemoveProperty(name);
    }

    /// <summary>
    /// Tests that "GetPropertyValue" method does not throws exceptions.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="value">Value.</param>
    /// <param name="newValue">New value.</param>
    [Theory]
    [MemberData(nameof(PropertiesTestData))]
#pragma warning disable xUnit1026
    public void GetPropertyValue_DoesNotThrowsExceptions(string name, object value, object newValue)
    {
        var configuration = new WavesConfiguration();
        configuration.AddProperty(name, value);
        configuration.GetPropertyValue(name);
    }

    /// <summary>
    /// Tests that "SetPropertyValue" method does not throws exceptions.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="value">Value.</param>
    /// <param name="newValue">New value.</param>
    [Theory]
    [MemberData(nameof(PropertiesTestData))]
    public void SetPropertyValue_DoesNotThrowsExceptions(string name, object value, object newValue)
    {
        var configuration = new WavesConfiguration();
        configuration.AddProperty(name, value);
        configuration.SetPropertyValue(name, newValue);
    }

    /// <summary>
    /// Tests that "GetPropertyValue" method returns expected value.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="value">Value.</param>
    /// <param name="newValue">New value.</param>
    [Theory]
    [MemberData(nameof(PropertiesTestData))]
#pragma warning disable xUnit1026
    public void GetPropertyValue_ValueEqualsExpected(string name, object value, object newValue)
    {
        var configuration = new WavesConfiguration();
        configuration.AddProperty(name, value);
        var actualValue = configuration.GetPropertyValue(name);
        Assert.Equal(value, actualValue);
    }

    /// <summary>
    /// Tests that "Contains" method does not throws exceptions.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="value">Value.</param>
    /// <param name="newValue">New value.</param>
    [Theory]
    [MemberData(nameof(PropertiesTestData))]
#pragma warning disable xUnit1026
    public void ContainsValue_ValueEqualsExpected(string name, object value, object newValue)
    {
        var configuration = new WavesConfiguration();
        configuration.AddProperty(name, value);
        var actualValue = configuration.Contains(name);
        Assert.True(actualValue);
    }

    /// <summary>
    /// Tests that "SaveConfiguration" method does not throws exceptions.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="value">Value.</param>
    /// <param name="newValue">New value.</param>
    [Theory]
    [MemberData(nameof(PropertiesTestData))]
#pragma warning disable xUnit1026
    public void SaveConfiguration_DoesNotThrowsExceptions(string name, object value, object newValue)
    {
        var id = Guid.NewGuid();
        var configuration = new WavesConfiguration();
        configuration.AddProperty(name, value);
        configuration.SaveConfiguration(_defaultConfigPath, $"{id}.json");
    }

    /// <summary>
    /// Tests that "LoadConfiguration" method does not throws exceptions.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="value">Value.</param>
    /// <param name="newValue">New value.</param>
    [Theory]
    [MemberData(nameof(PropertiesTestData))]
#pragma warning disable xUnit1026
    public void LoadConfiguration_DoesNotThrowsExceptions(string name, object value, object newValue)
    {
        var id = Guid.NewGuid();
        var configuration = new WavesConfiguration();
        configuration.AddProperty(name, value);
        configuration.SaveConfiguration(_defaultConfigPath, $"{id}.json");
        var loadedConfiguration = ConfigurationExtensions.LoadConfiguration(_defaultConfigPath, $"{id}.json");
    }

    /// <summary>
    /// Tests "LoadConfiguration": loaded configuration must be equals saved.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="value">Value.</param>
    /// <param name="newValue">New value.</param>
    [Theory]
    [MemberData(nameof(PropertiesTestData))]
#pragma warning disable xUnit1026
    public void LoadConfiguration_LoadedEqualsSaved(string name, object value, object newValue)
    {
        var id = Guid.NewGuid();
        var savedConfiguration = new WavesConfiguration();
        savedConfiguration.AddProperty(name, value);
        savedConfiguration.SaveConfiguration(_defaultConfigPath, $"{id}.json");
        var loadedConfiguration = ConfigurationExtensions.LoadConfiguration(_defaultConfigPath, $"{id}.json");
        Assert.Equal(savedConfiguration, loadedConfiguration);
    }
}