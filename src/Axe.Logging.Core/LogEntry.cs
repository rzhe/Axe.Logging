﻿using System;

namespace Axe.Logging.Core
{
    public class LogEntry
    {
        public LogEntry(Guid aggregateId, DateTime time, string entry, object user, object data, LogLevel level)
        {
            AggregateId = aggregateId;
            Time = time;
            Entry = entry;
            User = user;
            Data = data;
            Level = level;
        }

        public Guid AggregateId { get; set; }
        public DateTime Time { get; }
        public string Entry { get; }
        public object User { get; }
        public object Data { get; }
        public LogLevel Level { get; }
    }
}