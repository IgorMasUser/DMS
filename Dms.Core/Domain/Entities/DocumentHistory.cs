using Dms.Core.Domain.DomainBase;
using System.ComponentModel.DataAnnotations;

namespace Dms.Core.Domain.Entities
{
    public class DocumentHistory : Entity
    {
        [MaxLength(128)]
        public string? Event { get; set; }
        public DateTime Created { get; set; }
        public string PerformedBy { get; set; } = "Admin";
        public string? Details { get; set; }
        public FileData Document { get; set; }
        public int DocumentId { get; set; }
        public string? Comment { get; set; }
    }
}
