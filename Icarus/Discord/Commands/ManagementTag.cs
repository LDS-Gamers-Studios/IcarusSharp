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
            [SlashRequireUserPermissions(Permissions.ManageGuild, false)]
            public async Task Test(InteractionContext ctx, 
                [Option("name", "The tag name the user will call upon")]string name,
                [Option("content", "Tag text")]string content = null,
                [Option("attachment", "The file to attach. If embed, must be image.")]DiscordAttachment attachment = null,
                [Option("embed", "Post the tag in an embed?")]bool embed = true)
            {
                await ctx.DeferAsync();

                name = Regex.Replace(name.ToLower(), @"\s+", "");

                var mem = IcarusDbContext.IcarusMember(ctx);

                if ((content is null || content.Length == 0) && attachment is null)
                {
                    var e = ctx.IcarusEmbed()
                        .WithColor(DiscordColor.Red)
                        .WithTitle("Failed to create tag.")
                        .WithDescription("The tag must have either content or an attachment.");
                    await ctx.EditResponseAsync(e);
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
            }
        }
    }
}
