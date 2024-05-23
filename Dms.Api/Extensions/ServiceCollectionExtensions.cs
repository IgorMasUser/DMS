using Dms.Core.Application.Common.Interfaces;
using Dms.Core.Infrastructure.Configuration;
using Dms.Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Dms.Api.Services.Navigation;
using Dms.Core.Application.Services;
using MudBlazor.Services;
using MudBlazor;

namespace Dms.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlServerDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString(Constants.DEFAULTCONNECTION_KEY);

            if (string.IsNullOrWhiteSpace(connString)) throw new ArgumentNullException(nameof(connString));

            services.AddDbContextFactory<DmsDbContext>(options =>
            {
                options.UseSqlServer(connString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                });
            });

            services.AddScoped<IDmsDbContext>(provider => provider.GetRequiredService<IDbContextFactory<DmsDbContext>>().CreateDbContext());

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<IMenuService, MenuService>();
            services.AddTransient<IFilesStatisticService, FilesStatisticService>();
            services.AddTransient<IDocumentHistoryService, DocumentHistoryService>();
            services.AddHttpContextAccessor();
            services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
