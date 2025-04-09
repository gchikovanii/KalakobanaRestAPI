using Dapper;
using Kalakobana.AdminPanel.Domain.Models;
using Serilog;
using System.Data;

namespace Kalakobana.AdminPanel.Application.Services
{
    public class AdminPanelService : IAdminPanelService
    {
        private readonly IDbConnection _connection;
        private readonly ILogger _logger;

        public AdminPanelService(ILogger logger, IDbConnection connection)
        {
            _logger = logger;
            _connection = connection;
        }
        public async Task<bool> InsertAsync(DataRequest dataRequest)
        {
            var query = $"INSERT INTO {dataRequest.TableName} (Value) VALUES (@Value)";
            var parameters = new { Value = dataRequest.Value };

            try
            {
                var result = await _connection.ExecuteAsync(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inserting value into table {TableName}: {Value}", dataRequest.TableName, dataRequest.Value);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(DataRequest dataRequest)
        {
            var query = $"DELETE FROM {dataRequest.TableName} WHERE Value = @Value";
            var parameters = new { Value = dataRequest.Value };

            try
            {
                var result = await _connection.ExecuteAsync(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting data from table {TableName}: {Value}", dataRequest.TableName, dataRequest.Value);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<PendingData>> GetPendingDataAsync()
        {
            var query = "SELECT Id, TableName, Value FROM dbo.Pending_Data";

            try
            {
                var pendingData = await _connection.QueryAsync<PendingData>(query);
                return pendingData;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error fetching pending data from dbo.Pending_Data");
                throw new Exception("Error fetching pending data", ex);
            }
        }
        public async Task<bool> ProcessPendingDataAsync(ProcessPendingData data)
        {
            // Fetch the pending data
            var pendingData = await _connection.QueryFirstOrDefaultAsync<PendingData>(
                "SELECT Id, TableName, Value FROM dbo.Pending_Data WHERE Id = @Id",
                new { Id = data.Id }
            );

            if (pendingData is null)
            {
                _logger.Warning("Pending data with Id {PendingId} not found.", data.Id);
                return false;
            }

            if (data.ApproveStatus)
            {
                var insertDataRequest = new DataRequest
                {
                    TableName = pendingData.TableName,
                    Value = pendingData.Value
                };

                var insertSuccess = await InsertAsync(insertDataRequest);

                if (insertSuccess)
                {
                    return await DeletePendingDataAsync(data.Id);
                }
                _logger.Error("Failed to insert data into table {TableName}.", pendingData.TableName);
                return false;
            }
            else
            {
                return await DeletePendingDataAsync(data.Id);
            }
        }

        private async Task<bool> DeletePendingDataAsync(int pendingId)
        {
            var query = "DELETE FROM dbo.Pending_Data WHERE Id = @Id";
            var parameters = new { Id = pendingId };

            try
            {
                var result = await _connection.ExecuteAsync(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting pending data with Id {PendingId}", pendingId);
                throw new Exception("Error deleting pending data", ex);
            }
        }

    }
}
