using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Remora.Discord.Gateway.Assistant.Bot
{
    public class BotRestartBehavior
        : BackgroundService
    {
        public BotRestartBehavior(
            BotService                      botService,
            IOptions<BotConfiguration>  botConfiguration)
        {
            _botService         = botService;
            _botConfiguration   = botConfiguration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_botConfiguration.Value.RestartInterval, stoppingToken);

                await _botService.StopAsync(stoppingToken);
                await _botService.StartAsync(stoppingToken);
            }
        }

        private readonly BotService                 _botService;
        private readonly IOptions<BotConfiguration> _botConfiguration;
    }
}
