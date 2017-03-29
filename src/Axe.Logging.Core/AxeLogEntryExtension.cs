﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Logging.Core
{
    public static class AxeLogEntryExtension
    {
        private const string LOG_ENTRY_KEY = "Axe_Logging";

        public static T Mark<T>(this T exception, LogEntry logEntry) where T : Exception
        {
            VerifyException(exception);

            exception.Data.Add(LOG_ENTRY_KEY, logEntry);
            return exception;
        }

        public static LogEntry[] GetLogEntry(this Exception exception)
        {
            VerifyException(exception);

            var logEntries = new List<LogEntry>();

            GetLogEntryFromException(exception,  logEntries);

            if (!logEntries.Any())
            {
                LogEntry defaultUknownException = CreateDefaultUknownException(exception);
                logEntries.Add(defaultUknownException);
            }

            return logEntries.ToArray();
        }

        private static void VerifyException<T>(T exception) where T : Exception
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }
        }

        private static LogEntry CreateDefaultUknownException(Exception exception)
        {
            return new LogEntry(Guid.Empty, DateTime.UtcNow, exception.Message, default(object), exception, Level.Unknown);
        }

        private static void GetLogEntryFromException(Exception exception, List<LogEntry> logEntries)
        {
            if (exception.Data[LOG_ENTRY_KEY] != null)
            {
                logEntries.Add(exception.Data[LOG_ENTRY_KEY] as LogEntry);
            }

            var aggregateException = exception as AggregateException;

            if (aggregateException == null)
            {
                if (exception.InnerException != null)
                {
                    GetLogEntryFromException(exception.InnerException, logEntries);
                }
            }
            else
            {
                List<Exception> innerExceptions = aggregateException.InnerExceptions.ToList();
                foreach (var ex in innerExceptions)
                {
                    if (ex != null)
                    {
                        GetLogEntryFromException(ex, logEntries);
                    }

                }
            }
        }
    }
}