using Dms.Core.Application.Common.UIModels;

namespace Dms.Core.Application.Common.Interfaces
{
    public interface IFilesStatisticService
    {
        Task<List<FilesPerDay>> GetFilesPerDayAsync();
    }
}
