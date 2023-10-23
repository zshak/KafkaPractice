using Confluent.Kafka;
using Confluent.Kafka.Admin;
using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using kafkaProducer;
using static System.String;

class Program
{
    private static readonly ProducerConfig config = new ProducerConfig{ BootstrapServers = "localhost:9092"};

    public static async Task CreateTopic()
    {

        var adminClientConfig = new AdminClientConfig
        {
            BootstrapServers = "localhost:9092",
        };

        using (var adminClient = new AdminClientBuilder(adminClientConfig).Build())
        {
            var topic = "PracticeTopic";
            short replicationFactor = 1; // Set the desired replication factor here
            var partitions = 3; // Set the number of partitions for the topic

            try
            {
                // Get the list of topics
                var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(5));

                // Check if the topic already exists; if not, create it.
                if (!metadata.Topics.Exists(t => t.Topic == topic))
                {
                    var topicSpecification = new TopicSpecification
                    {
                        Name = topic,
                        NumPartitions = partitions,
                        ReplicationFactor = replicationFactor
                    };
                    await adminClient.CreateTopicsAsync(new[] { topicSpecification });
                    Console.WriteLine($"Topic '{topic}' created.");
                }
                else
                {
                    Console.WriteLine($"Topic '{topic}' already exists.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating topic: {ex}");
            }
        }
    }

    private static List<KafkaModel> GetInputs()
    {
        List<KafkaModel> result = new List<KafkaModel> ();
        while (true)
        {
            Console.Write("UserId: ");
            var input = Console.ReadLine();
            if(IsNullOrEmpty(input))
                break;
                int userId = Convert.ToInt32(input);

            Console.Write("Amount: ");
            input = Console.ReadLine();
            if (IsNullOrEmpty(input))
                break;
            int amount = Convert.ToInt32(input);

            result.Add(new KafkaModel(){UserId = userId, Amount = amount});
        }
        return result;
    }

    private async static Task ProduceInputs(List<KafkaModel> inputs)
    {
        foreach (var input in inputs)
        {
            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                var topic = "PracticeTopic";

                var value = JsonSerializer.Serialize(input);
                
                var deliveryReport = await producer.ProduceAsync(topic, new Message<string, string> { Key = input.ToString(), Value = value });
                Console.WriteLine($"Produced to: {deliveryReport.TopicPartitionOffset}");
            }
        }
    }

    static async Task Main(string[] args)
    {

        while (true)
        {
            await CreateTopic();

            List<KafkaModel> inputs = GetInputs();

            await ProduceInputs(inputs);
        }
    }
}
