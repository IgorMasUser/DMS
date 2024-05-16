using Dms.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Dms.Core.Infrastructure.Persistence.Base
{
    public abstract class BaseDbContext : DbContext
    {
        protected BaseDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected abstract IEnumerable<Assembly> MigrationsAssemblies { get; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var effectedRecordCount = await base.SaveChangesAsync(cancellationToken);
            return effectedRecordCount;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FileAccounter>()
            .HasMany(fa => fa.files)
            .WithOne(fd => fd.FileAccounter)
            .HasForeignKey(fd => fd.FileAccounterId);

            modelBuilder.Entity<DocumentHistory>()
             .HasOne(dh => dh.Document)
             .WithMany(fd => fd.DocumentHistory)
             .HasForeignKey(dh => dh.DocumentId);

            if (MigrationsAssemblies == null || !MigrationsAssemblies.Any())
            {
                return;
            }

            foreach (var assembly in MigrationsAssemblies)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            }
        }
    }
}
