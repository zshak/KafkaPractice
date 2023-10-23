using Confluent.Kafka;
using KafkaConsumer.Models.Connection;
using KafkaConsumer.Repos.Contract;
using KafkaConsumer.Services.Contract;
using Microsoft.Extensions.Options;
using Npgsql;

namespace KafkaConsumer;

public class UserService : IUserService
{
        private readonly IUserRepo _userRepo;
        private readonly Connector _connector;
        private static readonly List<(int, int)> Ranges = new List<(int, int)>(){(0, 100), (100, 500), (500, Int32.MaxValue)};
        public UserService(IUserRepo userRepo, IOptions<Connector> connectionString)
        {
            _userRepo = userRepo;
            _connector = connectionString.Value;
        }


        public async Task<List<Decimal>> GetUserBetRange(int userId)
        {
            List<Decimal> UserRanges = new List<decimal>();
            using (NpgsqlConnection connection = new NpgsqlConnection(_connector.ConnectionString))
            {
                foreach (var range in Ranges)
                {
                    UserRanges.Add(await _userRepo.GetUserBetsByRange(userId, range.Item1, range.Item2, connection));  
                }
            }
            return UserRanges;
        }

        public async Task<List<List<int>>> GetLeaderBoard()
        {
            List<List<int>> leaderBoard = new List<List<int>>();
            using (NpgsqlConnection connection = new NpgsqlConnection(_connector.ConnectionString))
            {
                foreach (var range in Ranges)
                {
                    leaderBoard.Add(await _userRepo.GetLeaderboard(range.Item1, range.Item2, 3,  connection));
                }
            }
            return leaderBoard;
        }
}


