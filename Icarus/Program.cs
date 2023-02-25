using Icarus.Discord;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

using System.Diagnostics;

namespace Icarus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("config.json", optional: false, reloadOnChange: false);

            // Add services to the container.
            builder.Services.AddScoped<DataContext>();
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<DiscordBotService>();
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();
                db.Database.Migrate();
            }

            app.Services.GetService<DiscordBotService>();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            //if (Debugger.IsAttached)
            //{
            //    var config = builder.Configuration;
            //    var connString = $"Server={config["sql:host"]};Port={config["sql:port"]};Database={config["sql:database"]};Uid={config["sql:username"]};Pwd={config["sql:password"]};";
            //    var conn = new MySqlConnection(connString);
            //    conn.Open();

            //    var cmd = new MySqlCommand($"select group_concat(TABLE_NAME) from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA='{config["sql:database"]}'", conn);
            //    var reader = cmd.ExecuteReader();
            //    reader.Read();
            //    var tables = reader[0].ToString().Split(',');
            //    reader.Dispose();
            //    cmd.Dispose();

            //    var outText = "";
            //    foreach (var table in tables)
            //    {
            //        cmd = new MySqlCommand($"show create table {table}", conn);
            //        reader = cmd.ExecuteReader();
            //        reader.Read();
            //        outText += reader[1].ToString() + ";\n\n";
            //        reader.Dispose();
            //        cmd.Dispose();
            //    }

            //    File.WriteAllText("setupDatabase.sql", outText);

            //    conn.Close();
            //    conn.Dispose();
            //}

            app.Run();
        }
    }
}