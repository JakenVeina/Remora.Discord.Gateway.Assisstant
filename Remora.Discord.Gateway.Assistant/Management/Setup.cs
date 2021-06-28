using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Remora.Commands.Extensions;
using Remora.Discord.Commands.Extensions;

namespace Remora.Discord.Gateway.Assistant.Management
{
    public static class Setup
    {
        public static IServiceCollection AddManagement(this IServiceCollection services, HostBuilderContext context)
        {
            services.AddOptions<ManagementConfiguration>()
                .Bind(context.Configuration.GetSection(typeof(Setup).Namespace!.ToString().Replace('.', ':')))
                .ValidateDataAnnotations();

            return services
                .AddDiscordCommands(enableSlash: true)
                .AddCommandGroup<ManagementCommandGroup>()
                .AddHostedService<ManagementCommandDeploymentService>();
        }
    }
}
