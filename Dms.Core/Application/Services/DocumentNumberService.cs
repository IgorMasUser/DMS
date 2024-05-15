using Dms.Core.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dms.Core.Application.Services
{
    public class DocumentNumberService : IDocumentNumberService
    {
        private readonly IDmsDbContext dbContext;

        public DocumentNumberService(IDmsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<string> GenerateNextAsync()
        {
            var conn = dbContext.Database.GetDbConnection();

            if (conn.State != ConnectionState.Open) await conn.OpenAsync();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT NEXT VALUE FOR DmsDocumentNumberSequence";
                var scalar = await cmd.ExecuteScalarAsync();
                int next = (int)scalar!;

                var year = DateTime.Now.Year;

                return $"{year}-{next}";
            }


        }
    }
}
