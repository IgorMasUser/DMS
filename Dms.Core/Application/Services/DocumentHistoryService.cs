using AutoMapper;
using Dms.Core.Application.Common.DTOs;
using Dms.Core.Application.Common.Interfaces;
using Dms.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dms.Core.Application.Services
{
    public class DocumentHistoryService : IDocumentHistoryService
    {
        private readonly IMapper mapper;
        private readonly IDmsDbContext dbContext;

        public DocumentHistoryService(IMapper mapper, IDmsDbContext dbContext)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<IList<DocumentHistoryDto>> ReadHistoryAsync(int documentId)
        {
            try
            {
                if (documentId <= 0) throw new ArgumentException("Invalid document ID.", nameof(documentId));

                var data = await dbContext.DocumentHistory
                    .Where(d => d.DocumentId == documentId)
                    .ToListAsync();

                return mapper.Map<List<DocumentHistoryDto>>(data);
            }
            catch
            {
                throw;
            }
        }

        public async Task WriteHistoryAsync(int documentId, string eventName, string eventText, CancellationToken cancellationToken = default)
        {
            try
            {
                var he = new DocumentHistory
                {
                    Created = DateTime.Now,
                    Event = eventName,
                    Details = eventText,
                    DocumentId = documentId
                };

                await dbContext.DocumentHistory.AddAsync(he, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                throw;
            }
        }
    }
}
