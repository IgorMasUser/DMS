using Dms.Core.Application.Common.Interfaces;
using Dms.Core.Domain.Entities;
using Dms.Core.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Dms.Core.Application.Services
{
    public class FileReadingService : IFileReadingService
    {
        private readonly IOptions<FilesOptions> filesOptions;

        public FileReadingService(IOptions<FilesOptions> filesOptions)
        {
            this.filesOptions = filesOptions ?? throw new ArgumentNullException(nameof(filesOptions));
        }

        public async Task<List<FileData>> ToReadFiles()
        {
            string directoryPath = filesOptions.Value.SourcePath;

            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"The directory {directoryPath} was not found.");
            }

            string[] files = Directory.GetFiles(directoryPath);
            List<FileData> documents = new List<FileData>();

            foreach (string filePath in files)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                FileData file = new FileData
                {
                    FileName = fileInfo.Name,
                    FileSize = fileInfo.Length,
                    ContentType = GetContentType(filePath),
                    Data = await File.ReadAllBytesAsync(filePath)
                };
                documents.Add(file);
            }

            return documents;
        }

        private string GetContentType(string filePath)
        {
            return MimeMapping.MimeUtility.GetMimeMapping(filePath);
        }

    }
}
