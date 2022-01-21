// using System.Threading.Tasks;
// using ReactiveUI.Fody.Helpers;
// using Waves.Core.Base.Attributes;
// using Waves.Core.Base.Interfaces;
// using Waves.Core.Plugins.Services.Interfaces;
//
// namespace Waves.Core.Base
// {
//     /// <summary>
//     /// Configurable object abstraction.
//     /// </summary>
//     public abstract class WavesConfigurableObject :
//         WavesObject,
//         IWavesConfigurableObject
//     {
//
//
//         /// <summary>
//         /// Gets configuration service.
//         /// </summary>
//         protected IWavesConfigurationService ConfigurationService { get; private set; }
//
//         /// <inheritdoc />
//         public async Task LoadConfigurationAsync()
//         {
//             var configuration = await ConfigurationService.GetConfigurationAsync(this);
//
//             if (configuration == null)
//             {
//                 return;
//             }
//
//             var pluginType = GetType();
//             var properties = pluginType.GetProperties();
//             foreach (var property in properties)
//             {
//                 var attributes = property.GetCustomAttributes(true);
//                 foreach (var attribute in attributes)
//                 {
//                     if (attribute is WavesPropertyAttribute)
//                     {
//                         property.SetValue(this, Configuration.GetPropertyValue(property.Name));
//                     }
//                 }
//             }
//         }
//
//         /// <inheritdoc />
//         public virtual Task SaveConfigurationAsync()
//         {
//             var configuration = new WavesConfiguration();
//
//             var pluginType = GetType();
//             var properties = pluginType.GetProperties();
//             foreach (var property in properties)
//             {
//                 var attributes = property.GetCustomAttributes(true);
//                 foreach (var attribute in attributes)
//                 {
//                     if (!(attribute is WavesPropertyAttribute))
//                     {
//                         continue;
//                     }
//
//                     var name = property.Name;
//                     var value = property.GetValue(this);
//                     configuration.AddProperty(name, value);
//                 }
//             }
//
//             return Task.CompletedTask;
//         }
//     }
// }
