using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Remora.Discord.Gateway.Assistant.Notifications
{
    public static class Setup
    {
        public static IServiceCollection AddNotifications(this IServiceCollection services, HostBuilderContext context)
        {
            services.AddOptions<NotificationsConfiguration>()
                .Bind(context.Configuration.GetSection(typeof(Setup).Namespace!.ToString().Replace('.', ':')))
                .ValidateDataAnnotations();

            return services
                .AddHostedService<NotificationsService>();
        }
    }
}
