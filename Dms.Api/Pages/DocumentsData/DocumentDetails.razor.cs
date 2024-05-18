using AutoMapper;
using Dms.Core.Application.Common.DTOs;
using Dms.Core.Application.Common.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dms.Api.Pages.DocumentsData
{
    public partial class DocumentDetails
    {
        [Inject] private IDmsDbContext dbContext { get; set; }

        [Inject] public IMapper Mapper { get; set; }

        [Parameter] public int? Id { get; set; }
        //[SupplyParameterFromQuery][Parameter] public string? Source { get; set; }
        protected FileDataDto Dto = new();
        protected MudTabs tabs;
        protected MudTabPanel accountingTab;
        protected bool formInvalid;
        protected bool accountingFormInvalid;

        async Task OnFormSave()
        {
                //Dto = await Manager.UpdateMetadataAsync(Dto);


            //if (Dto.MetadataType == CSW.Identity.Core.DataAccess.SQL.Entities.Dms.Enums.DocumentMetadataType.Invoice)
            //{
            //    if (await _accountingForm.Validate())
            //    {
            //        accountingFormInvalid = false;
            //    }
            //    else
            //    {
            //        accountingFormInvalid = true;

            //        tabs.ActivatePanel(accountingTab);

            //        Snackbar.Add(Loc["DocumentAccountingInvalid"], MudBlazor.Severity.Error);
            //    }
            //}


            //if (!formInvalid && !accountingFormInvalid)
            //{
            //    Snackbar.Add(Loc["DocumentMetadataUpdated"], MudBlazor.Severity.Success);
            //}
        }

        //async Task ReadyForMining()
        //{
        //    Dto.GlobalState = DmsGlobalState.InProcessing;
        //    Dto.MinorState = DmsMinorStates.InProcessing.Mining;

        //    await OnFormSave();

        //    Snackbar.Add($"__registered for manual mining", MudBlazor.Severity.Info);
        //}

        //void OnFormClose()
        //{
        //    NavigationManager.NavigateTo(Source ?? "/");
        //}
    }
}
