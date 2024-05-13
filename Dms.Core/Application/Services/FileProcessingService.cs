using Dms.Core.Application.Common.Interfaces;

namespace Dms.Core.Application.Services
{
    public class FileProcessingService : IFileProcessingService
    {
        private readonly IFileReadingService filessReader;

        public FileProcessingService(IFileReadingService filessReader)
        {
            this.filessReader = filessReader;
        }

        public async Task ProcessFilesAndStoreToDb()
        {
            var files = await filessReader.ToReadFiles();
        }
    }
}
