using GoodBurger.Api.Domain.Abstractions;

namespace GoodBurger.Api.Domain.Entities;

public class User : BaseEntity<Guid>
{
    private User() { }

    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;

    public static User Create(string username, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Nome de usuário não pode ser vazio.", nameof(username));

        return string.IsNullOrWhiteSpace(passwordHash)
            ? throw new ArgumentException("Hash da senha não pode ser vazio.", nameof(passwordHash))
            : new User
        {
            Id = Guid.NewGuid(),
            Username = username.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash
        };
    }

    public bool VerifyPassword(string password)
        => BCrypt.Net.BCrypt.Verify(password, PasswordHash);
}
