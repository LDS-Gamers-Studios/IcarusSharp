using Icarus.Models;

using Microsoft.EntityFrameworkCore;

namespace Icarus
{
    public class DataContext : DbContext
    {
        IConfiguration configuration;
        public DbSet<Member> Member { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Flag> Flag { get; set; }
        public DbSet<Filter> Filter { get; set; }
        public DbSet<FilterException> FilterException { get; set; }

        public DataContext(IConfiguration config)
        {
            configuration = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) { return; }
            var conn = $"Server={configuration["sql:host"]};Port={configuration["sql:port"]};Database={configuration["sql:database"]};Uid={configuration["sql:username"]};Pwd={configuration["sql:password"]};";
            optionsBuilder.UseMySql(conn, ServerVersion.AutoDetect(conn));
        }
    }
}
