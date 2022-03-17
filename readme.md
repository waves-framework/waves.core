## ![logo](files/images/logo_c_64.png)  Waves Core

![logo](https://img.shields.io/github/license/waves-framework/waves.core) ![logo](https://img.shields.io/nuget/v/Waves.Core)

### 📚 About Waves

**Waves** is a cross-platform framework designed for flexible developing of desktop, mobile applications and web-services.

### 📒 About Waves.Core

**Waves.Core** is base kernel of Waves framework. It contains interfaces, base primitives, abstractions, services and utilities of framework. 

### 🚀 Getting started

Like all Waves libraries Waves.Core distributes via **NuGet**. You can find the packages [here](https://www.nuget.org/profiles/Waves).

Or use these commands in the Package Manager to install Waves.Core manually:

```
Install-Package Waves.Core
```

### ⌨️ Usage basics

After installing the package you just need to declare new instance of host and declare own startup (inherited from `WavesStartup`) class:

```c#
var host = WavesBuilder.CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .Build();
```

Resolve services from container:

```c#
var logger = host.Services.GetService<ILogger<Program>>();
logger?.LogInformation("Hello world");
```

**⚠️ _Other documentation will be available soon._**

### 📋 Licence

Waves.Core is licenced under the [MIT licence](https://github.com/ambertape/waves.core/blob/master/license.md).
