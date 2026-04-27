using FluentValidation;
using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Infrastructure.Data;
using GoodBurger.Api.Infrastructure.Repositories;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;

namespace GoodBurger.Api.Infrastructure.Extensions;

public static class ServicesExtensions
{
    public static WebApplicationBuilder AddCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<AppUser>();

        return builder;
    }

    public static WebApplicationBuilder AddCustomHandlers(this WebApplicationBuilder builder)
    {
        builder.Services.Scan(scan => scan
            .FromAssembliesOf(typeof(IDomainHandler<,>))
            .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Handler")))
            .AsSelf()
            .WithScopedLifetime());

        builder.Services.AddValidatorsFromAssemblyContaining<Program>();

        return builder;
    }

    public static WebApplicationBuilder AddCustomRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.Scan(scan => scan
            .FromAssembliesOf(typeof(IOrderRepository))
            .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        return builder;
    }
}