using System.Threading.Tasks;
using Xunit;

namespace Waves.Core.Tests
{
    /// <summary>
    /// Tests for <see cref="Core"/>.
    /// </summary>
    public class CoreTests
    {
        private readonly IWavesCore _sut;
        
        /// <summary>
        /// Creates new instance of <see cref="CoreTests"/>.
        /// </summary>
        public CoreTests()
        {
            _sut = new Core();
        }
        
        [Fact]
        public async Task Core_Start_NotThrownExceptions()
        {
            await _sut.StartAsync();
        }
        
        [Fact]
        public async Task Core_BuildContainer_NotThrownExceptions()
        {
            await _sut.StartAsync();
            await _sut.BuildContainerAsync();
        }
    }
}
