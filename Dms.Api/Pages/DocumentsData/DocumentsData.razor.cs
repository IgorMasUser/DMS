using AutoMapper;
using Dms.Core.Application.Common.DTOs;
using Dms.Core.Application.Common.Interfaces;
using Dms.Core.Domain.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dms.Api.Pages.DocumentsData
{
    public partial class DocumentsData : ComponentBase
    {
        private string Title { get; set; } = "Documents data";

        private MudDataGrid<FileDataDto> table = default!;
        private FileData metadata = new();
        private List<FileDataDto> filesDataDto = new List<FileDataDto>();
        private bool loading;
        protected string? keyword;
        private int totalRecords;

        [Inject] public IMapper Mapper { get; set; }
        private FileDataDto fileDataDto = new FileDataDto();
        [Inject]
        private IDmsDbContext dbContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private async Task<GridData<FileDataDto>> ServerReload(GridState<FileDataDto> state)
        {
            try
            {
                loading = true;

                var documents = dbContext.FilesData;

                if (documents != null)
                {
                    filesDataDto = Mapper.Map<List<FileDataDto>>(documents);
                    totalRecords = filesDataDto.Count();
                }

                if (filesDataDto != null)
                {
                    return new GridData<FileDataDto> { TotalItems = totalRecords, Items = filesDataDto };
                }
                else
                {
                    return new GridData<FileDataDto> { TotalItems = 0, Items = new List<FileDataDto>() };
                }

            }
            finally
            {
                loading = false;
            }
        }
        private void CheckMetaData(int Id)
        {
            var navigationUrl = $"/{Core.Application.Common.Constants.Pages.DocumentDetails}/{Id}?source=/documentsdata/all";
            NavigationManager.NavigateTo(navigationUrl);
        }

        protected async Task OnSearch(string text)
        {
            keyword = text;
            await table.ReloadServerData();
        }
    }
}
