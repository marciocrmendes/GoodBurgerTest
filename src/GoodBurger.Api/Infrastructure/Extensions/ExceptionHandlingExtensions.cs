using GoodBurger.Api.Infrastructure.Configurations;

namespace GoodBurger.Api.Infrastructure.Extensions;

public static class ExceptionHandlingExtensions
{
    public static WebApplicationBuilder AddCustomExceptionHandling(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddExceptionHandler<ExceptionHandler>()
            .AddProblemDetails();

        return builder;
    }
}