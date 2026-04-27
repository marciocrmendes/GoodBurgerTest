using Scalar.AspNetCore;

namespace GoodBurger.Api.Infrastructure.Extensions;

public static class OpenApiExtensions
{
    public static WebApplicationBuilder AddCustomOpenApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
        return builder;
    }

    public static WebApplication MapOpenApiEndpoints(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.Title = "Good Burger API";
            });
        }
        return app;
    }
}
