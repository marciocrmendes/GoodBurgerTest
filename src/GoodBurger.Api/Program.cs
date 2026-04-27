using GoodBurger.Api.Infrastructure.Extensions;
using Serilog;

LoggingExtensions.ConfigureBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddCustomSerilog();
    builder.AddCustomDbContext();
    builder.AddCustomServices();
    builder.AddCustomHandlers();
    builder.AddCustomRepositories();
    builder.AddCustomAuthentication();
    builder.AddCustomExceptionHandling();
    builder.AddCustomCors();
    builder.AddCustomOpenApi();

    var app = builder.Build();

    app.UseSerilogMiddleware();
    app.UseCorsMiddleware();
    app.UseAuthenticationMiddleware();
    app.MapOpenApiEndpoints();

    await app.MigrateAndSeedDatabaseAsync();

    app.MapFeatureEndpoints();

    app.MapGet("/", context =>
    {
        context.Response.Redirect("/scalar");
        return Task.CompletedTask;
    });

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
