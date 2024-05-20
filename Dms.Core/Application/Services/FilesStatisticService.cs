using Dms.Core.Application.Common.Interfaces;
using Dms.Core.Application.Common.UIModels;
using Microsoft.EntityFrameworkCore;

namespace Dms.Core.Application.Services
{
    public class FilesStatisticService: IFilesStatisticService
    {
        private readonly IDmsDbContext dbContext;

        public FilesStatisticService(IDmsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<FilesPerDay>> GetFilesPerDayAsync()
        {
            var statisticResult = await dbContext.FilesData
                .GroupBy(fd => fd.ReadAt.Value.Date)
                .Select(g => new FilesPerDay
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            return statisticResult;
        }
    }
}
