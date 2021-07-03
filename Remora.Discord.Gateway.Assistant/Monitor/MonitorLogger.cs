using System;
using System.IO.Compression;

using Microsoft.Extensions.Logging;

namespace Remora.Discord.Gateway.Assistant.Monitor
{
    internal static class MonitorLogger
    {
        public static void MonitorStarting(ILogger logger)
            => _monitorStarting.Invoke(logger);
        private static readonly Action<ILogger> _monitorStarting
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Information,
                    eventId:        MonitorLogEvent.MonitorStarting.ToEventId(),
                    formatString:   "Starting unknown events monitor")
                .WithoutException();

        public static void MonitorStopped(ILogger logger)
            => _monitorStopped.Invoke(logger);
        private static readonly Action<ILogger> _monitorStopped
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Information,
                    eventId:        MonitorLogEvent.MonitorStopped.ToEventId(),
                    formatString:   "Unknown events monitor stopped")
                .WithoutException();

        public static void UnknownEventLogFileArchived(
                ILogger logger,
                ZipArchiveEntry entry)
            => _unknownEventLogFileArchived.Invoke(
                logger,
                entry.FullName,
                entry.Length,
                entry.CompressedLength);
        private static readonly Action<ILogger, string, long, long> _unknownEventLogFileArchived
            = LoggerMessage.Define<string, long, long>(
                    logLevel:       LogLevel.Debug,
                    eventId:        MonitorLogEvent.UnknownEventLogFileArchived.ToEventId(),
                    formatString:   "Unknown event log file archived: {EntryFullName}, ({EntryLength} bytes, {EntryCompressedLength} compressed)")
                .WithoutException();

        public static void UnknownEventLogFileArchiving(
                ILogger logger,
                string unknownEventFilePath)
            => _unknownEventLogFileArchiving.Invoke(
                logger,
                unknownEventFilePath);
        private static readonly Action<ILogger, string> _unknownEventLogFileArchiving
            = LoggerMessage.Define<string>(
                    logLevel:       LogLevel.Debug,
                    eventId:        MonitorLogEvent.UnknownEventLogFileArchiving.ToEventId(),
                    formatString:   "Archiving unknown event log file: {UnknownEventFilePath}")
                .WithoutException();

        public static void UnknownEventLogFileParsed(
                ILogger logger,
                UnknownEventDescriptor descriptor)
            => _unknownEventLogFileParsed.Invoke(
                logger,
                descriptor.Received,
                descriptor.RemoraApiVersion);
        private static readonly Action<ILogger, DateTimeOffset, Version> _unknownEventLogFileParsed
            = LoggerMessage.Define<DateTimeOffset, Version>(
                    logLevel:       LogLevel.Debug,
                    eventId:        MonitorLogEvent.UnknownEventLogFileParsed.ToEventId(),
                    formatString:   "Unknown event log file parsed: Received at {UnknownEventReceived} on API version {RemoraApiVersion}")
                .WithoutException();

        public static void UnknownEventLogFileParseFailed(
                ILogger logger,
                string  unknownEventFileName)
            => _unknownEventLogFileParseFailed.Invoke(
                logger,
                unknownEventFileName);
        private static readonly Action<ILogger, string> _unknownEventLogFileParseFailed
            = LoggerMessage.Define<string>(
                    logLevel:       LogLevel.Warning,
                    eventId:        MonitorLogEvent.UnknownEventLogFileParseFailed.ToEventId(),
                    formatString:   "Unknown event log file parse failed: {UnknownEventFileName}")
                .WithoutException();

        public static void UnknownEventLogFileParsing(
                ILogger logger,
                string  unknownEventFileName)
            => _unknownEventLogFileParsing.Invoke(
                logger,
                unknownEventFileName);
        private static readonly Action<ILogger, string> _unknownEventLogFileParsing
            = LoggerMessage.Define<string>(
                    logLevel:       LogLevel.Debug,
                    eventId:        MonitorLogEvent.UnknownEventLogFileParsing.ToEventId(),
                    formatString:   "Parsing unknown event log file: {UnknownEventFileName}")
                .WithoutException();

        public static void UnknownEventLogFileSaved(
                ILogger logger,
                string  unknownEventFilePath)
            => _unknownEventLogFileSaved.Invoke(
                logger,
                unknownEventFilePath);
        private static readonly Action<ILogger, string> _unknownEventLogFileSaved
            = LoggerMessage.Define<string>(
                    logLevel:       LogLevel.Debug,
                    eventId:        MonitorLogEvent.UnknownEventLogFileSaved.ToEventId(),
                    formatString:   "Unknown event log file saved: {UnknownEventFilePath}")
                .WithoutException();

        public static void UnknownEventLogFileSaving(
                ILogger logger,
                string  unknownEventLogPath)
            => _unknownEventLogFileSaving.Invoke(
                logger,
                unknownEventLogPath);
        private static readonly Action<ILogger, string> _unknownEventLogFileSaving
            = LoggerMessage.Define<string>(
                    logLevel:       LogLevel.Debug,
                    eventId:        MonitorLogEvent.UnknownEventLogFileSaving.ToEventId(),
                    formatString:   "Saving unknown event log file: {UnknownEventFilePath}")
                .WithoutException();

        public static void UnknownEventReceived(ILogger logger)
            => _unknownEventReceived.Invoke(logger);
        private static readonly Action<ILogger> _unknownEventReceived
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Information,
                    eventId:        MonitorLogEvent.UnknownEventReceived.ToEventId(),
                    formatString:   "Unknown event received")
                .WithoutException();

        public static void UnknownEventRecorded(ILogger logger)
            => _unknownEventRecorded.Invoke(logger);
        private static readonly Action<ILogger> _unknownEventRecorded
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Debug,
                    eventId:        MonitorLogEvent.UnknownEventRecorded.ToEventId(),
                    formatString:   "Unknown event recorded")
                .WithoutException();

        public static void UnknownEventRecording(ILogger logger)
            => _unknownEventRecording.Invoke(logger);
        private static readonly Action<ILogger> _unknownEventRecording
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Debug,
                    eventId:        MonitorLogEvent.UnknownEventRecording.ToEventId(),
                    formatString:   "Unknown event recording")
                .WithoutException();

        public static void UnknownEventsArchived(
                ILogger     logger,
                ZipArchive  archive)
            => _unknownEventsArchived.Invoke(
                logger,
                archive.Entries.Count);
        private static readonly Action<ILogger, int> _unknownEventsArchived
            = LoggerMessage.Define<int>(
                    logLevel:       LogLevel.Information,
                    eventId:        MonitorLogEvent.UnknownEventsArchived.ToEventId(),
                    formatString:   "Unknown events archived: {EntryCount} entries")
                .WithoutException();

        public static void UnknownEventsArchiving(ILogger logger)
            => _unknownEventsArchiving.Invoke(logger);
        private static readonly Action<ILogger> _unknownEventsArchiving
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Debug,
                    eventId:        MonitorLogEvent.UnknownEventsArchiving.ToEventId(),
                    formatString:   "Archiving unknown events")
                .WithoutException();

        public static void UnknownEventsDeleted(
                ILogger logger,
                string  unknownEventsLogPath)
            => _unknownEventsDeleted.Invoke(
                logger,
                unknownEventsLogPath);
        private static readonly Action<ILogger, string> _unknownEventsDeleted
            = LoggerMessage.Define<string>(
                    logLevel:       LogLevel.Information,
                    eventId:        MonitorLogEvent.UnknownEventsDeleted.ToEventId(),
                    formatString:   "Unknown events folder deleted: {UnknownEventsLogPath}")
                .WithoutException();

        public static void UnknownEventsDeleting(
                ILogger logger,
                string  unknownEventsLogPath)
            => _unknownEventsDeleting.Invoke(
                logger,
                unknownEventsLogPath);
        private static readonly Action<ILogger, string> _unknownEventsDeleting
            = LoggerMessage.Define<string>(
                    logLevel:       LogLevel.Debug,
                    eventId:        MonitorLogEvent.UnknownEventsDeleting.ToEventId(),
                    formatString:   "Deleting unknown events folder: {UnknownEventsLogPath}")
                .WithoutException();

        public static void UnknownEventsEnumerating(
                ILogger logger,
                string  unknownEventsLogPath)
            => _unknownEventsEnumerating.Invoke(
                logger,
                unknownEventsLogPath);
        private static readonly Action<ILogger, string> _unknownEventsEnumerating
            = LoggerMessage.Define<string>(
                    logLevel:       LogLevel.Debug,
                    eventId:        MonitorLogEvent.UnknownEventsEnumerating.ToEventId(),
                    formatString:   "Enumerating unknown events folder: {UnknownEventsLogPath}")
                .WithoutException();

        public static void UnknownEventsLogFilesFound(
                ILogger logger,
                string  unknownEventsLogPath,
                int     fileCount)
            => _unknownEventsLogFilesFound.Invoke(
                logger,
                fileCount,
                unknownEventsLogPath);
        private static readonly Action<ILogger, int, string> _unknownEventsLogFilesFound
            = LoggerMessage.Define<int, string>(
                    logLevel:       LogLevel.Debug,
                    eventId:        MonitorLogEvent.UnknownEventsLogFilesFound.ToEventId(),
                    formatString:   "Unknown events log has {FileCount} files: {UnknownEventsLogPath}")
                .WithoutException();

        public static void UnknownEventsLogPathEmpty(
                ILogger logger,
                string  unknownEventsLogPath)
            => _unknownEventsLogPathEmpty.Invoke(
                logger,
                unknownEventsLogPath);
        private static readonly Action<ILogger, string> _unknownEventsLogPathEmpty
            = LoggerMessage.Define<string>(
                    logLevel:       LogLevel.Debug,
                    eventId:        MonitorLogEvent.UnknownEventsLogPathEmpty.ToEventId(),
                    formatString:   "Unknown events log folder is empty: {UnknownEventsLogPath}")
                .WithoutException();

        public static void UnknownEventsLogPathNotFound(
                ILogger logger,
                string  unknownEventsLogPath)
            => _unknownEventsLogPathNotFound.Invoke(
                logger,
                unknownEventsLogPath);
        private static readonly Action<ILogger, string> _unknownEventsLogPathNotFound
            = LoggerMessage.Define<string>(
                    logLevel:       LogLevel.Debug,
                    eventId:        MonitorLogEvent.UnknownEventsLogPathNotFound.ToEventId(),
                    formatString:   "Unknown events log folder not found: {UnknownEventsLogPath}")
                .WithoutException();
    }
}
