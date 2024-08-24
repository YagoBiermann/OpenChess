using System.Net;
using System.Threading.RateLimiting;
using MatchMaking.Services;
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

//Request rate limit
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromSeconds(10),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 2,
            }));

    options.RejectionStatusCode = 429;
});

// Redis
var redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisConnectionString!);

builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
builder.Services.AddHttpClient();
builder.Services.AddSingleton(new RedisConnectionProvider(redis));
builder.Services.AddHostedService<IndexCreationService>();
builder.Services.AddHostedService<MatchMakingService>();

builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
    options.HttpsPort = 443;
});

builder.Services.AddControllers();
var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRouting();
app.MapControllers();

app.Run();
