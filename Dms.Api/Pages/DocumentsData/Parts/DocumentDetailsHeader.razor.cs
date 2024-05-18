using Dms.Core.Application.Common.DTOs;
using Microsoft.AspNetCore.Components;

namespace Dms.Api.Pages.DocumentsData.Parts
{
    public partial class DocumentDetailsHeader
    {
        [Parameter] public FileDataDto Dto { get; set; } = new();
        [Parameter] public bool ReadOnly { get; set; } = new();

    }
}
