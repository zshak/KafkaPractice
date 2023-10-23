using KafkaConsumer;
using KafkaConsumer.Models.Connection;
using KafkaConsumer.Repos;
using KafkaConsumer.Repos.Contract;
using KafkaConsumer.Services.Contract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IUserRepo, UserRepo>();

builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddOptions();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Connector>(builder.Configuration.GetSection("Connection"));

var app = builder.Build();


// Configure the HTTP request pipeline.


app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
