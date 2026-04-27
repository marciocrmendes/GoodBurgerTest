using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;

namespace GoodBurger.Api.Features.Combos.DeleteCombo;

public class DeleteComboHandler(
    IComboRepository comboRepository,
    IUnitOfWork unitOfWork) : IDomainHandler<DeleteComboRequest, Result.Empty>
{
    public async Task<Result<Result.Empty>> HandleAsync(DeleteComboRequest request, CancellationToken stoppingToken = default)
    {
        var combo = await comboRepository.FirstOrDefaultAsync(request.Id, stoppingToken);
        if (combo is null)
            return Result.Failure<Result.Empty>(Error.NotFound("Combo não encontrado."));

        comboRepository.Remove(combo);
        await unitOfWork.CommitAsync(stoppingToken);
        return Result.Success<Result.Empty>();
    }
}
