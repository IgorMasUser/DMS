using Dms.Core.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Dms.LocalFilesReader
{
    public class FilesReaderJob : IJob
    {

        private readonly ILogger<FilesReaderJob> logger;
        private readonly IFileProcessingService fileProcessingService;

        public FilesReaderJob(ILoggerFactory loggerFactory, IFileProcessingService fileProcessingService)
        {
            logger = loggerFactory.CreateLogger<FilesReaderJob>();
            this.fileProcessingService = fileProcessingService ?? throw new ArgumentNullException(nameof(fileProcessingService));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                logger.LogInformation("FilesReaderJob started");
                await fileProcessingService.ProcessFilesAndStoreToDb();
                logger.LogInformation("Files reading completed");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in FilesReaderJob: {ex.Message}");
                await Task.FromException(ex);
            }

            await Task.CompletedTask;
        }
    }
}
