
using Dapper;
using Kalakobana.AdminPanel.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Data;

namespace Kalakobana.AdminPanel.Application.Repos
{
    public class DataRepository : IDataRepository
    {
        private readonly IDbConnection _connection;
        private readonly ILogger _logger;

        public DataRepository(IConfiguration config, ILogger logger)
        {
            _connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
            _logger = logger;
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
                _logger.Error(ex, "Error deleting data");
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

        public async Task<bool> DeletePendingDataAsync(int id)
        {
            var query = "DELETE FROM dbo.Pending_Data WHERE Id = @Id";
            var parameters = new { Id = id };

            try
            {
                var result = await _connection.ExecuteAsync(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting pending data with Id {Id}", id);
                throw new Exception("Error deleting pending data", ex);
            }
        }

    }
}
