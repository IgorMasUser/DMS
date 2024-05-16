using Dms.Core.Application.Common.Interfaces;
using Dms.Core.Application.Common.UIModels.Enums;

namespace Dms.Core.Application.Services
{
    public class FileProcessingService : IFileProcessingService
    {
        private readonly IFileReadingService filesReader;
        private readonly IDatabaseService databaseService;
        private readonly IDocumentHistoryService documentHistoryService;

        public FileProcessingService(IFileReadingService filesReader, IDatabaseService databaseService, IDocumentHistoryService documentHistoryService)
        {
            this.filesReader = filesReader ?? throw new ArgumentNullException(nameof(filesReader));
            this.databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
            this.documentHistoryService = documentHistoryService ?? throw new ArgumentNullException(nameof(documentHistoryService));
        }

        public async Task ProcessFilesAndStoreToDb()
        {
            var files = await filesReader.ToReadFiles();
            await databaseService.StoreFilesAsync(files);

            foreach (var file in files)
            {
                string comment = $"File with document number {file.DocumentNumber} created";
                await documentHistoryService.WriteHistoryAsync(file.Id, nameof(DocumentHistoryEvent.Created), comment, default);
            }
        }
    }
}
