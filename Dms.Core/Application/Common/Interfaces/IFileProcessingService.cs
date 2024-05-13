namespace Dms.Core.Application.Common.Interfaces
{
    public interface IFileProcessingService
    {
        Task ProcessFilesAndStoreToDb();
    }
}