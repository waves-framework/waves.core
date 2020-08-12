using System.Threading.Tasks;
using NUnit.Framework;
using Waves.Core.Base;
using Waves.Core.Base.Interfaces;

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

        /// <summary>
        ///     Tests whether configuration is initialized successfully.
        /// </summary>
        [Test]
        public void CoreStart_IsConfigurationInitialized_True()
        {
            Assert.AreEqual(true, _core.InitializedServices["Configuration Loader Service"]);
        }

        /// <summary>
        ///     Tests whether application loader service is initialized successfully.
        /// </summary>
        [Test]
        public void CoreStart_IsApplicationLoaderInitialized_True()
        {
            Assert.AreEqual(true, _core.InitializedServices["Application Loader Service"]);
        }

        /// <summary>
        ///     Tests whether input service is initialized successfully.
        /// </summary>
        [Test]
        public void CoreStart_IsInputServiceInitialized_True()
        {
            Assert.AreEqual(true, _core.InitializedServices["Keyboard and Mouse Input Service"]);
        }

        /// <summary>
        ///     Tests whether logging is initialized successfully.
        /// </summary>
        [Test]
        public void CoreStart_IsLoggingInitialized_True()
        {
            foreach (var service in _core.InitializedServices)
            {
                if (service.Key.Contains("Logging Service"))
                {
                    Assert.AreEqual(true, service.Value);

                    return;
                }
            }

            Assert.Fail();
        }

        /// <summary>
        ///     Tests whether module service is initialized successfully.
        /// </summary>
        [Test]
        public void CoreStart_IsModuleLoaderServiceInitialized_True()
        {
            Assert.AreEqual(true, _core.InitializedServices["Module Loader Service"]);
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
        ///     Tests whether core is initialized successfully.
        /// </summary>
        [Test]
        public void CoreStart_IsLoggingServiceResolving_True()
        {
            var service = _core.GetService<ILoggingService>();

            if (service != null)
            {
                Assert.True(true);

                return;
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