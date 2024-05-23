using AutoMapper;
using Dms.Core.Application.Common.DTOs;
using Dms.Core.Application.Common.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using MudBlazor;

namespace Dms.Api.Pages.DocumentsData
{
    public abstract class DocumentDetailsPageBase : ComponentBase
    {
        [Parameter] public int? Id { get; set; }
        [SupplyParameterFromQuery][Parameter] public string? Source { get; set; }
        [Inject] protected IDmsDbContext dbContext { get; set; }
        [Inject] protected IMapper Mapper { get; set; }
        protected FileDataDto Dto = new();
        protected MudTabs tabs;
        [Inject]ILogger<DocumentDetailsPageBase> logger { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IJSRuntime JS { get; set; }
        [Inject] protected ISnackbar SnackbarComponent { get; set; }

        protected bool pageLoading = true;


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            try
            {
                await InitializePage();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occurred while getting document details: {ex.Message}");
            }
            finally
            {
                pageLoading = false;
                StateHasChanged();
            }
        }

        async Task InitializePage()
        {
            var document = await dbContext.FilesData.Where(f => f.Id == Id.Value).FirstOrDefaultAsync();
            Dto = Mapper.Map<FileDataDto>(document);
        }
    }
}
