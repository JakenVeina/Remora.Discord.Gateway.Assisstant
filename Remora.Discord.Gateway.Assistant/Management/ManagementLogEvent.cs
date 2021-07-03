namespace Remora.Discord.Gateway.Assistant.Management
{
    internal enum ManagementLogEvent
    {
        CommandsInitializing        = AssistantLogEvent.Management + 0x000101,
        CommandsDeploying           = AssistantLogEvent.Management + 0x000102,
        CommandsDeployed            = AssistantLogEvent.Management + 0x000103,
        CommandFollowingUp          = AssistantLogEvent.Management + 0x000104,
        CommandFollowedUp           = AssistantLogEvent.Management + 0x000105,
        UnknownEventsInfoRetrieving = AssistantLogEvent.Management + 0x000201,
        UnknownEventsInfoRetrieved  = AssistantLogEvent.Management + 0x000202,
        UnknownEventsDownloading    = AssistantLogEvent.Management + 0x000203,
        UnknownEventsDownloaded     = AssistantLogEvent.Management + 0x000204,
        UnknownEventsClearing       = AssistantLogEvent.Management + 0x000205,
        UnknownEventsCleared        = AssistantLogEvent.Management + 0x000206
    }
}
