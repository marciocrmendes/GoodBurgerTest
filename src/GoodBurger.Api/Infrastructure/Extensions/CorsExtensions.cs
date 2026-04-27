namespace GoodBurger.Api.Infrastructure.Extensions;

public static class CorsExtensions
{
    public static WebApplicationBuilder AddCustomCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader());
        });
        return builder;
    }

    public static WebApplication UseCorsMiddleware(this WebApplication app)
    {
        app.UseCors();
        return app;
    }
}
