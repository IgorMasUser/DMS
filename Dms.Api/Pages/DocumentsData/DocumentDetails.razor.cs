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

        async Task OnFormSave()
        {
        
        }
    }
}
