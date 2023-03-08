using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class LoggerHelper
    {
        public static ILogger<LoggerHelper> Logger;

        public static void Init(ILogger<LoggerHelper> logger)
        {
            Logger = logger;
        }

        #region 复制logger扩展方法


        //
        // 摘要:
        //     Formats and writes a debug log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   eventId:
        //     The event id associated with the log.
        //
        //   exception:
        //     The exception to log.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogDebug(EventId eventId, Exception? exception, string? message, params object?[] args)
        {
            Logger.LogDebug(eventId, exception, message, args);
        }

        //
        // 摘要:
        //     Formats and writes a debug log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   eventId:
        //     The event id associated with the log.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogDebug(EventId eventId, string? message, params object?[] args)
        {
            Logger.LogDebug(eventId, message, args);
        }

        //
        // 摘要:
        //     Formats and writes a debug log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   exception:
        //     The exception to log.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogDebug(Exception? exception, string? message, params object?[] args)
        {
            Logger.LogDebug(exception, message, args);
        }

        //
        // 摘要:
        //     Formats and writes a debug log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogDebug(string? message, params object?[] args)
        {
            Logger.LogDebug(message, args);
        }

        //
        // 摘要:
        //     Formats and writes a trace log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   eventId:
        //     The event id associated with the log.
        //
        //   exception:
        //     The exception to log.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogTrace(EventId eventId, Exception? exception, string? message, params object?[] args)
        {
            Logger.LogTrace(eventId, exception, message, args);
        }

        //
        // 摘要:
        //     Formats and writes a trace log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   eventId:
        //     The event id associated with the log.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogTrace(EventId eventId, string? message, params object?[] args)
        {
            Logger.LogTrace(eventId, message, args);
        }

        //
        // 摘要:
        //     Formats and writes a trace log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   exception:
        //     The exception to log.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogTrace(Exception? exception, string? message, params object?[] args)
        {
            Logger.LogTrace(exception, message, args);
        }

        //
        // 摘要:
        //     Formats and writes a trace log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogTrace(string? message, params object?[] args)
        {
            Logger.LogTrace(message, args);
        }

        //
        // 摘要:
        //     Formats and writes an informational log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   eventId:
        //     The event id associated with the log.
        //
        //   exception:
        //     The exception to log.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogInformation(EventId eventId, Exception? exception, string? message, params object?[] args)
        {
            Logger.LogInformation(eventId, exception, message, args);
        }

        //
        // 摘要:
        //     Formats and writes an informational log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   eventId:
        //     The event id associated with the log.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogInformation(EventId eventId, string? message, params object?[] args)
        {
            Logger.LogInformation(eventId, message, args);
        }

        //
        // 摘要:
        //     Formats and writes an informational log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   exception:
        //     The exception to log.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogInformation(Exception? exception, string? message, params object?[] args)
        {
            Logger.LogInformation(exception, message, args);
        }

        //
        // 摘要:
        //     Formats and writes an informational log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogInformation(string? message, params object?[] args)
        {
            Logger.LogInformation(message, args);
        }

        //
        // 摘要:
        //     Formats and writes a warning log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   eventId:
        //     The event id associated with the log.
        //
        //   exception:
        //     The exception to log.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogWarning(EventId eventId, Exception? exception, string? message, params object?[] args)
        {
            Logger.LogWarning(eventId, exception, message, args);
        }

        //
        // 摘要:
        //     Formats and writes a warning log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   eventId:
        //     The event id associated with the log.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogWarning(EventId eventId, string? message, params object?[] args)
        {
            Logger.LogWarning(eventId, message, args);
        }

        //
        // 摘要:
        //     Formats and writes a warning log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   exception:
        //     The exception to log.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogWarning(Exception? exception, string? message, params object?[] args)
        {
            Logger.LogWarning(exception, message, args);
        }

        //
        // 摘要:
        //     Formats and writes a warning log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogWarning(string? message, params object?[] args)
        {
            Logger.LogWarning(message, args);
        }

        //
        // 摘要:
        //     Formats and writes an error log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   eventId:
        //     The event id associated with the log.
        //
        //   exception:
        //     The exception to log.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogError(EventId eventId, Exception? exception, string? message, params object?[] args)
        {
            Logger.LogError(eventId, exception, message, args);
        }

        //
        // 摘要:
        //     Formats and writes an error log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   eventId:
        //     The event id associated with the log.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogError(EventId eventId, string? message, params object?[] args)
        {
            Logger.LogError(eventId, message, args);
        }

        //
        // 摘要:
        //     Formats and writes an error log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   exception:
        //     The exception to log.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogError(Exception? exception, string? message, params object?[] args)
        {
            Logger.LogError(exception, message, args);
        }

        //
        // 摘要:
        //     Formats and writes an error log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogError(string? message, params object?[] args)
        {
            Logger.LogError(message, args);
        }

        //
        // 摘要:
        //     Formats and writes a critical log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   eventId:
        //     The event id associated with the log.
        //
        //   exception:
        //     The exception to log.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogCritical(EventId eventId, Exception? exception, string? message, params object?[] args)
        {
            Logger.LogCritical(eventId, exception, message, args);
        }

        //
        // 摘要:
        //     Formats and writes a critical log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   eventId:
        //     The event id associated with the log.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogCritical(EventId eventId, string? message, params object?[] args)
        {
            Logger.LogCritical(eventId, message, args);
        }

        //
        // 摘要:
        //     Formats and writes a critical log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   exception:
        //     The exception to log.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogCritical(Exception? exception, string? message, params object?[] args)
        {
            Logger.LogCritical(exception, message, args);
        }

        //
        // 摘要:
        //     Formats and writes a critical log message.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   message:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void LogCritical(string? message, params object?[] args)
        {
            Logger.LogCritical(message, args);
        }

        //
        // 摘要:
        //     Formats and writes a log message at the specified log level.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   logLevel:
        //     Entry will be written on this level.
        //
        //   message:
        //     Format string of the log message.
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void Log(LogLevel logLevel, string? message, params object?[] args)
        {
            Logger.Log(logLevel, message, args);
        }

        //
        // 摘要:
        //     Formats and writes a log message at the specified log level.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   logLevel:
        //     Entry will be written on this level.
        //
        //   eventId:
        //     The event id associated with the log.
        //
        //   message:
        //     Format string of the log message.
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void Log(LogLevel logLevel, EventId eventId, string? message, params object?[] args)
        {
            Logger.Log(logLevel, eventId, message, args);
        }

        //
        // 摘要:
        //     Formats and writes a log message at the specified log level.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   logLevel:
        //     Entry will be written on this level.
        //
        //   exception:
        //     The exception to log.
        //
        //   message:
        //     Format string of the log message.
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void Log(LogLevel logLevel, Exception? exception, string? message, params object?[] args)
        {
            Logger.Log(logLevel, exception, message, args);
        }

        //
        // 摘要:
        //     Formats and writes a log message at the specified log level.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to write to.
        //
        //   logLevel:
        //     Entry will be written on this level.
        //
        //   eventId:
        //     The event id associated with the log.
        //
        //   exception:
        //     The exception to log.
        //
        //   message:
        //     Format string of the log message.
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        public static void Log(LogLevel logLevel, EventId eventId, Exception? exception, string? message, params object?[] args)
        {
            Logger.Log(logLevel, eventId, exception, message, args);
        }

        //
        // 摘要:
        //     Formats the message and creates a scope.
        //
        // 参数:
        //   logger:
        //     The Microsoft.Extensions.Logging.ILogger to create the scope in.
        //
        //   messageFormat:
        //     Format string of the log message in message template format. Example: "User {User}
        //     logged in from {Address}"
        //
        //   args:
        //     An object array that contains zero or more objects to format.
        //
        // 返回结果:
        //     A disposable scope object. Can be null.
        public static IDisposable? BeginScope(string messageFormat, params object?[] args)
        {
            return Logger.BeginScope(messageFormat, args);
        }


        #endregion


    }
}
