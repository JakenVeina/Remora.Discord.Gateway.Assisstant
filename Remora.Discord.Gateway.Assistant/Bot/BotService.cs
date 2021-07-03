using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Remora.Discord.Gateway.Assistant.Bot
{
    public class BotService
        : BackgroundService
    {
        public BotService(
            DiscordGatewayClient    client,
            ILogger<BotService>     logger)
        {
            _client = client;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            BotLogger.BotStarting(_logger);
            try
            {
                await _client.RunAsync(stoppingToken);
            }
            finally
            {
                BotLogger.BotStopped(_logger);
            }
        }

        private readonly DiscordGatewayClient   _client;
        private readonly ILogger                _logger;
    }
}
