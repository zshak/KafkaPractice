using Dapper;
using KafkaConsumer.Repos.Contract;
using Npgsql;

namespace KafkaConsumer.Repos
{
    public class UserRepo : IUserRepo
    {
        public async Task<Decimal> GetUserBetsByRange(int userId, int from, int to, NpgsqlConnection connection)
        {
            const string query = 
                @"SELECT SUM(amount)
                FROM kafka_practice.user_info
                where amount >= @from
                  and amount <= @to
                GROUP BY user_id
                having user_id = @userId";
            return await connection.QuerySingleOrDefaultAsync<Decimal>(query, new { userId, from, to });
        }

        public async Task<List<int>> GetLeaderboard(int from, int to, int numUsers, NpgsqlConnection connection)
        {
            const string query = @"
                Select user_id
                from kafka_practice.user_info
                where amount >= @from
                  and amount <= @to
                group by user_id
                order by SUM(amount)
                        ASC
                LIMIT @numUsers";

            return (await connection.QueryAsync<int>(query, new { from, to, numUsers })).ToList();
        }
    }
}
