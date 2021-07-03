namespace Remora.Discord.Gateway.Assistant.Notifications
{
    internal enum NotificationsLogEvent
    {
        NotificationsStarting       = AssistantLogEvent.Notifications + 0x000001,
        NotificationsStopped        = AssistantLogEvent.Notifications + 0x000002,
        UnknownEventsChecking       = AssistantLogEvent.Notifications + 0x000003,
        UnknownEventsNotFound       = AssistantLogEvent.Notifications + 0x000004,
        UnknownEventsFound          = AssistantLogEvent.Notifications + 0x000005,
        NotificationRendering       = AssistantLogEvent.Notifications + 0x000006,
        NotificationSending         = AssistantLogEvent.Notifications + 0x000007,
        NotificationSendFailed      = AssistantLogEvent.Notifications + 0x000008,
        NotificationSent            = AssistantLogEvent.Notifications + 0x000009,
        NotificationDelayResetting  = AssistantLogEvent.Notifications + 0x00000A
    }
}
