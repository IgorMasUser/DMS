using System.ComponentModel;

namespace Dms.Core.Application.Common.DTOs
{
    public class LogDto
    {
        [Description("Id")] public int Id { get; set; }
        [Description("Message")] public string? Message { get; set; }
        [Description("Message Template")] public string? MessageTemplate { get; set; }
        [Description("Level")] public string Level { get; set; } = default!;
        [Description("Timestamp")] public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        [Description("Exception")] public string? Exception { get; set; }
        [Description("Properties")] public string? Properties { get; set; }
        [Description("Log Event")] public string? LogEvent { get; set; }
        [Description("CorrelationId")] public string? CorrelationId { get; set; }
    }
}
