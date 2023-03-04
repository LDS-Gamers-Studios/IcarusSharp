using Icarus.Models;

using Microsoft.EntityFrameworkCore;

namespace Icarus
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration Configuration;

        public DbSet<Member> Member { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Flag> Flag { get; set; }
        public DbSet<Filter> Filter { get; set; }
        public DbSet<FilterException> FilterException { get; set; }
        public DbSet<ServerSettingValue> ServerSettingValue { get; set; }

        public DataContext(IConfiguration config)
        {
            Configuration = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) { return; }
            var conn =
                $"Server={Configuration["sql:host"]};" +
                $"Port={Configuration["sql:port"]};" +
                $"Database={Configuration["sql:database"]};" +
                $"Uid={Configuration["sql:username"]};" +
                $"Pwd={Configuration["sql:password"]};";
            optionsBuilder.UseMySql(conn, ServerVersion.AutoDetect(conn));
        }
    }
}
