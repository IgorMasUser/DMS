using Dms.Core.Domain.DomainBase;

namespace Dms.Core.Domain.Entities
{
    public class FileAccounter : Entity
    {
        public List<FileData> files = new List<FileData>();
        public string Name { get; set; }
    }
}
