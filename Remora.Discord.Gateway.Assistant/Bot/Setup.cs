using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Remora.Discord.API.Abstractions.Gateway.Commands;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Gateway.Commands;
using Remora.Discord.API.Objects;
using Remora.Discord.Gateway.Extensions;

namespace Remora.Discord.Gateway.Assistant.Bot
{
    public static class Setup
    {
        public static IServiceCollection AddBot(this IServiceCollection services, HostBuilderContext context)
        {
            services.AddOptions<BotConfiguration>()
                .Bind(context.Configuration.GetSection(typeof(Setup).Namespace!.ToString().Replace('.', ':')))
                .ValidateDataAnnotations();

            return services
                .AddDiscordGateway(serviceProvider => serviceProvider.GetRequiredService<IOptions<BotConfiguration>>().Value.Token)
                .Configure<DiscordGatewayClientOptions>(options =>
                {
                    options.Intents =
                        GatewayIntents.Guilds
                        | GatewayIntents.GuildMembers
                        | GatewayIntents.GuildBans
                        | GatewayIntents.GuildEmojis
                        | GatewayIntents.GuildIntegrations
                        | GatewayIntents.GuildWebhooks
                        | GatewayIntents.GuildInvites
                        | GatewayIntents.GuildVoiceStates
                        | GatewayIntents.GuildPresences
                        | GatewayIntents.GuildMessages
                        | GatewayIntents.GuildMessageReactions
                        | GatewayIntents.GuildMessageTyping
                        | GatewayIntents.DirectMessages
                        | GatewayIntents.DirectMessageReactions
                        | GatewayIntents.DirectMessageTyping;

                    options.Presence = new UpdatePresence(
                        Status:     ClientStatus.Online,
                        IsAFK:      false,
                        Since:      null,
                        Activities: new[] {
                            new Activity(
                                Name:   "Monitoring the Gateway...",
                                Type:   ActivityType.Custom)
                        });
                })
                .AddSingleton<BotService>()
                .AddHostedService(serviceProvider => serviceProvider.GetRequiredService<BotService>())
                .AddHostedService<BotRestartBehavior>();
        }
    }
}
