using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace GoodBurger.Api.Features.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth").WithTags("Autenticação");

        group.MapPost("/login", async (
            [FromBody] LoginRequest request,
            LoginHandler handler,
            IValidator<LoginRequest> validator,
            CancellationToken stoppingToken) =>
        {
            var validation = await validator.ValidateAsync(request, stoppingToken);
            if (!validation.IsValid)
                return Results.ValidationProblem(validation.ToDictionary());

            var result = await handler.HandleAsync(request, stoppingToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.Problem(detail: result.Error.Message, statusCode: (int)result.Error.Code);
        })
        .WithName("Login")
        .WithSummary("Autenticar funcionário")
        .WithDescription("Retorna um token JWT para acesso ao sistema.")
        .Produces<LoginResponse>()
        .ProducesValidationProblem()
        .ProducesProblem(401)
        .AllowAnonymous();

        return app;
    }
}
