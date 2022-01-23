using System;
using System.Linq;
using Waves.Core.Base.Attributes;

namespace Waves.Core.Extensions;

/// <summary>
///     Type extensions.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Gets <see cref="WavesObjectAttribute"/> from object.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <returns>Attribute.</returns>
    public static WavesObjectAttribute GetObjectAttribute(this object obj)
    {
        var type = obj.GetType();
        var attributes = type.GetCustomAttributes(true);
        var attribute = attributes.SingleOrDefault(x => x.GetType() == typeof(WavesObjectAttribute));
        return attribute is WavesObjectAttribute wavesObjectAttribute
            ? wavesObjectAttribute
            : null;
    }

    /// <summary>
    ///     Gets friendly name of type (including generics).
    /// </summary>
    /// <param name="type">Type.</param>
    /// <returns>Friendly name.</returns>
    public static string GetFriendlyName(
        this System.Type type)
    {
        var friendlyName = type.Name;
        if (!type.IsGenericType)
        {
            return friendlyName;
        }

        var iBacktick = friendlyName.IndexOf('`');
        if (iBacktick > 0)
        {
            friendlyName = friendlyName.Remove(iBacktick);
        }

        friendlyName += "<";
        var typeParameters = type.GetGenericArguments();
        for (var i = 0; i < typeParameters.Length; ++i)
        {
            var typeParamName = GetFriendlyName(typeParameters[i]);
            friendlyName += i == 0 ? typeParamName : "," + typeParamName;
        }

        friendlyName += ">";

        return friendlyName;
    }
}
