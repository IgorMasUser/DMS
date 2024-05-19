using AutoMapper;
using Dms.Core.Application.Common.DTOs;
using Dms.Core.Application.Common.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace Dms.Api.Pages.DocumentsData
{
    public abstract class DocumentDetailsPageBase : ComponentBase
    {
        [Parameter] public int? Id { get; set; }
        [SupplyParameterFromQuery] [Parameter] public string? Source { get; set; }
        [Inject] private IDmsDbContext dbContext { get; set; }

        [Inject] public IMapper Mapper { get; set; }
        protected FileDataDto Dto = new();
        protected MudTabs tabs;
        protected MudTabPanel accountingTab;
        protected bool ReadOnly => false;
        protected bool formInvalid;
        protected bool accountingFormInvalid;

        protected bool pageLoading = true;
        protected Exception? pageLoadingException;
               
               

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

                    try
                    {
                        await InitializePage();
                    }
                    catch (Exception ex)
                    {
                        pageLoadingException = ex;
                    }
                    finally
                    {
                        pageLoading = false;
                        StateHasChanged();
                    }
        }

        async Task InitializePage()
        {
            //Dto = Id.HasValue && Id.Value > 0
            //    ? await Manager.GetMetadataByIdAsync(Id.Value)
            //    : await Manager.New();

            var document = await dbContext.FilesData.Where(f=>f.Id == Id.Value).FirstOrDefaultAsync();
            Dto = Mapper.Map<FileDataDto>(document);
        }
    }
}
