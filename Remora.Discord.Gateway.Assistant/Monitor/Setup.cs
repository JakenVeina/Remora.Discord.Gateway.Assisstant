using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Remora.Discord.Gateway.Extensions;

namespace Remora.Discord.Gateway.Assistant.Monitor
{
    public static class Setup
    {
        public static IServiceCollection AddMonitor(this IServiceCollection services, HostBuilderContext context)
        {
            services.AddOptions<MonitorConfiguration>()
                .Bind(context.Configuration.GetSection(typeof(Setup).Namespace!.ToString().Replace('.', ':')))
                .ValidateDataAnnotations();

            return services
                .AddResponder<MonitorResponder>()
                .AddSingleton<MonitorService>()
                .AddHostedService(serviceProvider => serviceProvider.GetRequiredService<MonitorService>());
        }
    }
}
