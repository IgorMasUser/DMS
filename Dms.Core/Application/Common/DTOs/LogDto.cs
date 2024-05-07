namespace Dms.Core.Application.Common.DTOs
{
    public class LogDto
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public string? MessageTemplate { get; set; }
        public string Level { get; set; } = default!;
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public string? Exception { get; set; }
        public string? Properties { get; set; }
        public string? LogEvent { get; set; }
        public string? CorrelationId { get; set; }
    }
}
