# Build Time Logging

One of the limitations of using the `IHost` system is that during the build phase (i.e., when you're configuring `IHostBuilder` so that you can build an instance of `IHost`) you can't easily do logging. That's because logging is generally (almost always?) something that the build phase itself configures. It's a classic chicken and egg problem.

But there are enough things that can go wrong during the build phase that it'd be nice to be able to log unusual situations.

That's where `BuildTimeLoggerFactory` comes into play. It implements a simple version of Microsoft's `ILogger` subsystem that simply records log events that you give it in a simple list. When you've built your `IHost`, and thereby set up your logging system, you call a single method on `BuildTimeLoggerFactory` to dump the stored build-phase log events to the logging system. That enables you to see what happened during the build phase.

`BuildTimeLoggerFactory` exposes two properties and three methods:

## Properties

|Property|Nature|Type|Comments|
|--------|------|----|--------|
|Default|static|`BuildTimeLoggerFactory`|enables access to an environment-wide instance of `BuildTimeLoggerFactory`|
|EnableLogging|instance|`bool`|`true` to enable build-time logging, `false` to disable it (defaults to `false`)|

You use `Default` to access the one and only instance of `BuildTimeLoggerFactory` that exists within an environment which uses it (you can't create instances of `BuildTimeLoggerFactory` yourself; that's done so that all build-time logging flows through a single factory instance).

You use `Default` in one of two ways, depending on how you create the `ILogger` instance. If it's a simple readonly field you'd do this:

```c#
private readonly ILogger? _logger = BuildTimeLoggerFactory.Default.Create<LoggedType>();
```

Alternatively, if you want to create the logger inside a constructor call, you can do this:

```c#
private readonly ILogger? _logger;

public SomeType()
{
    _logger = BuildTimeLoggerFactory.Default.Create( GetType() );
}
```

I find I use the first approach whenever I have an *implicit* constructor, and the second approach when I have an *explicit* constructor, e.g., when I'm implementing logging in a base class whose precise final type I can't know ahead of time.

You use `EnableLogging` to control, globally, whether or not to enable instances of `ILogger` to be created. When logging is disabled, `null` is returned for every call to `Create()`. When used with the widespread approach to logging events:

```c#
_logger?.LogSomething(...);
```

that disables logging.

## Methods

`BuildTimeLoggerFactory` provides two methods for creating instances of `ILogger` and one method for dumping whatever logged events it's stored to your logging system once that system is setup.

### Create from Type

|Argument|Type|
|--------|----|
|type|`Type`|

```c#
private readonly ILogger? _logger;

public SomeType()
{
    _logger = BuildTimeLoggerFactory.Default.Create( GetType() );
}
```

### Create from Generic Type

|Generic Argument|Constraints|
|--------|----|
|SomeType|must be a class|

```c#
private readonly ILogger? _logger = BuildTimeLoggerFactory.Default.Create<LoggedType>();
```
