using Dms.Core.Application.Common.DTOs;

namespace Dms.Core.Application.Common.Interfaces
{
    public interface IDocumentHistoryService
    {
        Task WriteHistoryAsync(int documentEntityId, string eventName, string eventTitle,CancellationToken cancellationToken);
        Task<IList<DocumentHistoryDto>> ReadHistoryAsync(int documentEntityId);
    }
}
