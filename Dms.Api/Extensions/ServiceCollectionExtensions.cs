using Dms.Core.Application.Common.Interfaces;
using Dms.Core.Infrastructure.Configuration;
using Dms.Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Dms.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlServerDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString(Constants.DEFAULTCONNECTION_KEY);

            if (string.IsNullOrWhiteSpace(connString)) throw new ArgumentNullException(nameof(connString));

            services.AddDbContextFactory<LogsDbContext>(options =>
            {
                options.UseSqlServer(connString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                });
            });

            services.AddScoped<ILogsDbContext>(provider => provider.GetRequiredService<IDbContextFactory<LogsDbContext>>().CreateDbContext());

            return services;
        }
    }
}
