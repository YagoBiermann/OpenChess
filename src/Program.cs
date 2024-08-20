using System.Net;
using OpenChess.Application;
using OpenChess.Domain;
using OpenChess.Infrastructure;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment.EnvironmentName;
builder.Configuration.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
builder.Services.AddSignalR();
builder.Services.AddLogging(opt =>
   {
       opt.AddConsole(c =>
       {
           c.TimestampFormat = "[HH:mm:ss] ";
       });
   });

// Redis
ConnectionMultiplexer redis;
try
{
    var redisConnectionString = builder.Configuration.GetValue<string>("REDIS_CONNECTION_STRING");
    redis = ConnectionMultiplexer.Connect(redisConnectionString!);
}
catch (System.Exception ex)
{
    Console.WriteLine($"Failed to connect to Redis: {ex.Message}");
    throw;
}
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
builder.Services.AddTransient<IMatchRepository, MatchRepository>();
//MediaTr
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

//Enforce secure connections
builder.Services.AddHttpsRedirection(options =>
{
    if (!builder.Environment.IsDevelopment())
    {
        options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
    }
    else
    {
        options.RedirectStatusCode = (int)HttpStatusCode.TemporaryRedirect;
    }

    options.HttpsPort = int.Parse(builder.Configuration.GetConnectionString("https_port")!);
});


builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    Console.WriteLine($"Running in development.");
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.MapHub<ChessHub>("/Chess");
app.Run();