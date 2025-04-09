using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Kalakobana.Game.Application.Repos
{
    public class ValidationRepository : IValidationRepository
    {
        private readonly IDbConnection _connection;
        public ValidationRepository(IConfiguration config)
        {
            _connection = new SqlConnection(config.GetConnectionString("DefaultConnection"));
        }

        public async Task<bool> ExistsInTable(string tableName, string value)
        {
            var sql = $"SELECT COUNT(1) FROM [{tableName}] WHERE Value = @value";
            var result = await _connection.ExecuteScalarAsync<int>(sql, new { value });
            return result > 0;
        }
    }
}
