using Dms.Core.Domain.Entities;

namespace Dms.Core.Application.Common.DTOs
{
    public class FileDataDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string? ContentType { get; set; }
        public byte[]? Data { get; set; }
        public DateTime? ReadAt { get; set; } = DateTime.Now;
        public int? FileAccounterId { get; set; }
        public FileAccounter? FileAccounter { get; set; }
        public string DocumentNumber { get; set; }
        public ICollection<DocumentHistoryDto> DocumentHistory { get; set; }
    }
}
