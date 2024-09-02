using System.Net;
using OpenChess.Application;
using OpenChess.Domain;
using OpenChess.Infrastructure;
using StackExchange.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http.Connections;
using Redis.OM;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment.EnvironmentName;
builder.Configuration.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
builder.Services.AddLogging(opt =>
   {
       opt.AddConsole(c =>
       {
           c.TimestampFormat = "[HH:mm:ss] ";
       });
   });

// Redis

var redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisConnectionString!);

builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
builder.Services.AddSingleton(new RedisConnectionProvider(redis));
builder.Services.AddTransient<IMatchRepository, MatchRepository>();
builder.Services.AddSingleton<MoveTrackingService>();
builder.Services.AddSingleton<ConnectionTrackingService>();
builder.Services.AddSingleton<MatchTrackingService>();
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

//JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new Exception("JWT key not set!"))),
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            Console.WriteLine($"Access Token: {accessToken}");
            Console.WriteLine($"Request Path: {path}");
            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/chessHub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddSignalR(options =>
{
    options.KeepAliveInterval = TimeSpan.FromSeconds(10);
    options.HandshakeTimeout = TimeSpan.FromSeconds(5);
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebClient",
        builder =>
        {
            builder.WithOrigins(Environment.GetEnvironmentVariable("WEB_CLIENT") ?? throw new Exception("WEB_CLIENT not set!")) // Your client URL
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});
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
app.UseCors("AllowWebClient");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ChessHub>("/chessHub", options =>
{
    options.Transports = HttpTransportType.WebSockets;
});
app.UseMiddleware<ApiKeyMiddleware>();
app.Run();