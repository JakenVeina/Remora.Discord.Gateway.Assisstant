namespace Remora.Discord.Gateway.Assistant.Bot
{
    internal enum BotLogEvent
    {
        BotStopping = AssistantLogEvent.Bot + 0x000001,
        BotStopped  = AssistantLogEvent.Bot + 0x000002,
        BotStarting = AssistantLogEvent.Bot + 0x000003,
        BotStarted  = AssistantLogEvent.Bot + 0x000004
    }
}
