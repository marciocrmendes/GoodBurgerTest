using GoodBurger.Api.Domain.Common;

namespace GoodBurger.Api.Domain.Abstractions;

public interface IDomainHandler<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken stoppingToken = default);
}
