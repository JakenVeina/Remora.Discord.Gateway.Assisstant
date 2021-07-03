using System.IO;
using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Remora.Discord.Gateway.Assistant.Bot;
using Remora.Discord.Gateway.Assistant.Management;
using Remora.Discord.Gateway.Assistant.Monitor;
using Remora.Discord.Gateway.Assistant.Notifications;

namespace Remora.Discord.Gateway.Assistant
{
    public static class EntryPoint
    {
        public static void Main()
        {
            using var host = new HostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConsoleLifetime()
                .UseDefaultServiceProvider((context, options) =>
                {
                    var isDevelopment = context.HostingEnvironment.IsDevelopment();

                    options.ValidateOnBuild = isDevelopment;
                    options.ValidateScopes  = isDevelopment;
                })
                .ConfigureHostConfiguration(builder => builder
                    .AddEnvironmentVariables(prefix: "DOTNET_"))
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);

                    if (context.HostingEnvironment.IsDevelopment())
                        builder
                            .AddUserSecrets(Assembly.GetExecutingAssembly());
                })
                .ConfigureServices((context, services) => services
                    .AddBot(context)
                    .AddManagement(context)
                    .AddMonitor(context)
                    .AddNotifications(context))
                .ConfigureLogging((context, builder) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
                        builder
                            .AddConfiguration(context.Configuration.GetSection("Logging"))
                            .AddConsole()
                            .AddDebug();
                })
                .Build();

            host.Run();
        }
    }
}
