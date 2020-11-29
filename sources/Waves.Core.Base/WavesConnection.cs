using System;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    ///     Connection base class.
    /// </summary>
    [Serializable]
    public class WavesConnection : WavesObject, IWavesConnection
    {
        /// <summary>
        ///     Creates new instance of connection.
        /// </summary>
        /// <param name="input">Input point.</param>
        /// <param name="output">Output point.</param>
        public WavesConnection(IWavesEntryPoint input, IWavesEntryPoint output)
        {
            Initialize(input, output);
        }

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc />
        [Reactive]
        public override string Name { get; set; }

        /// <inheritdoc />
        [Reactive]
        public IWavesEntryPoint Input { get; set; }

        /// <inheritdoc />
        [Reactive]
        public IWavesEntryPoint Output { get; set; }

        /// <inheritdoc />
        public object Clone()
        {
            // TODO: [Waves.Core] Connection cloning.
            return new WavesConnection(null, null);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            Input = null;
            Output = null;
        }

        /// <summary>
        ///     Entry point initialization.
        /// </summary>
        /// <param name="input">Input point.</param>
        /// <param name="output">Output point.</param>
        public void Initialize(IWavesEntryPoint input, IWavesEntryPoint output)
        {
            if (input == null || output == null) return;

            Input = input;
            Output = output;
        }
    }
}