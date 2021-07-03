using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Remora.Discord.Gateway.Assistant.Bot
{
    public class BotRestartBehavior
        : BackgroundService
    {
        public BotRestartBehavior(
            BotService                  botService,
            IOptions<BotConfiguration>  botConfiguration,
            ILogger<BotRestartBehavior> logger)
        {
            _botService         = botService;
            _botConfiguration   = botConfiguration;
            _logger             = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_botConfiguration.Value.RestartInterval, stoppingToken);

                BotLogger.BotStopping(_logger);
                await _botService.StopAsync(stoppingToken);
                BotLogger.BotStopped(_logger);
                
                BotLogger.BotStarting(_logger);
                await _botService.StartAsync(stoppingToken);
                BotLogger.BotStarted(_logger);
            }
        }

        private readonly BotService                 _botService;
        private readonly IOptions<BotConfiguration> _botConfiguration;
        private readonly ILogger                    _logger;
    }
}
