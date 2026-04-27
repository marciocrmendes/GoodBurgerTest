using Serilog;

namespace GoodBurger.Api.Infrastructure.Extensions;

public static class LoggingExtensions
{
    public static void ConfigureBootstrapLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();
    }

    public static WebApplicationBuilder AddCustomSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, serviceProvideer, log) =>
            log.ReadFrom.Configuration(context.Configuration)
               .ReadFrom.Services(serviceProvideer)
               .Enrich.FromLogContext()
               .Enrich.WithProperty("Application", "GoodBurger.Api")
               .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
               .Enrich.WithProperty("MachineName", Environment.MachineName)
               .WriteTo.Console());

        return builder;
    }

    public static WebApplication UseSerilogMiddleware(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        return app;
    }
}