using GoodBurger.Api.Domain.Entities;
using GoodBurger.Api.Infrastructure.Data;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoodBurger.Api.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public Task<User?> FindByUsernameAsync(string username, CancellationToken stoppingToken = default)
        => context.Users.FirstOrDefaultAsync(x => x.Username == username, stoppingToken);
}
