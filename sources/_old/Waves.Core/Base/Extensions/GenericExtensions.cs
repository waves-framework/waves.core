﻿using System;

namespace Waves.Core.Base.Extensions
{
    /// <summary>
    /// Extensions for generic methods.
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        /// Invokes generic method in current object.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="obj">Object.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="genericType">Generic type.</param>
        /// <param name="parameters">Parameters of method.</param>
        public static void InvokeGenericMethod<T>(
            this T obj,
            string methodName,
            Type genericType,
            object[] parameters)
        {
            var method = typeof(T).GetMethod(methodName);
            var genericMethod = method?.MakeGenericMethod(genericType);
            genericMethod?.Invoke(obj, parameters);
        }
    }
}
