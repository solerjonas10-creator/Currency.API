namespace Currency.API.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEYNAME = "X-API-KEY";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        // Intentar obtener la clave del Header
        if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Falta la API Key.");
            return;
        }

        // Obtener la clave configurada en appsettings.json
        var apiKey = configuration.GetValue<string>("ApiKeySettings:X-API-KEY");

        // Comparar
        if (!apiKey!.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key inválida.");
            return;
        }

        // Si todo está bien, continuar al siguiente paso (Endpoint/Handler)
        await _next(context);
    }
}
