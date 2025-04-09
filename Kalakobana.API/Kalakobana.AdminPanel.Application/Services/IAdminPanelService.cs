using Kalakobana.AdminPanel.Domain.Models;

namespace Kalakobana.AdminPanel.Application.Services
{
    public interface IAdminPanelService
    {
        Task<bool> InsertAsync(DataRequest dataRequest);
        Task<bool> DeleteAsync(DataRequest dataRequest);
        Task<IEnumerable<PendingData>> GetPendingDataAsync();
        Task<bool> ProcessPendingDataAsync(ProcessPendingData data);
        Task<bool> SeedNamesAsync();
    }
}
