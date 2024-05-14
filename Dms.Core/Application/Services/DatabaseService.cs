using Dms.Core.Application.Common.Interfaces;
using Dms.Core.Domain.Entities;

namespace Dms.Core.Application.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IDmsDbContext dbContext;

        public DatabaseService(IDmsDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task StoreFilesAsync(List<FileData> files,CancellationToken cancellationToken = default)
        {
            if (files == null || files.Count == 0)
            {
                throw new ArgumentException("The files list cannot be null or empty", nameof(files));
            }

            foreach (var file in files)
            {
                dbContext.FilesData.Add(file);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
