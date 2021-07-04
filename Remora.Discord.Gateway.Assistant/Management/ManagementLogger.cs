using System;
using System.IO;

using Microsoft.Extensions.Logging;

using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Core;
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

        public static void CommandFollowUpFailed(
                ILogger         logger,
                IResultError    error)
            => _commandFollowUpFailed.Invoke(
                logger,
                error);
        private static readonly Action<ILogger, IResultError> _commandFollowUpFailed
            = LoggerMessage.Define<IResultError>(
                    logLevel:       LogLevel.Error,
                    eventId:        ManagementLogEvent.CommandFollowUpFailed.ToEventId(),
                    formatString:   "Command followup failed: {Error}")
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

        public static void DirectMessageChannelCreated(
                ILogger     logger,
                Snowflake   channelId)
            => _directMessageChannelCreated.Invoke(
                logger,
                channelId);
        private static readonly Action<ILogger, Snowflake> _directMessageChannelCreated
            = LoggerMessage.Define<Snowflake>(
                    logLevel:       LogLevel.Debug,
                    eventId:        ManagementLogEvent.DirectMessageChannelCreated.ToEventId(),
                    formatString:   "Direct message channel created: {ChannelId}")
                .WithoutException();

        public static void DirectMessageChannelCreateFailed(
                ILogger         logger,
                IResultError    error)
            => _directMessageChannelCreateFailed.Invoke(
                logger,
                error);
        private static readonly Action<ILogger, IResultError> _directMessageChannelCreateFailed
            = LoggerMessage.Define<IResultError>(
                    logLevel:       LogLevel.Error,
                    eventId:        ManagementLogEvent.DirectMessageChannelCreateFailed.ToEventId(),
                    formatString:   "Direct message channel creation failed: {Error}")
                .WithoutException();

        public static void DirectMessageChannelCreating(
                ILogger     logger,
                Snowflake   userId)
            => _directMessageChannelCreating.Invoke(
                logger,
                userId);
        private static readonly Action<ILogger, Snowflake> _directMessageChannelCreating
            = LoggerMessage.Define<Snowflake>(
                    logLevel:       LogLevel.Information,
                    eventId:        ManagementLogEvent.DirectMessageChannelCreating.ToEventId(),
                    formatString:   "Creating direct message channel: UserId: {UserId}")
                .WithoutException();

        public static void DirectMessageFailed(
                ILogger         logger,
                IResultError    error)
            => _directMessageFailed.Invoke(
                logger,
                error);
        private static readonly Action<ILogger, IResultError> _directMessageFailed
            = LoggerMessage.Define<IResultError>(
                    logLevel:       LogLevel.Error,
                    eventId:        ManagementLogEvent.DirectMessageFailed.ToEventId(),
                    formatString:   "Direct message failed: {Error}")
                .WithoutException();

        public static void OldMessageChecking(
                ILogger     logger,
                Snowflake   messageId,
                Snowflake   authorId,
                Snowflake   botId)
            => _oldMessageChecking.Invoke(
                logger,
                messageId,
                authorId,
                botId);
        private static readonly Action<ILogger, Snowflake, Snowflake, Snowflake> _oldMessageChecking
            = LoggerMessage.Define<Snowflake, Snowflake, Snowflake>(
                    logLevel:       LogLevel.Debug,
                    eventId:        ManagementLogEvent.OldMessageChecking.ToEventId(),
                    formatString:   "Checking old message {MessageId} by {AuthorId} (looking for {BotId})")
                .WithoutException();

        public static void OldMessageDeleted(ILogger logger)
            => _oldMessageDeleted.Invoke(logger);
        private static readonly Action<ILogger> _oldMessageDeleted
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Debug,
                    eventId:        ManagementLogEvent.OldMessageDeleted.ToEventId(),
                    formatString:   "Old message deleted")
                .WithoutException();

        public static void OldMessageDeleteFailed(
                ILogger logger,
                IResultError error)
            => _oldMessageDeleteFailed.Invoke(
                logger,
                error);
        private static readonly Action<ILogger, IResultError> _oldMessageDeleteFailed
            = LoggerMessage.Define<IResultError>(
                    logLevel:       LogLevel.Warning,
                    eventId:        ManagementLogEvent.OldMessageDeleteFailed.ToEventId(),
                    formatString:   "Failed to delete old message: {Error}")
                .WithoutException();

        public static void OldMessageDeleting(
                ILogger     logger,
                Snowflake   messageId)
            => _oldMessagesDeleting.Invoke(
                logger,
                messageId);
        private static readonly Action<ILogger, Snowflake> _oldMessagesDeleting
            = LoggerMessage.Define<Snowflake>(
                    logLevel:       LogLevel.Debug,
                    eventId:        ManagementLogEvent.OldMessageDeleting.ToEventId(),
                    formatString:   "Deleting old message {MessageId}")
                .WithoutException();

        public static void OldMessagesDownloadFailed(
                ILogger         logger,
                IResultError    error)
            => _oldMessagesDownloadFailed.Invoke(
                logger,
                error);
        private static readonly Action<ILogger, IResultError> _oldMessagesDownloadFailed
            = LoggerMessage.Define<IResultError>(
                    logLevel:       LogLevel.Warning,
                    eventId:        ManagementLogEvent.OldMessagesDownloadFailed.ToEventId(),
                    formatString:   "Failed to download old messages: {Error}")
                .WithoutException();

        public static void OldMessagesDownloading(
                ILogger     logger,
                Snowflake   channelId,
                Snowflake   before)
            => _oldMessagesDownloading.Invoke(
                logger,
                channelId,
                before);
        private static readonly Action<ILogger, Snowflake, Snowflake> _oldMessagesDownloading
            = LoggerMessage.Define<Snowflake, Snowflake>(
                    logLevel:       LogLevel.Debug,
                    eventId:        ManagementLogEvent.OldMessagesDownloading.ToEventId(),
                    formatString:   "Downloading old messages in channel {ChannelId} before {Before}")
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
