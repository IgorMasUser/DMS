using System.Collections.Generic;
using Dms.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Dms.Core.Application.Common.Interfaces
{
    public interface ILogsDbContext
    {
        DbSet<Logger> Loggers { get; set; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
