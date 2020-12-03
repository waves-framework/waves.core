using Waves.Core.Base.Interfaces.Services;

namespace Waves.Core.Tests.TestData.Interfaces
{
    /// <summary>
    /// Interface for test service.
    /// </summary>
    public interface ITestService : IWavesService
    {
        /// <summary>
        /// Test method.
        /// Must return "1".
        /// </summary>
        int Test();
    }
}