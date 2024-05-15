namespace Dms.Core.Application.Common.Interfaces
{
    public interface IDocumentNumberService
    {
        Task<string> GenerateNextAsync();
    }
}
