using Dms.Core.Application.Common.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace Dms.Api.Pages.DocumentsData
{
    public partial class DocumentDetails
    {
        [Inject] public ILogger<DocumentDetails> logger { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        async Task DownloadFile(string documentNumber)
        {
            try
            {
                var file = await dbContext.FilesData.FirstOrDefaultAsync(dn => dn.DocumentNumber.Equals(documentNumber));

                if (file == null) throw new FileNotFoundException($"No file found with DocumentNumber: {documentNumber}");

                using (MemoryStream ms = new MemoryStream(file.Data))
                {
                    using var streamRef = new DotNetStreamReference(stream: ms);

                    await JS.InvokeVoidAsync("downloadFileFromStream", file.FileName, streamRef);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"File not found: {ex.Message}");
            }

        }

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
    }
}
