using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace J4JSoftware.Utilities;

internal interface IBuildTimeLogger : ILogger
{
    List<LogEntry> Entries { get; }
}
