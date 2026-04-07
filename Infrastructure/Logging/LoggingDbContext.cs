using Infrastructure.Logging.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Logging
{
    public class LoggingDbContext(DbContextOptions<LoggingDbContext> options) : DbContext(options)
    {
        public DbSet<AppLogEntry> AppLogs => Set<AppLogEntry>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AppLogEntryConfiguration());
        }
    }
}
