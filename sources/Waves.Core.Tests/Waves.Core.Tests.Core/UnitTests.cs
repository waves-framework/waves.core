using NUnit.Framework;

namespace Waves.Core.Tests.Core
{
    /// <summary>
    ///     Core test class.
    /// </summary>
    public class UnitTests
    {
        private readonly Waves.Core.Core _core = new Waves.Core.Core();

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
        ///     Tests whether application loader service is initialized successfully.
        /// </summary>
        [Test]
        public void CoreStart_IsApplicationLoaderInitialized_True()
        {
            Assert.AreEqual(true, _core.CoreInitializationInformationDictionary["Application Loader Service"]);
        }

        /// <summary>
        ///     Tests whether input service is initialized successfully.
        /// </summary>
        [Test]
        public void CoreStart_IsInputServiceInitialized_True()
        {
            Assert.AreEqual(true, _core.CoreInitializationInformationDictionary["Keyboard and Mouse Input Service"]);
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
        ///     Tests whether module service is initialized successfully.
        /// </summary>
        [Test]
        public void CoreStart_IsModuleLoaderServiceInitialized_True()
        {
            Assert.AreEqual(true, _core.CoreInitializationInformationDictionary["Module Loader Service"]);
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