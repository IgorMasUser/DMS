using Dms.Core.Application.Common.Interfaces;
using Dms.Core.Domain.Entities;
using Dms.Core.Infrastructure.Persistence.Base;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Dms.Core.Infrastructure.Persistence
{
    public class DmsDbContext : BaseDbContext, IDmsDbContext
    {
        public DmsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Logger> Loggers { get; set; }
        public DbSet<FileData> FilesData { get; set; }
        public DbSet<FileAccounter> FileAccounters { get; set; }
        public DbSet<DocumentHistory> DocumentHistory { get; set; }

        protected override IEnumerable<Assembly> MigrationsAssemblies => new[] { typeof(DmsDbContext).Assembly };
    }
}
