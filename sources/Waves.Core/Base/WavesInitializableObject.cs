// using System;
// using System.Threading.Tasks;
// using Waves.Core.Base.Interfaces;
//
// namespace Waves.Core.Base;
//
// /// <summary>
// /// Waves initialization object abstraction.
// /// </summary>
// public abstract class WavesInitializableObject : WavesObject, IWavesInitializableObject
// {
//     /// <inheritdoc />
//     public bool IsInitialized { get; private set; }
//
//     /// <inheritdoc />
//     public async Task InitializeAsync()
//     {
//         if (IsInitialized)
//         {
//             return;
//         }
//
//         try
//         {
//             await RunInitializationAsync();
//             IsInitialized = true;
//         }
//         catch (Exception)
//         {
//             IsInitialized = false;
//         }
//     }
//
//     /// <summary>
//     /// Does initialization work.
//     /// </summary>
//     /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
//     protected abstract Task RunInitializationAsync();
// }
