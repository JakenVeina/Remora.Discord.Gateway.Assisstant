using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
    [Group("assitant")]
    public class ManagementCommandGroup
        : CommandGroup
    {
        [Group("unknown-events")]
        public class UnknownEventsCommandGroup
            : CommandGroup
        {
            // TODO: Revert all this back to use the interactions API, when https://github.com/Nihlus/Remora.Discord/pull/69 becomes available.
            public UnknownEventsCommandGroup(
                //IDiscordRestInteractionAPI  discordRestInteractionApi,
                IDiscordRestWebhookAPI      discordRestWebhookApi,
                MonitorService              monitorService,
                InteractionContext          interactionContext)
            {
                //_discordRestInteractionApi  = discordRestInteractionApi;
                _discordRestWebhookApi      = discordRestWebhookApi;
                _interactionContext         = interactionContext;
                _monitorService             = monitorService;
            }

            [Command("clear")]
            [Description("Removes all unknown events from the cache.")]
            public async Task<Result> ClearAsync()
            {
                _monitorService.ClearUnknownEvents();

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

                using var memoryStream = new MemoryStream();
                var didWrite = await _monitorService.TryWriteUnknownEventsTo(memoryStream, CancellationToken);
                memoryStream.Position = 0;

                var followupResult = await _discordRestWebhookApi.CreateFollowupMessageAsync(
                    applicationID:  _interactionContext.ApplicationID,
                    token:          _interactionContext.Token,
                    embeds:         new[]
                    {
                        new Embed(
                            Title:      didWrite
                                ? default(Optional<string>)
                                : "There are no unknown events available for download.",
                            Timestamp:  DateTimeOffset.UtcNow,
                            Footer:     new EmbedFooter($"API Version {MonitorService.RemoraApiVersion}"))
                    },
                    file:           new FileData("unknown-events.zip", memoryStream),
                    ct:             CancellationToken);
                return followupResult.IsSuccess
                    ? Result.FromSuccess()
                    : Result.FromError(followupResult.Error);
            }

            [Command("info")]
            [Description("Lists information about any unknown events currently stored in the cache.")]
            public async Task<Result> GetInfoAsync()
            {
                var embed = _monitorService.EnumerateUnknownEvents()
                    .ToArray()
                    .RenderInfo();

                //var result = await _discordRestInteractionApi.CreateInteractionResponseAsync(
                //    interactionID:      _interactionContext.ID,
                //    interactionToken:   _interactionContext.Token,
                //    response:           new InteractionResponse(
                //        Type:   InteractionCallbackType.ChannelMessageWithSource,
                //        Data:   new InteractionApplicationCommandCallbackData(
                //            Embeds: new[] { embed })),
                //    ct:                 CancellationToken);

                //return result;

                var followupResult = await _discordRestWebhookApi.CreateFollowupMessageAsync(
                    applicationID:  _interactionContext.ApplicationID,
                    token:          _interactionContext.Token,
                    embeds:         new[] { embed },
                    ct:             CancellationToken);
                return followupResult.IsSuccess
                    ? Result.FromSuccess()
                    : Result.FromError(followupResult.Error);
            }

            //private readonly IDiscordRestInteractionAPI     _discordRestInteractionApi;
            private readonly IDiscordRestWebhookAPI         _discordRestWebhookApi;
            private readonly InteractionContext             _interactionContext;
            private readonly MonitorService                 _monitorService;
        }
    }
}
