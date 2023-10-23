using System;
using KafkaConsumer.Repos;
using KafkaConsumer.Repos.Contract;
using KafkaConsumer1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace KafkaConsumer1
{
    public class Program
    {
        public static void Main(string[] args)
        {


            Host.CreateDefaultBuilder(args)
                .ConfigureServices((IServiceCollection collection) =>
                {
                    collection.AddHostedService<KafkaConsumerService>();
                    collection.AddSingleton<IUserRepo, UserRepo>();
                }).Build().Run();
            
        }
    }
}

