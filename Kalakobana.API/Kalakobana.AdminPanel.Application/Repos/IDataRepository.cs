using Kalakobana.AdminPanel.Domain.Models;

namespace Kalakobana.AdminPanel.Application.Repos
{
    public interface IDataRepository
    {
        Task<bool> InsertAsync(DataRequest request);
        Task<bool> DeleteAsync(DataRequest request);
    }
}
