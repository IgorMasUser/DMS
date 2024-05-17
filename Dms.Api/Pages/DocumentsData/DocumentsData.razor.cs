using AutoMapper;
using Dms.Core.Application.Common.DTOs;
using Dms.Core.Application.Common.Interfaces;
using Dms.Core.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace Dms.Api.Pages.DocumentsData
{
    public partial class DocumentsData : ComponentBase
    {
        private string Title { get; set; } = "Documents data";

        private MudDataGrid<FileDataDto> _table = default!;
        private FileData metadata = new();
        private List<FileDataDto> filesDataDto = new List<FileDataDto>();
        private bool _loading;
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
                _loading = true;

                //var sort = state.SortDefinitions.FirstOrDefault();
                //var sortDescending = sort is not null ? sort.Descending : false;


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
                _loading = false;
            }
        }
        private void CheckMetaData(int Id)
        {
            //var navigationUrl = $"/{Core.Constants.Pages.DocumentDetail}/{Id}?source=/metadata/all";
            //NavigationManager.NavigateTo(navigationUrl);
        }

        protected async Task OnSearch(string text)
        {
            keyword = text;
            await _table.ReloadServerData();
        }
    }
}
