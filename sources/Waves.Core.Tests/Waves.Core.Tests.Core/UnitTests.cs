using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Waves.Core.Base;
using Waves.Core.Base.Interfaces;
using Waves.Core.Tests.Core.TestData;
using Waves.Core.Tests.Core.TestData.Interfaces;

namespace Waves.Core.Tests.Core
{
    /// <summary>
    ///     Core test class.
    /// </summary>
    public class UnitTests
    {
        private bool _isPropertyChanged = false;

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
        //
        //
        // ///     Tests whether configuration is initialized successfully.
        // /// </summary>
        // [Test]
        // public void CoreStart_IsConfigurationInitialized_True()
        // {
        //     Assert.AreEqual(true, _core.InitializedServices["Configuration Loader Service"]);
        // }
        //
        // /// <summary>
        // ///     Tests whether application loader service is initialized successfully.
        // /// </summary>
        // [Test]
        // public void CoreStart_IsApplicationLoaderInitialized_True()
        // {
        //     Assert.AreEqual(true, _core.InitializedServices["Application Loader Service"]);
        // }
        //
        // /// <summary>
        // ///     Tests whether input service is initialized successfully.
        // /// </summary>
        // [Test]
        // public void CoreStart_IsInputServiceInitialized_True()
        // {
        //     Assert.AreEqual(true, _core.InitializedServices["Keyboard and Mouse Input Service"]);
        // }
        //
        // /// <summary>
        // ///     Tests whether logging is initialized successfully.
        // /// </summary>
        // [Test]
        // public void CoreStart_IsLoggingInitialized_True()
        // {
        //     foreach (var service in _core.InitializedServices)
        //     {
        //         if (service.Key.Contains("Logging Service"))
        //         {
        //             Assert.AreEqual(true, service.Value);
        //
        //             return;
        //         }
        //     }
        //
        //     Assert.Fail();
        // }
        //
        // /// <summary>
        // ///     Tests whether module service is initialized successfully.
        // /// </summary>
        // [Test]
        // public void CoreStart_IsModuleLoaderServiceInitialized_True()
        // {
        //     Assert.AreEqual(true, _core.InitializedServices["Module Loader Service"]);
        // }

        /// <summary>
        ///     Tests whether core is initialized successfully.
        /// </summary>
        [Test]
        public void CoreStart_IsRunning_True()
        {
            Assert.AreEqual(true, _core.IsRunning);
        }
        
        /// <summary>
        ///     Tests whether is applications service resolving.
        /// </summary>
        [Test]
        public void CoreStart_IsApplicationsServiceResolving_True()
        {
            var service = _core.GetService<IApplicationService>();

            Assert.NotNull(service);
        }

        /// <summary>
        ///     Tests whether is logging service resolving.
        /// </summary>
        [Test]
        public void CoreStart_IsLoggingServiceResolving_True()
        {
            var service = _core.GetService<ILoggingService>();

            Assert.NotNull(service);
        }
        
        /// <summary>
        /// Tests whether is input service resolving.
        /// </summary>
        [Test]
        public void CoreStart_IsInputServiceResolving_True()
        {
            var service = _core.GetService<IInputService>();

            Assert.NotNull(service);
        }
        
        /// <summary>
        /// Tests whether is module service resolving.
        /// </summary>
        [Test]
        public void CoreStart_IsModulesServiceResolving_True()
        {
            var service = _core.GetService<IModuleService>();

            Assert.NotNull(service);
        }

        /// <summary>
        /// Test whether test service is registering.
        /// </summary>
        [Test]
        public void CoreStart_IsTestServiceRegistering_True()
        {
            var service = new TestService();

            Assert.DoesNotThrow(delegate
            {
                _core.RegisterService<ITestService>(service);
            });
        }
        
        /// <summary>
        /// Test whether test service is resolving.
        /// </summary>
        [Test]
        public void CoreStart_IsTestServiceResolving_True()
        {
            var service = _core.GetService<ITestService>();

            Assert.NotNull(service);
        }
        
        /// <summary>
        /// Test whether test service saves configuration.
        /// </summary>
        [Test]
        public void CoreStart_IsTestServiceSavesConfiguration_True()
        {
            var service = _core.GetService<ITestService>();

            if (service != null)
            {
                Assert.DoesNotThrow(delegate
                {
                    service.SaveConfiguration(_core.Configuration);
                });
            }

            Assert.Fail();
        }

        /// <summary>
        ///     Tests notify property changed.
        /// </summary>
        [Test]
        public void CoreBase_IsNotifyPropertyChangedWorking_True()
        {
            var property = new Property<int>("Test", 0, false, false);

            property.PropertyChanged += OnPropertyChanged;

            var result = CheckPropertyChangedAsync(property).Result;

            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// Sets property's value and return if property changed triggered.
        /// </summary>
        /// <param name="property">Property.</param>
        /// <returns>Result.</returns>
        private async Task<bool> CheckPropertyChangedAsync(Property<int> property)
        {
            property.SetValue(1);

            await Task.Delay(100).ConfigureAwait(false);

            return _isPropertyChanged;
        }

        /// <summary>
        /// Notifies when property changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Arguments.</param>
        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _isPropertyChanged = true;
        }
    }
}