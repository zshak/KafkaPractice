namespace KafkaConsumer.Services.Contract
{
    public interface IUserService
    {
        Task<List<Decimal>> GetUserBetRange(int userId);
        Task<List<List<int>>> GetLeaderBoard();
    }
}
