using NUnit.Framework;

namespace Fluid.Core.Tests.Core
{
    /// <summary>
    ///     Core test class.
    /// </summary>
    public class UnitTests
    {
        private readonly Fluid.Core.Core _core = new Fluid.Core.Core();

        /// <summary>
        ///     Runs core if it is not running.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            if (!_core.IsRunning)
                _core.Start();
        }

        /// <summary>
        ///     Tests whether configuration is initialized successfully.
        /// </summary>
        [Test]
        public void CoreStart_IsConfigurationInitialized_True()
        {
            Assert.AreEqual(true, _core.CoreInitializationInformationDictionary["Configuration Loader Service"]);
        }

        /// <summary>
        ///     Tests whether IoC container is initialized successfully.
        /// </summary>
        [Test]
        public void CoreStart_IsContainerInitialized_True()
        {
            Assert.AreEqual(true, _core.CoreInitializationInformationDictionary["Service Container"]);
        }

        /// <summary>
        ///     Tests whether logging is initialized successfully.
        /// </summary>
        [Test]
        public void CoreStart_IsLoggingInitialized_True()
        {
            Assert.AreEqual(true, _core.CoreInitializationInformationDictionary["Logging Service"]);
        }

        /// <summary>
        ///     Tests whether core is initialized successfully.
        /// </summary>
        [Test]
        public void CoreStart_IsRunning_True()
        {
            Assert.AreEqual(true, _core.IsRunning);
        }
    }
}