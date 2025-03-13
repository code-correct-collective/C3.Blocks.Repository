using System.Diagnostics.CodeAnalysis;

namespace C3.Blocks.Repository.MsSql;

/// <summary>
/// Provides logging messages for the repository.
/// </summary>
[ExcludeFromCodeCoverage]
static partial class LoggingMessages
{
    [LoggerMessage(
        Level = LogLevel.Trace,
        EventId = LoggingEvents.TraceMethod,
        Message = "Method {methodName} called with args {@args}"
    )]
    public static partial void LogTraceMethod(this ILogger logger, string methodName, object[] args);

    [LoggerMessage(
        Level = LogLevel.Debug,
        EventId = LoggingEvents.DebugMethod,
        Message = "Method {methodName}: {message} with {@args}"
    )]
    public static partial void LogDebugMethod(this ILogger logger, string methodName, string message, object[] args);

    [LoggerMessage(
        Level = LogLevel.Error,
        EventId = LoggingEvents.DebugMethod,
        Message = "Method {methodName}: {message} with {@args}"
    )]
    public static partial void LogErrorMethod(this ILogger logger, Exception x, string methodName, string message, object[] args);

    [LoggerMessage(
        EventId = LoggingEvents.FindEntity,
        Message = "Searching by ID {@ids}"
    )]
    public static partial void LogFindEntity(this ILogger logger, LogLevel level, object[] ids);

    [LoggerMessage(
        EventId = LoggingEvents.AddEntityInfo,
        Level = LogLevel.Information,
        Message = "Attempting to add entity {id}"
    )]
    public static partial void LogAddEntityAttempt(this ILogger logger, object id);

    [LoggerMessage(
        EventId = LoggingEvents.AddEntityError,
        Level = LogLevel.Error,
        Message = "Entity is Null"
    )]
    public static partial void LogAddEntityNull(this ILogger logger);

    [LoggerMessage(
        EventId = LoggingEvents.AddEntityInfo,
        Level = LogLevel.Information,
        Message = "Adding a range of {count} entities"
    )]
    public static partial void LogAddRangeEntity(this ILogger logger, int count);

    [LoggerMessage(
        EventId = LoggingEvents.UpdateEntityInfo,
        Level = LogLevel.Information,
        Message = "Attempting to update entity {id}"
    )]
    public static partial void LogUpdateEntityAttempt(this ILogger logger, object id);

    [LoggerMessage(
        EventId = LoggingEvents.UpdateEntityError,
        Level = LogLevel.Error,
        Message = "Entity is Null"
    )]
    public static partial void LogUpdateEntityNull(this ILogger logger);

    [LoggerMessage(
        EventId = LoggingEvents.UpdateEntityInfo,
        Level = LogLevel.Information,
        Message = "Updating a range of {count} entities"
    )]
    public static partial void LogUpdateRangeEntity(this ILogger logger, int count);

    [LoggerMessage(
        EventId = LoggingEvents.RemoveEntityInfo,
        Level = LogLevel.Information,
        Message = "Attempting to remove entity {id}"
    )]
    public static partial void LogRemoveEntityAttempt(this ILogger logger, object id);

    [LoggerMessage(
        EventId = LoggingEvents.RemoveEntityError,
        Level = LogLevel.Error,
        Message = "Entity is Null"
    )]
    public static partial void LogRemoveEntityNull(this ILogger logger);

    [LoggerMessage(
        EventId = LoggingEvents.RemoveEntityInfo,
        Level = LogLevel.Information,
        Message = "Removing a range of {count} entities"
    )]
    public static partial void LogRemoveRangeEntity(this ILogger logger, int count);
}
