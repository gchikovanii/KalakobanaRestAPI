namespace Kalakobana.Game.Application.Repos
{
    public interface IValidationRepository
    {
        Task<bool> ExistsInTable(string tableName, string value);
    }
}
