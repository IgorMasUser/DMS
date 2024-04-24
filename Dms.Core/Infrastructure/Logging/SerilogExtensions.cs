using Microsoft.AspNetCore.Builder;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Extensions.Hosting;

namespace Dms.Core.Infrastructure.Logging
{
    public static class SerilogExtensions
    {
        public static void AddSerilog(
    this IHostBuilder builder,
    bool useSql = true,
    string? connStringKey = default,
    string? tableSchema = default,
    bool writeToProviders = true)
        {
            builder.UseSerilog((context, configuration) =>
            {
            configuration.ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithUtcTime()
                .ApplyConfigPreferences(context.Configuration, useSql, connStringKey, tableSchema)
                , writeToProviders: writeToProviders);
        });

    private static void ApplyConfigPreferences(this LoggerConfiguration serilogConfig, IConfiguration configuration, bool useSql, string? connStringKey = default, string? tableSchema = default)
        {
            EnrichWithClientInfo(serilogConfig, configuration);

            if (useSql) WriteToDatabase(serilogConfig, configuration, connStringKey, tableSchema);
        }

        private static void WriteToDatabase(LoggerConfiguration serilogConfig, IConfiguration configuration, string? connStringKey = default, string? tableSchema = default)
        {
            var connString = string.IsNullOrWhiteSpace(connStringKey)
                        ? configuration.GetConnectionString(Constants.DEFAULTCONNECTION_KEY)
                        : configuration.GetValue<string>(connStringKey);

            Guard.StringNotNullOrWhiteSpace(connString);

            WriteToSqlServer(serilogConfig, connString, tableSchema);
        }

        private static void EnrichWithClientInfo(LoggerConfiguration serilogConfig, IConfiguration configuration)
        {
            var privacySettings
                = configuration.GetSection(ConfigurationUtils.GetPathConfigurationProperty<CommonConfiguration>(c => c.PrivacySettings)).Get<PrivacySettingsConfigurtation>();

            if (privacySettings == null) return;
            if (privacySettings.LogClientIpAddresses) serilogConfig.Enrich.WithClientIp();
            if (privacySettings.LogClientAgents) serilogConfig.Enrich.WithRequestHeader("User-Agent");
        }

        private static void WriteToSqlServer(LoggerConfiguration serilogConfig, string connectionString, string? loggersSchemaName = default)
        {
            Guard.StringNotNullOrWhiteSpace(connectionString);

            MSSqlServerSinkOptions sinkOpts = new()
            {
                TableName = Constants.LOGGERS_DEFAULT_TABLENAME,
                SchemaName = loggersSchemaName ?? Constants.LOGGERS_DEFAULT_TABLESCHEMA,
                AutoCreateSqlDatabase = false,
                AutoCreateSqlTable = false,
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
                    ColumnName = "ClientIP", PropertyName = "ClientIp", DataType = SqlDbType.NVarChar, DataLength = 64
                },
                new()
                {
                    ColumnName = "UserName", PropertyName = "UserName", DataType = SqlDbType.NVarChar, DataLength = 64
                },
                new()
                {
                    ColumnName = "ClientAgent", PropertyName = "UserAgent", DataType = SqlDbType.NVarChar,
                    DataLength = -1
                },
                new()
                {
                    ColumnName = "CorrelationId", PropertyName = "RequestId", DataType = SqlDbType.NVarChar,
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

        internal class UtcTimestampEnricher : ILogEventEnricher
        {
            public void Enrich(LogEvent logEvent, ILogEventPropertyFactory pf)
            {
                logEvent.AddOrUpdateProperty(pf.CreateProperty("TimeStamp", logEvent.Timestamp.UtcDateTime));
            }
        }

    }

}
