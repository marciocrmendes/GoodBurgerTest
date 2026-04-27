using GoodBurger.Api.Domain.Abstractions;
using GoodBurger.Api.Domain.Common;
using GoodBurger.Api.Infrastructure.Repositories.Interfaces;
using static GoodBurger.Api.Domain.Common.Result;

namespace GoodBurger.Api.Features.MenuItems.DeleteMenuItem;

public class DeleteMenuItemHandler(
    IMenuItemRepository menuItemRepository,
    IUnitOfWork unitOfWork) : IDomainHandler<DeleteMenuItemRequest, Empty>
{
    public async Task<Result<Empty>> HandleAsync(DeleteMenuItemRequest request, CancellationToken stoppingToken = default)
    {
        var item = await menuItemRepository.FindTrackedAsync(request.Id, stoppingToken);
        if (item is null)
            return Failure<Empty>(Error.NotFound("Item não encontrado."));

        menuItemRepository.Remove(item);
        await unitOfWork.CommitAsync(stoppingToken);
        return Success<Empty>();
    }
}
