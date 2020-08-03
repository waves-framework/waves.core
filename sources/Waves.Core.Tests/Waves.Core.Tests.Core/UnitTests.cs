using System.Threading.Tasks;
using NUnit.Framework;
using Waves.Core.Base;

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

        private bool _isPropertyChanged = false;

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