using Dms.Core.Application.Common.DTOs;
using Dms.Core.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace Dms.Api.Pages.DocumentsData.Parts
{
    public partial class DocumentDetailsBody
    {
        [Parameter] public FileDataDto Dto { get; set; } = new FileDataDto { FileAccounter = new FileAccounter()};
    }
}
