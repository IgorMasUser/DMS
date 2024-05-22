using Dms.Core.Application.Common.DTOs;
using Dms.Core.Application.Common.Interfaces;
using Dms.Core.Domain.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dms.Api.Pages.DocumentsData.Parts
{
    public partial class DocumentDetailsHistory
    {
        [Parameter] public FileDataDto Dto { get; set; } = new FileDataDto { FileAccounter = new FileAccounter() };
        private readonly DocumentHistoryDto currentDto = new();
        [Inject] IDocumentHistoryService documentHistoryService { get; set; }

        bool loading = true;

        protected async Task<GridData<DocumentHistoryDto>> Reload(GridState<DocumentHistoryDto> state)
        {
            try
            {
                loading = true;

                var history = await documentHistoryService.ReadHistoryAsync(Dto.Id);

                return new GridData<DocumentHistoryDto>() { TotalItems = history.Count, Items = history };
            }
            finally
            {
                loading = false;
            }
        }
    }
}
