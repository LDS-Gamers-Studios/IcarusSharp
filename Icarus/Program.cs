using AspNet.Security.OAuth.Discord;

using Icarus.Discord;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using System.Globalization;
using System.Reflection;
using System.Security.Claims;

namespace Icarus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("config.json", optional: false, reloadOnChange: false);

            Extensions.VersionText = builder.Configuration["versionText"];

            // Add services to the container
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = DiscordAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddDiscord(options =>
                {
                    options.ClientId = builder.Configuration["discord:oauth:clientId"];
                    options.ClientSecret = builder.Configuration["discord:oauth:clientSecret"];

                    options.ClaimActions.MapCustomJson("urn:discord:avatar:url", user =>
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "https://cdn.discordapp.com/avatars/{0}/{1}.{2}",
                            user.GetString("id"),
                            user.GetString("avatar"),
                            user.GetString("avatar").StartsWith("a_") ? "gif" : "png"));
                    options.ClaimActions.MapCustomJson(ClaimTypes.Role, user =>
                    {
                        var id = user.GetString("id");

                        var guild = DiscordBotService.Instance.Client.Guilds[ulong.Parse(builder.Configuration["discord:guild"])];
                        var member = guild.Members.FirstOrDefault(m => m.Key.ToString() == id).Value;
                        var manager = member.Permissions.HasFlag(DSharpPlus.Permissions.ManageGuild);

                        return manager ? "Manager" : "";
                    });
                });
            builder.Services.AddAuthorization(options => options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
            builder.Services.AddScoped<DataContext>();
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<DiscordBotService>();
            builder.Services.AddHttpContextAccessor();
            var app = builder.Build();

            if (builder.Configuration["sql:autoMigrate"].ToLower() == "true")
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();
                db.Database.Migrate();
            }
            
            var bot = app.Services.GetService<DiscordBotService>();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/" && context.Request.QueryString.Value == "?logout=1")
                {
                    await context.SignOutAsync();
                    context.Response.Redirect("/");
                    return;
                }

                var id = context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var guild = bot.Client.Guilds[ulong.Parse(builder.Configuration["discord:guild"])];

                if (!guild.Members.ContainsKey(ulong.Parse(id))) { return; }

                await next();
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}
