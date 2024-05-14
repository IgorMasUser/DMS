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

            var fileAccounter = new FileAccounter
            {
                Name = "FilesTracker",
                files = files
            };

            dbContext.FileAccounters.Add(fileAccounter);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
