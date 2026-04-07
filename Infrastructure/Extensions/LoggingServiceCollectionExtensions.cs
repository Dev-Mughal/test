using Common.Logging;
using Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Extensions
{
    public static class LoggingServiceCollectionExtensions
    {
        public static IServiceCollection AddPostgresAppLogging(this IServiceCollection services, IConfiguration configuration)
        {
            var loggingConnectionString = configuration.GetConnectionString("LoggingConnection") 
                ?? throw new InvalidOperationException("A logging connection string is required.");

            services.Configure<PostgresLoggingOptions>(configuration.GetSection(PostgresLoggingOptions.SectionName));

            services.AddDbContextFactory<LoggingDbContext>(options =>
            {
                options.UseNpgsql(loggingConnectionString, npgsql => npgsql.EnableRetryOnFailure());
            });

            services.AddSingleton<PostgresAppLogger>();
            services.AddSingleton<IAppLogger>(provider => provider.GetRequiredService<PostgresAppLogger>());
            services.AddSingleton<ILoggerProvider, PostgresLoggerProvider>();
            services.AddHostedService(provider => provider.GetRequiredService<PostgresAppLogger>());

            return services;
        }
    }
}
