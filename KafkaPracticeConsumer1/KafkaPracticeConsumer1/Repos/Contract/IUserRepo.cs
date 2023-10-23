using KafkaConsumer1;
using Npgsql;

namespace KafkaConsumer.Repos.Contract
{
    public interface IUserRepo
    {
        Task AddUserInfo(KafkaModel model, NpgsqlConnection connection);    
    }
}
