using Dms.Core.Domain.Entities;

namespace Dms.Core.Application.Common.Interfaces
{
    public interface IDatabaseService
    {
        Task StoreFilesAsync(List<FileData> files, CancellationToken cancellationToken = default);
    }
}