using Dms.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Dms.Core.Application.Common.Interfaces
{
    public interface IDmsDbContext
    {
        DbSet<Logger> Loggers { get; set; }
        DbSet<FileData> FilesData { get; set; }
        DbSet<FileAccounter> FileAccounters { get; set; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
