using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            ILogger<NotificationsService>           logger,
            MonitorService                          monitorService,
            IOptions<NotificationsConfiguration>    notificationsConfiguration)
        {
            _discordRestWebhookApi      = discordRestWebhookApi;
            _logger                     = logger;
            _monitorService             = monitorService;
            _notificationsConfiguration = notificationsConfiguration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            NotificationsLogger.NotificationsStarting(_logger);
            try
            {
                var delay = Task.CompletedTask;

                while (!stoppingToken.IsCancellationRequested)
                {
                    NotificationsLogger.UnknownEventsChecking(_logger);

                    var descriptors = _monitorService.EnumerateUnknownEvents()
                        .ToArray();

                    if (descriptors.Length == 0)
                        NotificationsLogger.UnknownEventsNotFound(_logger);
                    else
                        NotificationsLogger.UnknownEventsFound(_logger, descriptors.Length);

                    if ((descriptors.Length == 1) || delay.IsCompleted)
                    {
                        NotificationsLogger.NotificationRendering(_logger);
                        var embed = descriptors
                            .RenderInfo();

                        NotificationsLogger.NotificationSending(_logger, embed);
                        var executeResult = await _discordRestWebhookApi.ExecuteWebhookAsync(
                            webhookID:  new Snowflake(_notificationsConfiguration.Value.WebhookId),
                            token:      _notificationsConfiguration.Value.WebhookToken,
                            embeds:     new[] { embed },
                            ct:         stoppingToken);
                        
                        if (executeResult.IsSuccess)
                            NotificationsLogger.NotificationSent(_logger, embed);
                        else
                            NotificationsLogger.NotificationSendFailed(_logger, executeResult.Unwrap().Message);
                    }

                    if (delay.IsCompleted)
                    {
                        var interval = _notificationsConfiguration.Value.Interval;

                        NotificationsLogger.NotificationDelayResetting(_logger, interval);
                        delay = Task.Delay(interval, stoppingToken);
                    }

                    await Task.WhenAny(delay, _monitorService.WhenUnknownEventSavedAsync(stoppingToken));
                }
            }
            finally
            {
                NotificationsLogger.NotificationsStopped(_logger);
            }
        }

        private readonly IDiscordRestWebhookAPI                 _discordRestWebhookApi;
        private readonly ILogger                                _logger;
        private readonly MonitorService                         _monitorService;
        private readonly IOptions<NotificationsConfiguration>   _notificationsConfiguration;
    }
}
