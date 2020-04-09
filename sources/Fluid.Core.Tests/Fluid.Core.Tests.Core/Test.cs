using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fluid.Core.Tests.Core
{
    /// <summary>
    /// Core test class.
    /// </summary>
    [TestClass]
    public class Test
    {
        private Fluid.Core.Core _core = new Fluid.Core.Core();

        /// <summary>
        /// Runs core if it is not running.
        /// </summary>
        private void RunCore()
        {
            if (!_core.IsRunning)
                _core.Start();
        }

        /// <summary>
        /// Tests is configuration initialized successfully.
        /// </summary>
        [TestMethod]
        public void CoreStart_IsConfigurationInitialized_True()
        {
            RunCore();

            Assert.AreEqual(_core.IsConfigurationInitialized, true);
        }

        /// <summary>
        /// Tests is logging initialized successfully.
        /// </summary>
        [TestMethod]
        public void CoreStart_IsLoggingInitialized_True()
        {
            RunCore();

            Assert.AreEqual(_core.IsLoggingInitialized, true);
        }
    }
}