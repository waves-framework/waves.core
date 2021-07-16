## ![logo](files/images/logo_c_64.png)  Waves Core

![logo](https://img.shields.io/github/license/waves-framework/waves.core) ![logo](https://img.shields.io/nuget/v/Waves.Core)

### 📚 About Waves

**Waves** is a cross-platform framework designed for flexible developing of desktop, mobile applications and web-services.

### 📒 About Waves.Core

**Waves.Core** is base kernel of Waves framework. It contains interfaces, base primitives, abstractions, services and utilities of framework. Base services include container service based on Autofac, logging service based on NLog, service for input messages and service for loading native assemblies. 

### 🚀 Getting started

Like all Waves libraries Waves.Core distributes via **NuGet**. You can find the packages [here](https://www.nuget.org/profiles/Waves).

Or use these commands in the Package Manager to install Waves.Core manually:

```
Install-Package Waves.Core
```

### ⌨️ Usage basics

After installing the package you just need to declare new instance of a core and start it.

```c#
var core = new Core();
await core.StartAsync();
await core.BuildContainerAsync();
```

To stop the core type:

```c#
 await core.StopAsync();
```

### 📋 Licence

Waves.Core is licenced under the [MIT licence](https://github.com/ambertape/waves.core/blob/master/license.md).
