namespace KoreanSecrets.API.Middleware;

public static class CustomExceptionHandlerExtension
{
    public static IApplicationBuilder UseCustomExceptionHandler(this
        IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandler>();
    }
}
