using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;

using Emzi0767.Utilities;

using Microsoft.Extensions.Logging;

using System.Reflection;

namespace Icarus.Discord
{
    public class DiscordBotService
    {
        DiscordClient Client;
        public static IConfiguration Configuration;
        ILogger Logger;

        public DiscordBotService(ILogger<DiscordBotService> logger, IConfiguration config)
        {
            Logger = logger;

            Configuration = config;
            logger.LogInformation("Booting DSharpPlus... - ");

            Client = new DiscordClient(new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                LargeThreshold = 10000,
                LoggerFactory = new LoggerFactory(new List<ILoggerProvider> { new LogMessageDiverter<DiscordBotService>(logger) }),
                MessageCacheSize = 4096,
                Token = config["discord:token"],
            });

            var slash = Client.UseSlashCommands(new SlashCommandsConfiguration()
            {
                Services = new ServiceCollection()
                    .AddSingleton(config)
                    .AddSingleton<ILogger>(logger)
                    .AddScoped<IcarusDbContext>()
                    .AddLogging(a => a.AddProvider(new LogMessageDiverter<DiscordBotService>(logger)))
                    .BuildServiceProvider()
            });

            Client.UseInteractivity();

            slash.SlashCommandErrored += Slash_SlashCommandErrored;
            slash.ContextMenuErrored += Slash_ContextMenuErrored;
            slash.AutocompleteErrored += Slash_AutocompleteErrored;

            slash.RegisterCommands(Assembly.GetExecutingAssembly(), ulong.Parse(config["discord:guild"]));


            RegisterEvents();

            Client.ConnectAsync().Wait();
        }

        private async Task Slash_AutocompleteErrored(SlashCommandsExtension sender, AutocompleteErrorEventArgs e)
        {
            await InteractionError(e.Exception, "Autocomplete", e.Context.User.Mention, e.Context.Channel.Mention);
        }

        private async Task Slash_ContextMenuErrored(SlashCommandsExtension sender, ContextMenuErrorEventArgs e)
        {
            await InteractionError(e.Exception, e.Context.QualifiedName, e.Context.User.Mention, e.Context.Channel.Mention);
            await e.Context.CreateResponseAsync("I've run into an error. I've let my devs know.", true);
        }

        private async Task Slash_SlashCommandErrored(SlashCommandsExtension sender, SlashCommandErrorEventArgs e)
        {
            await InteractionError(e.Exception, e.Context.QualifiedName, e.Context.User.Mention, e.Context.Channel.Mention);
            await e.Context.CreateResponseAsync("I've run into an error. I've let my devs know.", true);
        }

        private async Task InteractionError(Exception exception, string command, string user, string channel)
        {
            var client = new DiscordWebhookClient();
            var webhook = await client.AddWebhookAsync(new Uri(Configuration["discord:webhook"]));

            var desc = exception.ToString();
            desc = desc.Length > 4080 ? desc[..4080] : desc;
            desc = $"```cs\n{desc}\n```";

            var embed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Red)
                .WithTitle("An error occured!")
                .WithDescription(desc)
                .AddField("Exception", exception.GetType().Name, true)
                .AddField("Message", exception.Message, true)
                .AddField("Command Run", command, true)
                .AddField("User", user, true)
                .AddField("Channel", channel, true)
                .Build();

            await webhook.ExecuteAsync(new DiscordWebhookBuilder().AddEmbed(embed));

            Logger.LogError($"Command error:\n\n{exception.Message}\n\n{exception}");
        }

        void RegisterEvents()
        {
            var eventAtt = Assembly.GetExecutingAssembly().DefinedTypes
                .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public))
                .Where(m => m.GetCustomAttribute<DiscordEventHandler>() is not null)
                .Select(m => (m, m.GetCustomAttribute<DiscordEventHandler>().Event))
                .ToList();

            var t = typeof(DiscordClient);
            foreach (var e in t.GetEvents())
            {
                var hooks = eventAtt.Where(e2 => e2.Event == e.Name).ToList();
                foreach (var hook in hooks)
                {
                    Delegate handler =
                         Delegate.CreateDelegate(e.EventHandlerType,
                                                 hook.m);
                    e.AddEventHandler(Client, handler);
                }
            }

            Logger.LogInformation("Discord events registered.");
        }
    }
}
