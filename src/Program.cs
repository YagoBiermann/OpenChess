using System.Net;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment.EnvironmentName;
builder.Configuration.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
builder.Services.AddSignalR();
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


app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.MapHub<ChessHub>("/Chess");
app.Run();