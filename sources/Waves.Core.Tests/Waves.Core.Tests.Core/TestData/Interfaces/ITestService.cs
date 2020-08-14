using Waves.Core.Base.Interfaces;

namespace Waves.Core.Tests.Core.TestData.Interfaces
{
    /// <summary>
    /// Interface for test service.
    /// </summary>
    public interface ITestService : IService
    {
        /// <summary>
        /// Test method.
        /// Must return "1".
        /// </summary>
        int Test();
    }
}