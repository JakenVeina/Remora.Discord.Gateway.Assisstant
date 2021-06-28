using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

namespace Remora.Discord.Gateway.Assistant.Bot
{
    public class BotService
        : BackgroundService
    {
        public BotService(DiscordGatewayClient client)
            => _client = client;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
            => _client.RunAsync(stoppingToken);

        private readonly DiscordGatewayClient _client;
    }
}
