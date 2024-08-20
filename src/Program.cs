using System.Net;

var builder = WebApplication.CreateBuilder(args);
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

app.MapGet("/", () => "Hello World!");

app.Run();
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.Run();