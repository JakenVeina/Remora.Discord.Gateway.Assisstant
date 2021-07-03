namespace Remora.Discord.Gateway.Assistant.Monitor
{
    internal enum MonitorLogEvent
    {
        UnknownEventReceived            = AssistantLogEvent.Monitor + 0x000101,
        UnknownEventRecording           = AssistantLogEvent.Monitor + 0x000102,
        UnknownEventRecorded            = AssistantLogEvent.Monitor + 0x000103,
        MonitorStarting                 = AssistantLogEvent.Monitor + 0x000201,
        MonitorStopped                  = AssistantLogEvent.Monitor + 0x000202,
        UnknownEventsEnumerating        = AssistantLogEvent.Monitor + 0x000203,
        UnknownEventsLogPathNotFound    = AssistantLogEvent.Monitor + 0x000204,
        UnknownEventsLogPathEmpty       = AssistantLogEvent.Monitor + 0x000205,
        UnknownEventsLogFilesFound      = AssistantLogEvent.Monitor + 0x000206,
        UnknownEventLogFileSaving       = AssistantLogEvent.Monitor + 0x000207,
        UnknownEventLogFileSaved        = AssistantLogEvent.Monitor + 0x000208,
        UnknownEventLogFileParsing      = AssistantLogEvent.Monitor + 0x000209,
        UnknownEventLogFileParseFailed  = AssistantLogEvent.Monitor + 0x00020A,
        UnknownEventLogFileParsed       = AssistantLogEvent.Monitor + 0x00020B,
        UnknownEventsArchiving          = AssistantLogEvent.Monitor + 0x00020C,
        UnknownEventsArchived           = AssistantLogEvent.Monitor + 0x00020D,
        UnknownEventLogFileArchiving    = AssistantLogEvent.Monitor + 0x00020E,
        UnknownEventLogFileArchived     = AssistantLogEvent.Monitor + 0x00020F,
        UnknownEventsDeleting           = AssistantLogEvent.Monitor + 0x000210,
        UnknownEventsDeleted            = AssistantLogEvent.Monitor + 0x000211,
        UnknownEventLogFileDeleting     = AssistantLogEvent.Monitor + 0x000212,
        UnknownEventLogFileDeleted      = AssistantLogEvent.Monitor + 0x000213,
    }
}
