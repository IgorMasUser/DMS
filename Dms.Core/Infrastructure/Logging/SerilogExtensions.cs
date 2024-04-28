using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Extensions.Hosting;
using Serilog.Core;
using Constants = Dms.Core.Infrastructure.Configuration.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace Dms.Core.Infrastructure.Logging
{

    public static class SerilogExtensions
    {
        public static void AddSerilog(this IHostBuilder builder, bool writeToProviders = true)
        {
            builder.UseSerilog((context, serviceProvider, configuration) =>
            {
                var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
                if (httpContextAccessor is null) throw new ArgumentNullException(nameof(httpContextAccessor));

                configuration.ReadFrom.Configuration(context.Configuration)
                             .Enrich.FromLogContext()
                             .Enrich.WithUtcTime()
                             .Enrich.WithCorrelationId(httpContextAccessor)
                             .WriteToDatabase(context.Configuration);
            }, writeToProviders: writeToProviders);
        }

        private static void WriteToDatabase(this LoggerConfiguration serilogConfig, IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString(Constants.DEFAULTCONNECTION_KEY);

            if (string.IsNullOrWhiteSpace(connString)) throw new ArgumentNullException(nameof(connString));

            WriteToSqlServer(serilogConfig, connString);
        }

        private static void WriteToSqlServer(LoggerConfiguration serilogConfig, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            MSSqlServerSinkOptions sinkOpts = new()
            {
                TableName = Constants.LOGGERS_DEFAULT_TABLENAME,
                SchemaName = Constants.LOGGERS_DEFAULT_TABLESCHEMA,
                AutoCreateSqlDatabase = false,
                AutoCreateSqlTable = true,
                BatchPostingLimit = 100,
                BatchPeriod = new TimeSpan(0, 0, 20)
            };

            ColumnOptions columnOpts = new()
            {
                Store = new Collection<StandardColumn>
            {
                StandardColumn.Id,
                StandardColumn.TimeStamp,
                StandardColumn.Level,
                StandardColumn.LogEvent,
                StandardColumn.Exception,
                StandardColumn.Message,
                StandardColumn.MessageTemplate,
                StandardColumn.Properties
            },
                AdditionalColumns = new Collection<SqlColumn>
            {
                new()
                {
                    ColumnName = "CorrelationId", PropertyName = "CorrelationId", DataType = SqlDbType.NVarChar,
                    DataLength = 64
                }
            },
                TimeStamp = { ConvertToUtc = true, ColumnName = "TimeStamp" },
                LogEvent = { DataLength = 2048 }
            };
            columnOpts.PrimaryKey = columnOpts.Id;
            columnOpts.TimeStamp.NonClusteredIndex = true;

            serilogConfig.WriteTo.Async(wt => wt.MSSqlServer(
                connectionString,
                sinkOpts,
                columnOptions: columnOpts
            ));
        }

        public static LoggerConfiguration WithUtcTime(this LoggerEnrichmentConfiguration enrichmentConfiguration)
        {
            return enrichmentConfiguration.With<UtcTimestampEnricher>();
        }

        public static LoggerConfiguration WithCorrelationId(this LoggerEnrichmentConfiguration enrichmentConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            return enrichmentConfiguration.With(new CorrelationIdEnricher(httpContextAccessor));
        }
    }


    internal class UtcTimestampEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory pf)
        {
            logEvent.AddOrUpdateProperty(pf.CreateProperty("TimeStamp", logEvent.Timestamp.UtcDateTime));
        }
    }

    internal class CorrelationIdEnricher : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CorrelationIdEnricher(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var correlationId = _httpContextAccessor.HttpContext?.TraceIdentifier ?? "N/A";
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("CorrelationId", correlationId));
        }
    }
}
