using System;
using Microsoft.Extensions.Logging;

namespace J4JSoftware.Utilities;

public record LogEntry( LogLevel Level, Type LoggedType, string? Text, Exception? Exception );
