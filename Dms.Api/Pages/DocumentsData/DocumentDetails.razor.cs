using AutoMapper;
using Dms.Core.Application.Common.DTOs;
using Dms.Core.Application.Common.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using MudBlazor;

namespace Dms.Api.Pages.DocumentsData
{
    public partial class DocumentDetails
    {
        [Inject] private IDmsDbContext dbContext { get; set; }
        [Inject] public IJSRuntime JS { get; set; }
        [Inject] public IMapper Mapper { get; set; }
        [Inject] public ILogger<DocumentDetails> logger { get; set; }

        async Task OnFormSave()
        {

        }

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
    }
}
