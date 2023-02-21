using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

using Icarus.Models;

using System.Text.RegularExpressions;

namespace Icarus.Discord.Commands
{
    public partial class Management : ApplicationCommandModule
    {
        [SlashCommandGroup("tag", "Tag management")]
        class ManagementTag
        {
            IcarusDbContext IcarusDbContext;

            public ManagementTag(IcarusDbContext db)
            {
                IcarusDbContext = db;
            }

            [SlashCommand("create", "Creates a tag")]
            public async Task Create(InteractionContext ctx,
                [Autocomplete(typeof(TagAutocompleteProvider))][Option("name", "The tag name the user will call upon")]string name,
                [Option("content", "Tag text")]string content = null,
                [Option("attachment", "The file to attach. If embed, must be image.")]DiscordAttachment attachment = null,
                [Option("embed", "Post the tag in an embed?")]bool embed = false)
            {
                await ctx.DeferAsync();

                name = Regex.Replace(name.ToLower(), @"\s+", "");

                var mem = IcarusDbContext.IcarusMember(ctx);

                if ((content is null || content.Length == 0) && attachment is null)
                {
                    var e2 = ctx.IcarusEmbed()
                        .WithColor(DiscordColor.Red)
                        .WithTitle("Failed to create tag.")
                        .WithDescription("The tag must have either content or an attachment.");
                    await ctx.EditResponseAsync(e2);
                    return;
                }

                var foundTag = IcarusDbContext.Tag.FirstOrDefault(t => t.Name == name);
                if (foundTag is not null)
                {
                    if (await ctx.ConfirmAction("This tag already exists.", "Overwrite?\n\nTag name: " + name))
                    {
                        IcarusDbContext.Tag.Remove(foundTag);
                    }
                    else
                    {
                        return;
                    }
                }

                if (content is not null) { content = content.Replace("{nl}", "\n"); }
                IcarusDbContext.Tag.Add(new Tag()
                {
                    CreatedBy = mem,
                    CreatedAt = DateTime.Now,
                    AttachmentURL = attachment?.Url,
                    Content = content,
                    IsEmbed = embed,
                    Name = name,
                });
                IcarusDbContext.SaveChanges();

                var e = ctx.IcarusEmbed()
                    .WithTitle("Tag Created")
                    .WithDescription(content)
                    .AddField("Name", name, true)
                    .AddField("Embed", embed.ToString(), true)
                    .AddField("Attachment", attachment?.Url ?? "None", true);
                await ctx.EditResponseAsync(e);
            }

            [SlashCommand("delete", "Deletes a tag")]
            public async Task Delete(InteractionContext ctx,
                [Autocomplete(typeof(TagAutocompleteProvider))][Option("name", "The tag name the user will call upon")] string name)
            {
                await ctx.DeferAsync();

                name = Regex.Replace(name.ToLower(), @"\s+", "");

                var foundTag = IcarusDbContext.Tag.FirstOrDefault(t => t.Name == name);
                if (foundTag is null)
                {
                    var e2 = ctx.IcarusEmbed()
                        .WithColor(DiscordColor.Red)
                        .WithTitle("Failed To Delete Tag")
                        .WithDescription($"I was unable to find a tag with the name `{name}`.");
                    await ctx.EditResponseAsync(e2);
                    return;
                }

                if (!(await ctx.ConfirmAction("Are you sure?", $"Are you sure that you want to delete the tag `{name}`?")))
                {
                    return;
                }

                IcarusDbContext.Tag.Remove(foundTag);
                IcarusDbContext.SaveChanges();

                var e = ctx.IcarusEmbed()
                    .WithTitle("Tag Deleted")
                    .WithDescription($"I deleted the tag `{name}`.");
                await ctx.EditResponseAsync(e);
            }

            public class TagAutocompleteProvider : IAutocompleteProvider
            {
                public async Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx)
                {
                    return 
                        (new IcarusDbContext(DiscordBotService.Configuration)).Tag
                        .Where(t => t.Name.Contains(ctx.OptionValue.ToString()))
                        .Select(t => new DiscordAutoCompleteChoice(t.Name, t.Name))
                        .ToList();
                }
            }
        }
    }
}
