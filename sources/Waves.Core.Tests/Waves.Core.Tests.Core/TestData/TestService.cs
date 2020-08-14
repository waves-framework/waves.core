using System;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.Tests.Core.TestData.Interfaces;

namespace Waves.Core.Tests.Core.TestData
{
    /// <summary>
    /// Test service.
    /// </summary>
    public class TestService : Service, ITestService
    {
        private int _returnValue = 0;
        
        /// <inheritdoc />
        public override Guid Id => Guid.Parse("6202394C-C997-488F-9EFC-FE58EC3DE75E");
        
        /// <inheritdoc />
        public override string Name { get; set; } = "Test service";

        /// <inheritdoc />
        public override void Initialize(ICore core)
        {
            _returnValue = 1;
        }

        /// <inheritdoc />
        public override void LoadConfiguration(IConfiguration configuration)
        {
            try
            {
                _returnValue = LoadConfigurationValue(configuration, "TestService-ReturnValue", 1);

                OnMessageReceived(this, 
                    new Message(
                        "Loading configuration", 
                        "Configuration loads successfully.", 
                        Name,
                    MessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Loading configuration", 
                        "Error loading configuration.", 
                        Name, 
                        e, 
                        false));
            }
        }

        /// <inheritdoc />
        public override void SaveConfiguration(IConfiguration configuration)
        {
            try
            {
                configuration.SetPropertyValue("TestService-ReturnValue", _returnValue);

                OnMessageReceived(this, 
                    new Message(
                        "Saving configuration", 
                        "Configuration saved successfully.", 
                        Name,
                    MessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Saving configuration", 
                        "Error saving configuration.", 
                        Name, 
                        e, 
                        false));
            }
        }

        /// <inheritdoc />
        public int Test()
        {
            return _returnValue;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
        }
    }
}