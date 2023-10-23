using Dapper;
using KafkaConsumer.Repos.Contract;
using KafkaConsumer1;
using Npgsql;

namespace KafkaConsumer.Repos
{
    public class UserRepo : IUserRepo
    {
        public async Task AddUserInfo(KafkaModel model, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            const string query = @"INSERT INTO kafka_practice.user_info (user_id, amount) VALUES (@UserId, @Amount)";
            await connection.ExecuteAsync(query, model);
        }

    }
}
