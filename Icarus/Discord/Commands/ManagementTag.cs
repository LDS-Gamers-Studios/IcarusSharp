using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using Icarus.Models;

using System.Text.RegularExpressions;

namespace Icarus.Discord.Commands
{
    public partial class Management
    {
        [SlashCommandGroup("tag", "Tag management")]
        class ManagementTag
        {
            readonly DataContext DataContext;

            public ManagementTag(DataContext db)
            {
                DataContext = db;
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

                if ((content is null || content.Length == 0) && attachment is null)
                {
                    await ctx.Error("Failed To Create Tag", "The tag must have either content or an attachment.");
                    return;
                }

                var existingTag = DataContext.Tag.FirstOrDefault(t => t.Name == name);
                if (existingTag is not null)
                {
                    if (!await ctx.ConfirmAction("This tag already exists.", "Overwrite?\n\nTag name: " + name))
                    {
                        return;
                    }
                    DataContext.Tag.Remove(existingTag);
                }

                if (content is not null) { content = content.Replace("{nl}", "\n"); }
                DataContext.Tag.Add(new Tag()
                {
                    AttachmentURL = attachment?.Url,
                    Content = content,
                    IsEmbed = embed,
                    Name = name,
                });
                DataContext.SaveChanges();

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

                var existingTag = DataContext.Tag.FirstOrDefault(t => t.Name == name);
                if (existingTag is null)
                {
                    await ctx.Error("Failed To Delete Tag", $"I was unable to find a tag with the name `{name}`.");
                    return;
                }

                if (!(await ctx.ConfirmAction("Are you sure?", $"Are you sure that you want to delete the tag `{name}`?")))
                {
                    return;
                }

                DataContext.Tag.Remove(existingTag);
                DataContext.SaveChanges();

                var e = ctx.IcarusEmbed()
                    .WithTitle("Tag Deleted")
                    .WithDescription($"I deleted the tag `{name}`.");
                await ctx.EditResponseAsync(e);
            }

            public class TagAutocompleteProvider : IAutocompleteProvider
            {
                public Task<IEnumerable<DiscordAutoCompleteChoice>> Provider(AutocompleteContext ctx) =>
                    Task.FromResult(
                        new DataContext(DiscordBotService.Configuration).Tag
                        .Where(t => t.Name.Contains(ctx.OptionValue.ToString()))
                        .Select(t => new DiscordAutoCompleteChoice(t.Name, t.Name))
                        .AsEnumerable()
                    );
            }
        }
    }
}
