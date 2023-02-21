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
        [SlashCommandGroup("filter", "Adjusts the moderation filter")]
        class ManagementFilter
        {
            IcarusDbContext IcarusDbContext;

            public ManagementFilter(IcarusDbContext db)
            {
                IcarusDbContext = db;
            }

            [SlashCommand("add", "Add a filter entry")]
            public async Task Add(InteractionContext ctx, 
                [Option("text", "The text to filter")]string filterText,
                [Option("type", "The type of filter entry")]FilterType type)
            {
                await ctx.DeferAsync(true);

                filterText = filterText.ToLower();

                var filter = new Filter()
                {
                    FilterText = filterText,
                    Type = type,
                    AddedBy = IcarusDbContext.IcarusMember(ctx),
                    AddTime = DateTime.Now,
                    FilterTextConverted = Utility.FilterConvert(filterText),
                };

                var existingFilter = IcarusDbContext.Filter.FirstOrDefault(f => f.FilterText == filterText || f.FilterTextConverted == filter.FilterTextConverted);

                var addedUpdated = "Added";
                if (existingFilter is not null)
                {
                    var exCount = IcarusDbContext.Entry(existingFilter).Collection(f => f.FilterExceptions).Query().Count();

                    if (existingFilter.Type == type)
                    {
                        await ctx.Error("Failed To Add To Filter", "That text is already in the filter with that type.", "Exceptions", exCount.ToString());
                        return;
                    }
                    else
                    {
                        if (!await ctx.ConfirmAction("Change Filter Type", $"Are you sure you'd like to change the filter type from `{existingFilter.Type}` to {type} for text ||{filterText}||?"))
                        {
                            return;
                        }

                        IcarusDbContext.Remove(existingFilter);
                        addedUpdated = "Updated";
                    }
                }

                IcarusDbContext.Add(filter);
                IcarusDbContext.SaveChanges();

                var e = ctx.IcarusEmbed()
                    .WithTitle("Added To Filter")
                    .WithDescription($"{addedUpdated} ||{filterText}|| as {type}.");
                await ctx.EditResponseAsync(e);
                return;
            }

            [SlashCommand("remove", "Remove a filter entry")]
            public async Task Remove(InteractionContext ctx,
                [Option("text", "The text to filter")] string filterText)
            {
                await ctx.DeferAsync(true);

                filterText = filterText.ToLower();
                var converted = Utility.FilterConvert(filterText);
                var existingFilter = IcarusDbContext.Filter.FirstOrDefault(f => f.FilterText == filterText || f.FilterTextConverted == converted);

                if (existingFilter is null)
                {
                    await ctx.Error("Failed To Remove From Filter", "I was unable to find that filter text already set.");
                    return;
                }

                IcarusDbContext.Entry(existingFilter).Collection(f => f.FilterExceptions).Load();
                var exCount = existingFilter.FilterExceptions.Count;

                if (!await ctx.ConfirmAction("Remove From Filter", $"Are you sure you'd like to remove ||{filterText}|| ({existingFilter.Type}) from the filter? It has {exCount} exception{(exCount != 1 ? "s" : "")}."))
                {
                    return;
                }

                IcarusDbContext.FilterException.RemoveRange(existingFilter.FilterExceptions.ToArray());
                IcarusDbContext.Filter.Remove(existingFilter);
                IcarusDbContext.SaveChanges();

                var e = ctx.IcarusEmbed()
                    .WithTitle("Removed Filter")
                    .WithDescription($"Removed ||{filterText}||.");
                await ctx.EditResponseAsync(e);
                return;
            }
        }
    }
}
