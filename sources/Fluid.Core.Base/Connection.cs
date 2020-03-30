using System;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    /// <summary>
    /// Connection base class.
    /// </summary>
    [Serializable]
    public class Connection : Object, IConnection
    {
        /// <summary>
        ///     Creates new instance of connection.
        /// </summary>
        /// <param name="input">Input point.</param>
        /// <param name="output">Output point.</param>
        public Connection(IEntryPoint input, IEntryPoint output)
        {
            Initialize(input, output);
        }

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc />
        public override string Name { get; set; }

        /// <inheritdoc />
        public IEntryPoint Input { get; set; }

        /// <inheritdoc />
        public IEntryPoint Output { get; set; }

        /// <summary>
        /// Entry point initialization.
        /// </summary>
        /// <param name="input">Input point.</param>
        /// <param name="output">Output point.</param>
        public void Initialize(IEntryPoint input, IEntryPoint output)
        {
            if (input == null || output == null) return;

            Input = input;
            Output = output;
        }

        /// <inheritdoc />
        public object Clone()
        {
            // TODO: [Fluid.Core] Connection cloning.
            return new Connection(null, null);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Input = null;
            Output = null;
        }
    }
}