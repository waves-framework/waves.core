using NUnit.Framework;

namespace Fluid.Core.Tests.Core
{
    /// <summary>
    /// Core test class.
    /// </summary>
    public class UnitTests
    {
        private readonly Fluid.Core.Core _core = new Fluid.Core.Core();

        /// <summary>
        /// Runs core if it is not running.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            if (!_core.IsRunning)
                _core.Start();
        }

        /// <summary>
        /// Tests is configuration initialized successfully.
        /// </summary>
        [Test]
        public void CoreStart_IsConfigurationInitialized_True()
        {
            Assert.AreEqual(true, _core.IsConfigurationInitialized);
        }

        /// <summary>
        /// Tests is logging initialized successfully.
        /// </summary>
        [Test]
        public void CoreStart_IsContainerInitialized_True()
        {
            Assert.AreEqual(true, _core.IsContainerInitialized);
        }

        /// <summary>
        /// Tests is logging initialized successfully.
        /// </summary>
        [Test]
        public void CoreStart_IsLoggingInitialized_True()
        {
            Assert.AreEqual(true, _core.IsLoggingInitialized);
        }
    }
}