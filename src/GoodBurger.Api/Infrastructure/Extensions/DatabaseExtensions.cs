using GoodBurger.Api.Infrastructure.Data;
using GoodBurger.Api.Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GoodBurger.Api.Infrastructure.Extensions;

public static class DatabaseExtensions
{
    public static WebApplicationBuilder AddCustomDbContext(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>()
            .AddDbContext<AppDbContext>((sp, options) =>
            {
                var connnectionString = builder.Configuration.GetConnectionString("goodburger-db");

                options.UseNpgsql(connnectionString, npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null);
                });

                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });


        builder.Services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>("AppDbContext-ready", tags: ["ready"]);

        return builder;
    }

    public static async Task MigrateAndSeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var databaseContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await databaseContext.Database.MigrateAsync();

        if (app.Environment.IsProduction()) return;

        await SeedData.SeedAsync(app.Environment, databaseContext);
    }
}
