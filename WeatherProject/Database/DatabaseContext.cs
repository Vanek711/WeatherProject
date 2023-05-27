using Microsoft.EntityFrameworkCore;
using WeatherProject.Models.WeatherData;

namespace WeatherProject.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> opts) : base(opts)
        {

        }
        public DbSet<WeatherTypes> Weather { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherTypes>(entity =>
            {
            });
        }
    }
}
