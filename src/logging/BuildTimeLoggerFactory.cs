using Microsoft.Extensions.Logging;

namespace J4JSoftware.Utilities;

public class BuildTimeLoggerFactory
{
    public static BuildTimeLoggerFactory Default { get; } = new();

    private readonly Dictionary<Type, IBuildTimeLogger> _loggers = [];

    private BuildTimeLoggerFactory()
    {
    }

    public bool EnableLogging { get; set; }

    public ILogger? Create<T>()
        where T : class =>
        Create( typeof( T ) );

    public ILogger? Create( Type type )
    {
        if( !EnableLogging )
            return null;

        if( _loggers.TryGetValue( type, out var logger ) )
            return logger;

        var loggerType = typeof( BuildTimeLogger<> ).MakeGenericType( type );
        logger = (IBuildTimeLogger) Activator.CreateInstance( loggerType, type )!;

        _loggers.Add( type, logger );

        return logger;
    }

    public void Dump( ILogger? logger, LogLevel minLevel = LogLevel.Trace )
    {
        if( logger == null )
            return;

        foreach( var entry in _loggers.SelectMany( kvp => kvp.Value.Entries )
                                      .Where( e => e.Level >= minLevel ) )
        {
            logger.Log( entry.Level, entry.Exception, "{loggedType}:: {text}", entry.LoggedType.Name, entry.Text );
        }
    }
}
