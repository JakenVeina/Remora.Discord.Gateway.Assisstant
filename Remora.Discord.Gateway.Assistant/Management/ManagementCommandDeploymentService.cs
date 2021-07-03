using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Remora.Discord.Commands.Services;
using Remora.Discord.Core;
using Remora.Results;

namespace Remora.Discord.Gateway.Assistant.Management
{
    public class ManagementCommandDeploymentService
        : IHostedService
    {
        public ManagementCommandDeploymentService(
            ILogger<ManagementCommandDeploymentService> logger,
            IOptions<ManagementConfiguration>           managementConfiguration,
            SlashService                                slashService)
        {
            _logger                     = logger;
            _managementConfiguration    = managementConfiguration;
            _slashService               = slashService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            ManagementLogger.CommandsInitializing(_logger);

            var checkResult = _slashService.SupportsSlashCommands();
            if (!checkResult.IsSuccess)
                throw new InvalidOperationException($"The registered commands of the bot don't support slash commands: {checkResult.Unwrap().Message}", (checkResult.Unwrap() as ExceptionError)?.Exception);

            ManagementLogger.CommandsDeploying(_logger);

            var updateResult = await _slashService.UpdateSlashCommandsAsync(new Snowflake(_managementConfiguration.Value.GuildId), cancellationToken);
            if (!updateResult.IsSuccess)
                throw new InvalidOperationException($"Failed to deploy commands to guild {_managementConfiguration.Value.GuildId}: {updateResult.Unwrap().Message}", (updateResult.Unwrap() as ExceptionError)?.Exception);

            ManagementLogger.CommandsDeployed(_logger);
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;

        private readonly ILogger                            _logger;
        private readonly IOptions<ManagementConfiguration>  _managementConfiguration;
        private readonly SlashService                       _slashService;
    }
}
