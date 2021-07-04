namespace Remora.Discord.Gateway.Assistant.Management
{
    internal enum ManagementLogEvent
    {
        CommandsInitializing                = AssistantLogEvent.Management + 0x000101,
        CommandsDeploying                   = AssistantLogEvent.Management + 0x000102,
        CommandsDeployed                    = AssistantLogEvent.Management + 0x000103,
        CommandFollowingUp                  = AssistantLogEvent.Management + 0x000201,
        CommandFollowUpFailed               = AssistantLogEvent.Management + 0x000202,
        CommandFollowedUp                   = AssistantLogEvent.Management + 0x000203,
        UnknownEventsInfoRetrieving         = AssistantLogEvent.Management + 0x000204,
        UnknownEventsInfoRetrieved          = AssistantLogEvent.Management + 0x000205,
        UnknownEventsDownloading            = AssistantLogEvent.Management + 0x000206,
        UnknownEventsDownloaded             = AssistantLogEvent.Management + 0x000207,
        UnknownEventsClearing               = AssistantLogEvent.Management + 0x000208,
        UnknownEventsCleared                = AssistantLogEvent.Management + 0x000209,
        DirectMessageChannelCreating        = AssistantLogEvent.Management + 0x00020A,
        DirectMessageChannelCreateFailed    = AssistantLogEvent.Management + 0x00020B,
        DirectMessageChannelCreated         = AssistantLogEvent.Management + 0x00020C,
        DirectMessageFailed                 = AssistantLogEvent.Management + 0x00020D,
        OldMessagesDownloading              = AssistantLogEvent.Management + 0x00020E,
        OldMessagesDownloadFailed           = AssistantLogEvent.Management + 0x00020F,
        OldMessageChecking                  = AssistantLogEvent.Management + 0x000210,
        OldMessageDeleting                  = AssistantLogEvent.Management + 0x000211,
        OldMessageDeleteFailed              = AssistantLogEvent.Management + 0x000212,
        OldMessageDeleted                   = AssistantLogEvent.Management + 0x000213
    }
}
