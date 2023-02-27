using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;

using System.Reflection;

namespace Icarus.Discord
{
    public class DiscordBotService
    {
        public static DiscordBotService Instance;

        public readonly DiscordClient Client;
        readonly ILogger Logger;

        public static IConfiguration Configuration { get; private set; }

        public DiscordBotService(ILogger<DiscordBotService> logger, IConfiguration config)
        {
            Instance = this;
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

            Client.ClientErrored += Client_ClientErrored;
            Client.SocketErrored += Client_SocketErrored;

            var slash = Client.UseSlashCommands(new SlashCommandsConfiguration()
            {
                Services = new ServiceCollection()
                    .AddSingleton(config)
                    .AddSingleton<ILogger>(logger)
                    .AddScoped<DataContext>()
                    .AddLogging(a => a.AddProvider(new LogMessageDiverter<DiscordBotService>(logger)))
                    .BuildServiceProvider()
            });
            slash.SlashCommandErrored += Slash_SlashCommandErrored;
            slash.ContextMenuErrored += Slash_ContextMenuErrored;
            slash.AutocompleteErrored += Slash_AutocompleteErrored;
            slash.RegisterCommands(Assembly.GetExecutingAssembly(), ulong.Parse(config["discord:guild"]));

            Client.UseInteractivity();

            RegisterEvents();

            Client.ConnectAsync().Wait();
        }

        private async Task Client_SocketErrored(DiscordClient sender, DSharpPlus.EventArgs.SocketErrorEventArgs e) =>
            await ErrorHandler(e.Exception, null, null, null);

        private async Task Client_ClientErrored(DiscordClient sender, DSharpPlus.EventArgs.ClientErrorEventArgs e) =>
            await ErrorHandler(e.Exception, null, null, null);

        private async Task Slash_AutocompleteErrored(SlashCommandsExtension sender, AutocompleteErrorEventArgs e) =>
            await ErrorHandler(e.Exception, "Autocomplete - Field: " + e.Context.FocusedOption.Name, e.Context.User.Mention, e.Context.Channel.Mention);

        private async Task Slash_ContextMenuErrored(SlashCommandsExtension sender, ContextMenuErrorEventArgs e)
        {
            await ErrorHandler(e.Exception, e.Context.QualifiedName, e.Context.User.Mention, e.Context.Channel.Mention);
            try
            {
                await e.Context.CreateResponseAsync("I've run into an error. I've let my devs know.", true);
            }
            catch
            {
                await e.Context.EditResponseAsync("I've run into an error. I've let my devs know.");
            }
        }

        private async Task Slash_SlashCommandErrored(SlashCommandsExtension sender, SlashCommandErrorEventArgs e)
        {
            await ErrorHandler(e.Exception, e.Context.QualifiedName, e.Context.User.Mention, e.Context.Channel.Mention);
            try
            {
                await e.Context.CreateResponseAsync("I've run into an error. I've let my devs know.", true);
            }
            catch
            {
                await e.Context.EditResponseAsync("I've run into an error. I've let my devs know.");
            }
        }

        private async Task ErrorHandler(Exception exception, string command, string user, string channel)
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
                .AddField("Message", exception.Message, true);

            if (command is not null)
            {
                embed = embed.AddField("Command Run", command, true);
            }
            if (user is not null)
            {
                embed = embed.AddField("User", user, true);
            }
            if (channel is not null)
            {
                embed = embed.AddField("Channel", channel, true);
            }

            await webhook.ExecuteAsync(new DiscordWebhookBuilder().AddEmbed(embed.Build()));

            Logger.LogError("Command error:\n\n{exceptionMessage}\n\n{exception}", exception.Message, exception.ToString());
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
                foreach (var (m, _) in hooks)
                {
                    Delegate handler =
                         Delegate.CreateDelegate(e.EventHandlerType, m);
                    e.AddEventHandler(Client, handler);
                }
            }

            Logger.LogInformation("Discord events registered.");
        }
    }
}
