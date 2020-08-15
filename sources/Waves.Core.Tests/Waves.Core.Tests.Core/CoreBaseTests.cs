using System.Threading.Tasks;
using NUnit.Framework;
using Waves.Core.Base;

namespace Waves.Core.Tests.Core
{
    /// <summary>
    /// Waves.Core.Base classes tests.
    /// </summary>
    public class CoreBaseTests
    {
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