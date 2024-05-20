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

        private MudDataGrid<FileDataDto> table = default!;
        private FileData metadata = new();
        private List<FileDataDto> filesDataDto = new List<FileDataDto>();
        private bool loading;
        protected string? keyword;
        private int totalRecords;
        [Inject] public ILogger<DocumentsData> logger { get; set; }

        [Inject] public IMapper Mapper { get; set; }

        [Inject]
        private IDmsDbContext dbContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private async Task<GridData<FileDataDto>> ServerReload(GridState<FileDataDto> state)
        {
            try
            {
                loading = true;

                var documents = await GetDocumentsData();
                if (documents != null)
                {
                    filesDataDto = Mapper.Map<List<FileDataDto>>(documents);
                    totalRecords = filesDataDto.Count();
                }

                if (filesDataDto != null && filesDataDto.Count() > 0)
                {
                    return new GridData<FileDataDto> { TotalItems = totalRecords, Items = filesDataDto };
                }
                else
                {
                    return new GridData<FileDataDto> { TotalItems = 0, Items = new List<FileDataDto>() };
                }

            }
            catch (Exception ex)
            {
                logger.LogError($"Error occurred while getting documents data: {ex.Message}");
                return new GridData<FileDataDto> { TotalItems = 0, Items = new List<FileDataDto>() };
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

        private async Task<List<FileData>> GetDocumentsData()
        {
            IQueryable<FileData> query = dbContext.FilesData;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(f => f.DocumentNumber != null && f.DocumentNumber.Contains(keyword));
            }

            query = query.Include(f => f.FileAccounter);
            var documents = await query.ToListAsync();

            return documents;
        }
    }
}
