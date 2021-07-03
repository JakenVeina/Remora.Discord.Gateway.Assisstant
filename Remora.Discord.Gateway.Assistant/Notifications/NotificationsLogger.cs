using System;

using Microsoft.Extensions.Logging;

using Remora.Discord.API.Abstractions.Objects;

namespace Remora.Discord.Gateway.Assistant.Notifications
{
    internal static class NotificationsLogger
    {
        public static void NotificationDelayResetting(
                ILogger     logger,
                TimeSpan    interval)
            => _notificationDelayResetting.Invoke(
                logger,
                interval);
        private static readonly Action<ILogger, TimeSpan> _notificationDelayResetting
            = LoggerMessage.Define<TimeSpan>(
                    logLevel:       LogLevel.Debug,
                    eventId:        NotificationsLogEvent.NotificationDelayResetting.ToEventId(),
                    formatString:   "Resetting notification delay: {Interval}")
                .WithoutException();

        public static void NotificationRendering(ILogger logger)
            => _notificationRendering.Invoke(logger);
        private static readonly Action<ILogger> _notificationRendering
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Debug,
                    eventId:        NotificationsLogEvent.NotificationRendering.ToEventId(),
                    formatString:   "Rendering notification")
                .WithoutException();

        public static void NotificationSending(
                ILogger logger,
                IEmbed  embed)
            => _notificationSending.Invoke(
                logger,
                embed.Title.HasValue ? embed.Title.Value : null);
        private static readonly Action<ILogger, string?> _notificationSending
            = LoggerMessage.Define<string?>(
                    logLevel:       LogLevel.Information,
                    eventId:        NotificationsLogEvent.NotificationSending.ToEventId(),
                    formatString:   "Sending notification: {NotificationTitle}")
                .WithoutException();

        public static void NotificationSendFailed(
                ILogger logger,
                string  failureMessage)
            => _notificationSendFailed.Invoke(
                logger,
                failureMessage);
        private static readonly Action<ILogger, string> _notificationSendFailed
            = LoggerMessage.Define<string>(
                    logLevel:       LogLevel.Warning,
                    eventId:        NotificationsLogEvent.NotificationSendFailed.ToEventId(),
                    formatString:   "Notification failed to send: {FailureMessage}")
                .WithoutException();

        public static void NotificationSent(
                ILogger logger,
                IEmbed  embed)
            => _notificationSent.Invoke(
                logger,
                embed.Title.HasValue ? embed.Title.Value : null);
        private static readonly Action<ILogger, string?> _notificationSent
            = LoggerMessage.Define<string?>(
                    logLevel:       LogLevel.Debug,
                    eventId:        NotificationsLogEvent.NotificationSent.ToEventId(),
                    formatString:   "Notification sent: {NotificationTitle}")
                .WithoutException();

        public static void NotificationsStarting(ILogger logger)
            => _notificationsStarting.Invoke(logger);
        private static readonly Action<ILogger> _notificationsStarting
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Information,
                    eventId:        NotificationsLogEvent.NotificationsStarting.ToEventId(),
                    formatString:   "Starting notifications service")
                .WithoutException();

        public static void NotificationsStopped(ILogger logger)
            => _notificationsStopped.Invoke(logger);
        private static readonly Action<ILogger> _notificationsStopped
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Information,
                    eventId:        NotificationsLogEvent.NotificationsStopped.ToEventId(),
                    formatString:   "Notifications service stopped")
                .WithoutException();

        public static void UnknownEventsChecking(ILogger logger)
            => _unknownEventsChecking.Invoke(logger);
        private static readonly Action<ILogger> _unknownEventsChecking
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Debug,
                    eventId:        NotificationsLogEvent.UnknownEventsChecking.ToEventId(),
                    formatString:   "Checking for unknown events")
                .WithoutException();

        public static void UnknownEventsFound(
                ILogger logger,
                int     unknownEventCount)
            => _unknownEventsFound.Invoke(
                logger,
                unknownEventCount);
        private static readonly Action<ILogger, int> _unknownEventsFound
            = LoggerMessage.Define<int>(
                    logLevel:       LogLevel.Debug,
                    eventId:        NotificationsLogEvent.UnknownEventsFound.ToEventId(),
                    formatString:   "Found {UnknownEventCount} unknown events")
                .WithoutException();

        public static void UnknownEventsNotFound(ILogger logger)
            => _unknownEventsNotFound.Invoke(logger);
        private static readonly Action<ILogger> _unknownEventsNotFound
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Debug,
                    eventId:        NotificationsLogEvent.UnknownEventsNotFound.ToEventId(),
                    formatString:   "No unknown events found")
                .WithoutException();
    }
}
