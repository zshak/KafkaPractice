using Npgsql;

namespace KafkaConsumer.Repos.Contract
{
    public interface IUserRepo
    {
        Task<Decimal> GetUserBetsByRange(int userId, int from, int to, NpgsqlConnection connection);
        Task<List<int>> GetLeaderboard(int from, int to, int numUsers, NpgsqlConnection connection);
    }
}
