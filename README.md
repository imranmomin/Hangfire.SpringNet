# Hangfire.SpringNet
Hangfire job activator based on Spring.Net IoC container

================

[Spring.Net](http://springframework.net/) integration for [Hangfire](http://hangfire.io). Provides an implementation of the `JobActivator` class, allowing you to use Spring.Net container to **resolve job type instances**.

*Hangfire.SpringNet* resolves service instances.

Installation
--------------

*Hangfire.SpringNet* is available as a NuGet Package. Type the following command into NuGet Package Manager Console window to install it:

```
Install-Package Hangfire.SpringNet
```

Usage
------

The package provides an extension methods for the `IGlobalConfiguration` interface, so you can enable Spring.Net integration using the `GlobalConfiguration` class:

```csharp
Spring.Context.IApplicationContext context = Spring.Context.Support.ContextRegistry.GetContext();
GlobalConfiguration.Configuration.UseSpringActivator(builder.Build());
```

After invoking the `UseSpringActivator` method, Spring.Net-based implementation of the `JobActivator` class will be used to resolve job type instances during the background job processing.

Also be aware that many web related properties that you may be using such as `HttpContext.Current` **will not be available**.
