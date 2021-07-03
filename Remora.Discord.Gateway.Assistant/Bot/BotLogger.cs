using System;

using Microsoft.Extensions.Logging;

namespace Remora.Discord.Gateway.Assistant.Bot
{
    internal static class BotLogger
    {
        public static void BotStarted(ILogger logger)
            => _botStarted.Invoke(logger);
        private static readonly Action<ILogger> _botStarted
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Information,
                    eventId:        BotLogEvent.BotStarted.ToEventId(),
                    formatString:   "Bot started")
                .WithoutException();

        public static void BotStarting(ILogger logger)
            => _botStarting.Invoke(logger);
        private static readonly Action<ILogger> _botStarting
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Information,
                    eventId:        BotLogEvent.BotStarting.ToEventId(),
                    formatString:   "Starting bot")
                .WithoutException();

        public static void BotStopped(ILogger logger)
            => _botStopped.Invoke(logger);
        private static readonly Action<ILogger> _botStopped
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Information,
                    eventId:        BotLogEvent.BotStopped.ToEventId(),
                    formatString:   "Bot stopped")
                .WithoutException();

        public static void BotStopping(ILogger logger)
            => _botStopping.Invoke(logger);
        private static readonly Action<ILogger> _botStopping
            = LoggerMessage.Define(
                    logLevel:       LogLevel.Information,
                    eventId:        BotLogEvent.BotStopping.ToEventId(),
                    formatString:   "Stopping bot")
                .WithoutException();
    }
}
