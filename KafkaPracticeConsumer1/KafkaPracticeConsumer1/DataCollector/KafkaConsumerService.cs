using System;
using System.ComponentModel;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaConsumer.Repos.Contract;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace KafkaConsumer1
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly ConsumerConfig _config;
        private readonly IConsumer<string, string> _consumer;
        private readonly IUserRepo _userRepo;

        private const string _connectionString =
            "User ID=postgres;Password=zuka;Host=localhost;Port=5432;Database=NikasMagariAssignment;ApplicationName=placeHolder;";
        public KafkaConsumerService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
            _config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092", // Kafka broker address
                GroupId = "my-consumer-group",       // Consumer group ID
                AutoOffsetReset = AutoOffsetReset.Earliest, // Set to "latest" or "earliest" as needed
                PartitionAssignmentStrategy = PartitionAssignmentStrategy.RoundRobin
            };
            _consumer = new ConsumerBuilder<string, string>(_config).Build();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe("PracticeTopic");
            return ExecuteAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume();
                Console.WriteLine(consumeResult.Message.Value);
                KafkaModel model = JsonSerializer.Deserialize<KafkaModel>(consumeResult.Message.Value);
                using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
                {
                    await _userRepo.AddUserInfo(model, connection);
                }
                Console.WriteLine($"Received message: Userid - {model.UserId}, Amount - {model.Amount}");
            }
        }

    }
}

