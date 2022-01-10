using System;
using System.IO;
using Waves.Core.Base;
using Waves.Core.Base.Interfaces;
using Waves.Core.Utils.Serialization;

namespace Waves.Core.Extensions;

/// <summary>
/// Configuration extensions.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Saves configuration to file.
    /// </summary>
    /// <param name="configuration">Configuration.</param>
    /// <param name="path">Path to config directory.</param>
    /// <param name="fileName">File name.</param>
    public static void SaveConfiguration(this IWavesConfiguration configuration, string path, string fileName)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (fileName == null)
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var fullPath = Path.Combine(path, fileName);
        Json.WriteToFile(fullPath, configuration);
    }

    /// <summary>
    /// Loads configuration from file.
    /// </summary>
    /// <param name="path">Path to config directory.</param>
    /// <param name="fileName">File name.</param>
    /// <returns>Returns configuration.</returns>
    public static WavesConfiguration LoadConfiguration(string path, string fileName)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (fileName == null)
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        var fullPath = Path.Combine(path, fileName);
        return Json.ReadFile<WavesConfiguration>(fullPath);
    }
}
