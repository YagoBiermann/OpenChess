public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly string ApiKeyHeaderName;

    private readonly string _apiKey;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _apiKey = Environment.GetEnvironmentVariable("INTERNAL_API_KEY") ?? throw new Exception("Internal API Key not set!");
        _configuration = configuration;
        ApiKeyHeaderName = _configuration["ApiSettings:Headers:ApiKey"] ?? throw new Exception("Header not set!");
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/internal"))
        {
            if (context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                if (extractedApiKey == _apiKey)
                {
                    await _next(context);
                    return;
                }
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.CompleteAsync();
        }
        else
        {
            await _next(context);
        }
    }
}