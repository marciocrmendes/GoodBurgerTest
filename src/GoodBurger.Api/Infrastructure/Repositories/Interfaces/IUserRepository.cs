using GoodBurger.Api.Domain.Entities;

namespace GoodBurger.Api.Infrastructure.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> FindByUsernameAsync(string username, CancellationToken stoppingToken = default);
}
