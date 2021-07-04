using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Commands.Contexts;
using Remora.Discord.Core;
using Remora.Discord.Gateway.Assistant.Monitor;
using Remora.Results;

namespace Remora.Discord.Gateway.Assistant.Management
{
    [Group("assistant")]
    public class ManagementCommandGroup
        : CommandGroup
    {
        [Group("unknown-events")]
        public class UnknownEventsCommandGroup
            : CommandGroup
        {
            // TODO: Revert all this back to use the interactions API, when https://github.com/Nihlus/Remora.Discord/pull/69 becomes available.
            public UnknownEventsCommandGroup(
                IDiscordRestChannelAPI              discordRestChannelApi,
                //IDiscordRestInteractionAPI          discordRestInteractionApi,
                IDiscordRestUserAPI                 discordRestUserApi,
                IDiscordRestWebhookAPI              discordRestWebhookApi,
                ILogger<UnknownEventsCommandGroup>  logger,
                MonitorService                      monitorService,
                InteractionContext                  interactionContext)
            {
                _discordRestChannelApi      = discordRestChannelApi;
                //_discordRestInteractionApi  = discordRestInteractionApi;
                _discordRestUserApi         = discordRestUserApi;
                _discordRestWebhookApi      = discordRestWebhookApi;
                _interactionContext         = interactionContext;
                _logger                     = logger;
                _monitorService             = monitorService;
            }

            [Command("clear")]
            [Description("Removes all unknown events from the cache.")]
            public async Task<Result> ClearAsync()
            {
                ManagementLogger.UnknownEventsClearing(_logger);
                _monitorService.ClearUnknownEvents();
                ManagementLogger.UnknownEventsCleared(_logger);

                //var result = await _discordRestInteractionApi.CreateInteractionResponseAsync(
                //    interactionID:      _interactionContext.ID,
                //    interactionToken:   _interactionContext.Token,
                //    response:           new InteractionResponse(
                //        Type:   InteractionCallbackType.ChannelMessageWithSource,
                //        Data:   new InteractionApplicationCommandCallbackData(
                //            Embeds:     new[]
                //            {
                //                new Embed(
                //                    Title:      "The Unknown Events cache has been cleared.",
                //                    Timestamp:  DateTimeOffset.UtcNow,
                //                    Footer:     new EmbedFooter($"API Version {MonitorService.RemoraApiVersion}"))
                //            })),
                //    ct:                 CancellationToken);

                //return result;

                ManagementLogger.CommandFollowingUp(_logger);
                var followupResult = await _discordRestWebhookApi.CreateFollowupMessageAsync(
                    applicationID:  _interactionContext.ApplicationID,
                    token:          _interactionContext.Token,
                    embeds:         new[]
                    {
                        new Embed(
                            Title:      "The unknown events cache has been cleared.",
                            Timestamp:  DateTimeOffset.UtcNow,
                            Footer:     new EmbedFooter($"API Version {MonitorService.RemoraApiVersion}"))
                    },
                    ct:             CancellationToken);
                ManagementLogger.CommandFollowedUp(_logger, followupResult);

                return followupResult.IsSuccess
                    ? Result.FromSuccess()
                    : Result.FromError(followupResult.Error);
            }

            [Command("download")]
            [Description("Provides a downloadable ZIP file, containing all unknown events currently in the cache.")]
            public async Task<Result> DownloadAsync()
            {
                //var responseResult = await _discordRestInteractionApi.CreateInteractionResponseAsync(
                //    interactionID:      _interactionContext.ID,
                //    interactionToken:   _interactionContext.Token,
                //    response:           new InteractionResponse(
                //        Type:   InteractionCallbackType.DeferredChannelMessageWithSource),
                //    ct:                 CancellationToken);
                //if (!responseResult.IsSuccess)
                //    return responseResult;

                ManagementLogger.UnknownEventsDownloading(_logger);
                using var memoryStream = new MemoryStream();
                var didWrite = await _monitorService.TryArchiveUnknownEventsTo(memoryStream, CancellationToken);
                memoryStream.Position = 0;
                ManagementLogger.UnknownEventsDownloaded(_logger, memoryStream);

                var error = null as IResultError;
                var deleteOperation = null as Task;
                if (didWrite)
                {
                    var userId = _interactionContext.User.ID;

                    ManagementLogger.DirectMessageChannelCreating(_logger, userId);
                    var channelResult = await _discordRestUserApi.CreateDMAsync(userId, CancellationToken);
                    if (!channelResult.IsSuccess)
                    {
                        error = channelResult.Unwrap();
                        ManagementLogger.DirectMessageChannelCreateFailed(_logger, error);
                    }
                    else
                    {
                        var channelId = channelResult.Entity.ID;

                        ManagementLogger.DirectMessageChannelCreated(_logger, channelId);

                        var messageResult = await _discordRestChannelApi.CreateMessageAsync(
                            channelID:  channelId,
                            embeds:     new[]
                            {
                                new Embed(
                                    Timestamp:  DateTimeOffset.UtcNow,
                                    Footer:     new EmbedFooter($"API Version {MonitorService.RemoraApiVersion}"))
                            },
                            file:       new FileData("unknown-events.zip", memoryStream),
                            ct:         CancellationToken);
                        if (!messageResult.IsSuccess)
                        {
                            error = messageResult.Unwrap();
                            ManagementLogger.DirectMessageFailed(_logger, error);
                        }
                        else
                            deleteOperation = DeleteOldMessagesAsync(
                                channelId:  channelId,
                                authorId:   messageResult.Entity.Author.ID,
                                before:     messageResult.Entity.ID);
                    }
                }

                ManagementLogger.CommandFollowingUp(_logger);
                var followupResult = await _discordRestWebhookApi.CreateFollowupMessageAsync(
                    applicationID:  _interactionContext.ApplicationID,
                    token:          _interactionContext.Token,
                    embeds:         new[]
                    {
                        new Embed(
                            Title:      (error is not null) ? $"An error occurred during processing of the download: {error.Message}"
                                :       didWrite            ? "Your download is ready. Please check your Direct Messages."
                                                            : "There are no unknown events available for download.",
                            Timestamp:  DateTimeOffset.UtcNow,
                            Footer:     new EmbedFooter($"API Version {MonitorService.RemoraApiVersion}"))
                    },
                    ct:             CancellationToken);
                if (!followupResult.IsSuccess)
                {
                    error = followupResult.Unwrap();
                    ManagementLogger.CommandFollowUpFailed(_logger, error);
                }
                else
                    ManagementLogger.CommandFollowedUp(_logger, followupResult);

                if (deleteOperation is not null)
                    await deleteOperation;

                return (error is null)
                    ? Result.FromSuccess()
                    : Result.FromError(error);

                async Task DeleteOldMessagesAsync(
                    Snowflake channelId,
                    Snowflake authorId,
                    Snowflake before)
                {
                    ManagementLogger.OldMessagesDownloading(_logger, channelId, before);

                    var oldMessagesResult = await _discordRestChannelApi.GetChannelMessagesAsync(
                        channelID:  channelId,
                        before:     before,
                        ct:         CancellationToken);
                    if (!oldMessagesResult.IsSuccess)
                    {
                        ManagementLogger.OldMessagesDownloadFailed(_logger, oldMessagesResult.Unwrap());
                        return;
                    }

                    foreach(var oldMessage in oldMessagesResult.Entity)
                    {
                        ManagementLogger.OldMessageChecking(_logger, oldMessage.ID, oldMessage.Author.ID);
                        
                        if ((oldMessage.Author.ID == authorId) && (oldMessage.Attachments.Any(a => a.Filename == "unknown-events.zip")))
                        {
                            ManagementLogger.OldMessageDeleting(_logger, oldMessage.ID);
                            var deleteResult = await _discordRestChannelApi.DeleteMessageAsync(
                                channelID:  channelId,
                                messageID:  oldMessage.ID,
                                ct:         CancellationToken);

                            if (!deleteResult.IsSuccess)
                                ManagementLogger.OldMessageDeleteFailed(_logger, deleteResult.Unwrap());
                            else
                                ManagementLogger.OldMessageDeleted(_logger);
                        }
                    }
                }
            }

            [Command("info")]
            [Description("Lists information about any unknown events currently stored in the cache.")]
            public async Task<Result> GetInfoAsync()
            {
                ManagementLogger.UnknownEventsInfoRetrieving(_logger);
                var embed = _monitorService.EnumerateUnknownEvents()
                    .ToArray()
                    .RenderInfo();
                ManagementLogger.UnknownEventsInfoRetrieved(_logger, embed);

                //var result = await _discordRestInteractionApi.CreateInteractionResponseAsync(
                //    interactionID:      _interactionContext.ID,
                //    interactionToken:   _interactionContext.Token,
                //    response:           new InteractionResponse(
                //        Type:   InteractionCallbackType.ChannelMessageWithSource,
                //        Data:   new InteractionApplicationCommandCallbackData(
                //            Embeds: new[] { embed })),
                //    ct:                 CancellationToken);

                //return result;

                ManagementLogger.CommandFollowingUp(_logger);
                var followupResult = await _discordRestWebhookApi.CreateFollowupMessageAsync(
                    applicationID:  _interactionContext.ApplicationID,
                    token:          _interactionContext.Token,
                    embeds:         new[] { embed },
                    ct:             CancellationToken);
                ManagementLogger.CommandFollowedUp(_logger, followupResult);

                return followupResult.IsSuccess
                    ? Result.FromSuccess()
                    : Result.FromError(followupResult.Error);
            }

            private readonly IDiscordRestChannelAPI         _discordRestChannelApi;
            //private readonly IDiscordRestInteractionAPI     _discordRestInteractionApi;
            private readonly IDiscordRestUserAPI            _discordRestUserApi;
            private readonly IDiscordRestWebhookAPI         _discordRestWebhookApi;
            private readonly InteractionContext             _interactionContext;
            private readonly ILogger                        _logger;
            private readonly MonitorService                 _monitorService;
        }
    }
}
