using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Quartz;
using Dms.LocalFilesReader;
using Dms.Core.Application.Common.Interfaces;
using Dms.Core.Application.Services;
using Dms.Core.Infrastructure.Configuration;
using Dms.Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration(config =>
{
    var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\.."));
    config.SetBasePath(basePath);
    config.AddJsonFile("appsettings.json", optional: false);
});

string cron;

builder.ConfigureServices((hostContext, services) =>
{
    cron = hostContext.Configuration.GetSection("FilesReader:Cron").Value!;
    services.Configure<FilesOptions>(hostContext.Configuration.GetSection("FilesSettings"));
    ConfigureQuartz(services);

    services.AddDbContext<DmsDbContext>(options =>
        options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<IDmsDbContext>(provider => provider.GetService<DmsDbContext>());

    services.AddSingleton<IFileReadingService, FileReadingService>();
    services.AddTransient<IFileProcessingService, FileProcessingService>();
    services.AddSingleton<IDatabaseService, DatabaseService>();
});

var host = builder.Build();
await host.RunAsync();

void ConfigureQuartz(IServiceCollection services)
{
    services.AddQuartz(q =>
    {
        var jobKey = new JobKey("FilesReaderJob");
        q.AddJob<FilesReaderJob>(opts => opts.WithIdentity(jobKey));
        q.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity("Job-trigger")
            .WithCronSchedule(cron));
    });

    services.AddQuartzHostedService(q =>
        q.WaitForJobsToComplete = true);
}
