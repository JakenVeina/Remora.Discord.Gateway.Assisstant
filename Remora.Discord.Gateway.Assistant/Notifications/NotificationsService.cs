using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.API.Objects;
using Remora.Discord.Core;
using Remora.Discord.Gateway.Assistant.Monitor;

namespace Remora.Discord.Gateway.Assistant.Notifications
{
    public class NotificationsService
        : BackgroundService
    {
        public NotificationsService(
            IDiscordRestWebhookAPI                  discordRestWebhookApi,
            IOptions<MonitorConfiguration>          monitorConfiguration,
            MonitorService                          monitorService,
            IOptions<NotificationsConfiguration>    notificationsConfiguration)
        {
            _discordRestWebhookApi      = discordRestWebhookApi;
            _monitorConfiguration       = monitorConfiguration;
            _monitorService             = monitorService;
            _notificationsConfiguration = notificationsConfiguration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var delay = Task.CompletedTask;

            while (!stoppingToken.IsCancellationRequested)
            {
                if (Directory.Exists(_monitorConfiguration.Value.EventsLogPath))
                {
                    var descriptors = _monitorService.EnumerateUnknownEvents()
                        .ToArray();

                    if ((descriptors.Length == 1) || ((descriptors.Length > 0) && delay.IsCompleted))
                    {
                        var embed = descriptors
                            .RenderInfo();

                        await _discordRestWebhookApi.ExecuteWebhookAsync(
                            webhookID:  new Snowflake(_notificationsConfiguration.Value.WebhookId),
                            token:      _notificationsConfiguration.Value.WebhookToken,
                            embeds:     new[] { embed },
                            ct:         stoppingToken);
                    }
                }

                if (delay.IsCompleted)
                    delay = Task.Delay(_notificationsConfiguration.Value.Interval, stoppingToken);

                await Task.WhenAny(delay, _monitorService.WhenRecordedAsync(stoppingToken));
            }
        }

        private readonly IDiscordRestWebhookAPI                 _discordRestWebhookApi;
        private readonly IOptions<MonitorConfiguration>         _monitorConfiguration;
        private readonly MonitorService                         _monitorService;
        private readonly IOptions<NotificationsConfiguration>   _notificationsConfiguration;
    }
}
