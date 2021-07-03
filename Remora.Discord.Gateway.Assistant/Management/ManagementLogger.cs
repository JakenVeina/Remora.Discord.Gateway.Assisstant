using System;
using System.IO;

using Microsoft.Extensions.Logging;

using Remora.Discord.API.Abstractions.Objects;
using Remora.Results;

namespace Remora.Discord.Gateway.Assistant.Management
{
    internal static class ManagementLogger
    {
        public static void CommandFollowedUp(
                ILogger             logger,
                Result<IMessage>    result)
            => _commandFollowedUp.Invoke(
                logger,
                result);
        private static readonly Action<ILogger, Result<IMessage>> _commandFollowedUp
            = LoggerMessage.Define<Result<IMessage>>(
                    logLevel:       LogLevel.Debug,
                    eventId:        ManagementLogEvent.CommandFollowedUp.ToEventId(),
                    formatString:   "Command followed up: {Result}")
                .WithoutException();

        public static void CommandFollowingUp(ILogger logger)
            => _commandFollowingUp.Invoke(logger);
        private static readonly Action<ILogger> _commandFollowingUp
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Debug,
                    eventId:        ManagementLogEvent.CommandFollowingUp.ToEventId(),
                    formatString:   "Following up command")
                .WithoutException();

        public static void CommandsDeploying(ILogger logger)
            => _commandsDeploying.Invoke(logger);
        private static readonly Action<ILogger> _commandsDeploying
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Information,
                    eventId:        ManagementLogEvent.CommandsDeploying.ToEventId(),
                    formatString:   "Deploying commands")
                .WithoutException();

        public static void CommandsDeployed(ILogger logger)
            => _commandsDeployed.Invoke(logger);
        private static readonly Action<ILogger> _commandsDeployed
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Information,
                    eventId:        ManagementLogEvent.CommandsDeployed.ToEventId(),
                    formatString:   "Commands deployed")
                .WithoutException();

        public static void CommandsInitializing(ILogger logger)
            => _commandsInitializing.Invoke(logger);
        private static readonly Action<ILogger> _commandsInitializing
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Information,
                    eventId:        ManagementLogEvent.CommandsInitializing.ToEventId(),
                    formatString:   "Initializing commands")
                .WithoutException();

        public static void UnknownEventsCleared(ILogger logger)
            => _unknownEventsCleared.Invoke(logger);
        private static readonly Action<ILogger> _unknownEventsCleared
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Information,
                    eventId:        ManagementLogEvent.UnknownEventsCleared.ToEventId(),
                    formatString:   "Unknown events cleared")
                .WithoutException();

        public static void UnknownEventsClearing(ILogger logger)
            => _unknownEventsClearing.Invoke(logger);
        private static readonly Action<ILogger> _unknownEventsClearing
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Debug,
                    eventId:        ManagementLogEvent.UnknownEventsClearing.ToEventId(),
                    formatString:   "Clearing unknown events")
                .WithoutException();

        public static void UnknownEventsDownloaded(
                ILogger         logger,
                MemoryStream    stream)
            => _unknownEventsDownloaded.Invoke(
                logger,
                stream.Length);
        private static readonly Action<ILogger, long> _unknownEventsDownloaded
            = LoggerMessage.Define<long>(
                    logLevel:       LogLevel.Information,
                    eventId:        ManagementLogEvent.UnknownEventsDownloaded.ToEventId(),
                    formatString:   "Unknown events downloaded: DownloadSize: {DownloadSize}")
                .WithoutException();

        public static void UnknownEventsDownloading(ILogger logger)
            => _unknownEventsDownloading.Invoke(logger);
        private static readonly Action<ILogger> _unknownEventsDownloading
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Debug,
                    eventId:        ManagementLogEvent.UnknownEventsDownloading.ToEventId(),
                    formatString:   "Downloading unknown events")
                .WithoutException();

        public static void UnknownEventsInfoRetrieved(
                ILogger logger,
                IEmbed embed)
            => _unknownEventsInfoRetrieved.Invoke(
                logger,
                embed.Title.HasValue ? embed.Title.Value : null);
        private static readonly Action<ILogger, string?> _unknownEventsInfoRetrieved
            = LoggerMessage.Define<string?>(
                    logLevel:       LogLevel.Information,
                    eventId:        ManagementLogEvent.UnknownEventsInfoRetrieved.ToEventId(),
                    formatString:   "Unknown events info retrieved: {InfoSummary}")
                .WithoutException();

        public static void UnknownEventsInfoRetrieving(ILogger logger)
            => _unknownEventsInfoRetrieving.Invoke(logger);
        private static readonly Action<ILogger> _unknownEventsInfoRetrieving
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Debug,
                    eventId:        ManagementLogEvent.UnknownEventsInfoRetrieving.ToEventId(),
                    formatString:   "Retrieving unknown events info")
                .WithoutException();
    }
}
