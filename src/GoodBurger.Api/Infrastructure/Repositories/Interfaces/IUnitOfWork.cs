namespace GoodBurger.Api.Infrastructure.Repositories.Interfaces;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken stoppingToken = default);
}
