using System.Net;
using Redis.OM;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment.EnvironmentName;
builder.Configuration.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
// http client
builder.Services.AddHttpClient("HttpClient", client =>
{
    var header = builder.Configuration["Headers.ApiKey"] ?? throw new Exception("Header not set!");
    var value = Environment.GetEnvironmentVariable("INTERNAL_API_KEY") ?? throw new Exception("Internal API Key not set!");
    client.DefaultRequestHeaders.Add(header, value);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.BaseAddress = new Uri(builder.Configuration["BaseUrl"] ?? throw new Exception("Base Url not set!"));
});

// Redis
var redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisConnectionString!);

builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
builder.Services.AddHttpClient();
builder.Services.AddSingleton(new RedisConnectionProvider(redis));

builder.Services.AddControllers();
var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();
app.MapControllers();

app.Run();
