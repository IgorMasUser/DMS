using Dms.Core.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using MudBlazor;

namespace Dms.Api.Pages.DocumentsData
{
    public partial class DocumentDetails
    {
        [Inject] public ILogger<DocumentDetails> logger { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        private void NavigateBack()
        {
            if (!string.IsNullOrEmpty(Source))
            {
                NavigationManager.NavigateTo(Source);
            }
            else
            {
                NavigationManager.NavigateTo("/");
            }
        }

        async Task DownloadFile(string documentNumber)
        {
            if (string.IsNullOrWhiteSpace(documentNumber))
            {
                logger.LogError("Document number is null or empty.");
                SnackbarComponent.Add("Invalid document number.", Severity.Error);
                return;
            }

            try
            {
                var file = await GetExistingFile(documentNumber);

                if (file != null)
                {
                    await DownloadFileFromStream(file);
                }
            }
            catch (FileNotFoundException ex)
            {
                logger.LogError(ex.Message);
                SnackbarComponent.Add("File not found.", Severity.Error);
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to download file: {ex.Message}");
                SnackbarComponent.Add("Failed to download.", Severity.Error);
            }
        }

        private async Task<FileData> GetExistingFile(string documentNumber)
        {
            var file = await dbContext.FilesData.FirstOrDefaultAsync(dn => dn.DocumentNumber.Equals(documentNumber));
            if (file == null)
            {
                throw new FileNotFoundException($"No file found with DocumentNumber: {documentNumber}");
            }

            return file;
        }

        private async Task DownloadFileFromStream(FileData file)
        {
            await using var ms = new MemoryStream(file.Data);
            using var streamRef = new DotNetStreamReference(stream: ms);

            await JS.InvokeVoidAsync("downloadFileFromStream", file.FileName, streamRef);
            SnackbarComponent.Add("File downloaded.", Severity.Info);
        }

    }
}
