using Dms.Core.Domain.Entities;

namespace Dms.Core.Application.Common.Interfaces
{
    public interface IFileReadingService
    {
        Task<List<FileData>> ToReadFiles();
    }
}