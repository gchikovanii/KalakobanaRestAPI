using Dapper;
using Kalakobana.Game.Domain.Models;
using System.Data;

namespace Kalakobana.Game.Application.Repos
{
    public class VotingRepository : IVotingRepository
    {
        private readonly IDbConnection _dbConnection;
        public VotingRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<int> AddPendingDataAsync(PendingData data)
        {
            if (string.IsNullOrWhiteSpace(data?.TableName) || string.IsNullOrWhiteSpace(data?.Value))
                throw new ArgumentException("Both TableName and Value must be provided.");

            const string sql = @"
                INSERT INTO dbo.Pending_Data (TableName, Value)
                VALUES (@TableName, @Value);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            return await _dbConnection.ExecuteScalarAsync<int>(sql, data);
        }
    }
}
