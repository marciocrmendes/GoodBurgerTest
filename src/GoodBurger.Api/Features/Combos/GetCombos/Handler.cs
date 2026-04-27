using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Features.Combos._Shared;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;

namespace GoodBurger.Api.Features.Combos.GetCombos;

public class GetCombosHandler(IComboRepository comboRepository) : IDomainHandler<GetCombosRequest, List<ComboResponse>>
{
    public async Task<Result<List<ComboResponse>>> HandleAsync(GetCombosRequest request, CancellationToken stoppingToken = default)
        => Result.Success(await comboRepository.GetAllAsync(stoppingToken));
}
