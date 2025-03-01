using Microsoft.Extensions.Logging;

namespace J4JSoftware.Utilities;

public class BuildTimeLogger<T>( Type loggedType ) : ILogger<T>, IBuildTimeLogger
    where T : class
{
    public List<LogEntry> Entries { get; } = [];

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        if( !IsEnabled( logLevel ) )
            return;

        Entries.Add( new LogEntry( logLevel, loggedType, state?.ToString(), exception ) );
    }

    public bool IsEnabled( LogLevel logLevel ) => true;

    public IDisposable BeginScope<TState>( TState state )
        where TState : notnull =>
        default!;
}
