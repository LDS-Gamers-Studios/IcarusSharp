using Icarus.Models;

using Microsoft.EntityFrameworkCore;

namespace Icarus
{
    public class IcarusDbContext : DbContext
    {
        IConfiguration configuration;
        public DbSet<Member> Member { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Flag> Flag { get; set; }

        public IcarusDbContext(IConfiguration config)
        {
            configuration = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) { return; }
            var conn = $"Server={configuration["sql:host"]};Port={configuration["sql:port"]};Database={configuration["sql:database"]};Uid={configuration["sql:username"]};Pwd={configuration["sql:password"]};";
            optionsBuilder.UseMySQL(conn);
        }
    }
}
