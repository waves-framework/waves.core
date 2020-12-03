using System;

namespace Waves.Core
{
    /// <summary>
    ///     Core.
    /// </summary>
    public class Core : CoreBase
    {
        /// <inheritdoc />
        public override Guid Id => Guid.Parse("F84165C5-5C00-4D20-9FE0-27A7DC66BB64");

        /// <inheritdoc />
        public override string Name { get; set; } = "Core";
    }
}