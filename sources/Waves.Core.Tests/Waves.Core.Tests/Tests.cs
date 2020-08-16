using System;
using NUnit.Framework;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.Base.Interfaces.Services;
using Waves.Core.Tests.TestData;
using Waves.Core.Tests.TestData.Interfaces;

namespace Waves.Core.Tests
{
    /// <summary>
    ///     Core test class.
    /// </summary>
    public class CoreTests
    {
        private ICore _core;

        /// <summary>
        ///     Runs core if it is not running.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _core = new Waves.Core.Core();
            _core.Start();
        }

        /// <summary>
        ///     Tests whether core is initialized successfully.
        /// </summary>
        [Test]
        public void CoreStart_IsRunning_True()
        {
            Assert.AreEqual(true, _core.IsRunning);
        }

        /// <summary>
        /// Tests whether configuration is initialized.
        /// </summary>
        [Test]
        public void CoreStart_IsConfigurationInitialized_True()
        {
            Assert.True(_core.Configuration.IsInitialized);
        }

        /// <summary>
        /// Tests whether container service resolving.
        /// </summary>
        [Test]
        public void Core_IsContainerServiceResolving_NotNull()
        {
            Assert.NotNull(_core.GetInstance<IContainerService>());
        }

        /// <summary>
        /// Tests whether logging service resolving.
        /// </summary>
        [Test]
        public void Core_IsLoggingServiceResolving_NotNull()
        {
            Assert.NotNull(_core.GetInstance<ILoggingService>());
        }

        /// <summary>
        /// Tests whether test service registering.
        /// </summary>
        [Test]
        public void Core_IsTestServiceRegistering_DoesNotThrow()
        {
            Assert.DoesNotThrow(delegate
            {
                _core.RegisterInstance<ITestService>(new TestService());
            });
        }

        /// <summary>
        /// Tests whether test service resolving.
        /// </summary>
        [Test]
        public void Core_IsTestServiceResolving_NotNull()
        {
            _core.RegisterInstance<ITestService>(new TestService());

            Assert.NotNull(_core.GetInstance<ITestService>());
        }

        /// <summary>
        /// Tests whether log is writing with text.
        /// </summary>
        [Test]
        public void Core_IsLogWriting_Text_DoesNotThrow()
        {
            Assert.DoesNotThrow(delegate
            {
                _core.WriteLog("Test");
            });
        }

        /// <summary>
        /// Tests whether log is writing with text.
        /// </summary>
        [Test]
        public void Core_IsLogWriting_Message_DoesNotThrow()
        {
            Assert.DoesNotThrow(delegate
            {
                _core.WriteLog(new Message("Test", "Test", "Unit test", MessageType.Information));
            });
        }

        /// <summary>
        /// Tests whether log is writing with text.
        /// </summary>
        [Test]
        public void Core_IsLogWriting_Exception_DoesNotThrow()
        {
            Assert.DoesNotThrow(delegate
            {
                _core.WriteLog(new Exception("Test exception"), "Unit test", false);
            });
        }

        /// <summary>
        /// Tests whether core saving configuration.
        /// </summary>
        [Test]
        public void Core_IsConfigurationSaving_DoesNotThrow()
        {
            Assert.DoesNotThrow(delegate
            {
                _core.SaveConfiguration();
            });
        }

        /// <summary>
        /// Tests whether core stopping.
        /// </summary>
        [Test]
        public void Core_IsCoreStopping_DoesNotThrow()
        {
            Assert.DoesNotThrow(delegate
            {
                _core.Stop();
            });
        }
    }
}