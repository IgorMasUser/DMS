using Dms.Core.Domain.DomainBase;

namespace Dms.Core.Domain.Entities
{
    public class FileData : Entity
    {
        public string? FileName { get; set; }
        public long FileSize { get; set; }
        public string? ContentType { get; set; }
        public byte[]? Data { get; set; }
        public DateTime? ReadAt { get; set; } = DateTime.Now;
    }
}
