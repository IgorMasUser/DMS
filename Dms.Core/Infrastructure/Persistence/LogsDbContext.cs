using Dms.Core.Application.Common.Interfaces;
using Dms.Core.Domain.Entities;
using Dms.Core.Infrastructure.Persistence.Base;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Dms.Core.Infrastructure.Persistence
{
    public class LogsDbContext : BaseDbContext, ILogsDbContext
    {
        public LogsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Logger> Loggers { get; set; }

        protected override IEnumerable<Assembly> MigrationsAssemblies => new[] { typeof(LogsDbContext).Assembly };
    }
}
