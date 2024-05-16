using Dms.Core.Application.Common.Interfaces;
using Dms.Core.Application.Common.UIModels.Enums;
using Dms.Core.Domain.Entities;
using Dms.Core.Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dms.Core.Application.Services
{
    public class FileReadingService : IFileReadingService
    {
        private readonly IOptions<FilesOptions> filesOptions;
        private readonly ILogger<FileReadingService> logger;
        private readonly IDocumentNumberService numberService;

        public FileReadingService(IOptions<FilesOptions> filesOptions,
            ILogger<FileReadingService> logger,
            IDocumentNumberService numberService)
        {
            this.filesOptions = filesOptions ?? throw new ArgumentNullException(nameof(filesOptions));
            this.logger = logger;
            this.numberService = numberService ?? throw new ArgumentNullException(nameof(numberService));
        }

        public async Task<List<FileData>> ToReadFiles()
        {
            var documents = new List<FileData>();

            try
            {
                string directoryPath = filesOptions.Value.SourcePath;

                if (!Directory.Exists(directoryPath))
                {
                    throw new DirectoryNotFoundException($"The directory {directoryPath} was not found.");
                }

                string[] files = Directory.GetFiles(directoryPath);

                foreach (string filePath in files)
                {
                    var generatedDocNumber = await numberService.GenerateNextAsync();
                    if (generatedDocNumber is null)
                    {
                        throw new InvalidOperationException("Generated document number is null.");
                    }

                    FileInfo fileInfo = new FileInfo(filePath);
                    FileData file = new FileData
                    {
                        FileName = fileInfo.Name,
                        FileSize = fileInfo.Length,
                        ContentType = GetContentType(filePath),
                        Data = await File.ReadAllBytesAsync(filePath),
                        DocumentNumber = generatedDocNumber
                    };
                    documents.Add(file);
                }

                return documents;
            }
            catch (DirectoryNotFoundException dirEx)
            {
                logger.LogError(dirEx, $"The directory was not found: {dirEx.Message}");
                return documents;
            }
            catch (IOException ioEx)
            {
                logger.LogError(ioEx, $"IO error occurred: {ioEx.Message}");
                return documents;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while creating metadata: {ex.Message}");
                return documents;
            }
        }

        private string GetContentType(string filePath)
        {
            return MimeMapping.MimeUtility.GetMimeMapping(filePath);
        }

    }
}
