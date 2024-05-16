using System.ComponentModel;

namespace Dms.Core.Application.Common.DTOs
{
    public class DocumentHistoryDto
    {
        [Description("Event")] public string? Event { get; set; }
        [Description("Comment")] public string? Comment { get; set; }
        [Description("Created")] public DateTime Created { get; set; }
    }
}
