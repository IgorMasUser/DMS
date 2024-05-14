using Dms.Core.Application.Common.Interfaces;
using Dms.Core.Domain.Entities;

namespace Dms.Core.Application.Services
{
    public class DatabaseService : IDatabaseService
    {
        public async Task StoreFilesAsync(List<FileData> files)
        {
            foreach (var file in files)
            {
                
            }
        }
    }
}
