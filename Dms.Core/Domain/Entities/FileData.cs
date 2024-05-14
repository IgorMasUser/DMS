using Dms.Core.Domain.DomainBase;
using System.ComponentModel.DataAnnotations;

namespace Dms.Core.Domain.Entities
{
    public class FileData : Entity
    {
        [MaxLength(256)]
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string? ContentType { get; set; }
        public byte[]? Data { get; set; }
        public DateTime? ReadAt { get; set; } = DateTime.Now;
        public int? FileAccounterId { get; set; }
        public FileAccounter? FileAccounter { get; set; }
    }
}
