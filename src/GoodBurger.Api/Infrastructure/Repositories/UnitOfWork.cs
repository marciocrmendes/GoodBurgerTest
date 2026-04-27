using GoodBurger.Api.Infrastructure.Data;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;

namespace GoodBurger.Api.Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public Task CommitAsync(CancellationToken stoppingToken = default) =>
        context.SaveChangesAsync(stoppingToken);
}
