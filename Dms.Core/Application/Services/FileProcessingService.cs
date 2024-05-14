using Dms.Core.Application.Common.Interfaces;

namespace Dms.Core.Application.Services
{
    public class FileProcessingService : IFileProcessingService
    {
        private readonly IFileReadingService filesReader;
        private readonly IDatabaseService databaseService;

        public FileProcessingService(IFileReadingService filesReader, IDatabaseService databaseService)
        {
            this.filesReader = filesReader ?? throw new ArgumentNullException(nameof(filesReader));
            this.databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        }

        public async Task ProcessFilesAndStoreToDb()
        {
            var files = await filesReader.ToReadFiles();
            await databaseService.StoreFilesAsync(files);
        }
    }
}
