using FluentValidation.TestHelper;
using GoodBurger.Api.Features.Orders.UpdateOrder;
using Shouldly;

namespace GoodBurger.Tests.Validators;

public class UpdateOrderRequestValidatorTests
{
    private readonly UpdateOrderRequestValidator _sut = new();

    [Fact]
    public void Validate_ValidRequest_PassesAll()
    {
        var request = new UpdateOrderRequest(Guid.NewGuid(), [Guid.NewGuid()]);

        var result = _sut.TestValidate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Validate_EmptyList_FailsNotEmpty()
    {
        var request = new UpdateOrderRequest(Guid.NewGuid(), []);

        var result = _sut.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.MenuItemIds);
    }

    [Fact]
    public void Validate_EmptyGuid_FailsGuidCheck()
    {
        var request = new UpdateOrderRequest(Guid.NewGuid(), [Guid.Empty]);

        var result = _sut.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.MenuItemIds);
    }

    [Fact]
    public void Validate_DuplicateIds_FailsDuplicateCheck()
    {
        var id = Guid.NewGuid();
        var request = new UpdateOrderRequest(Guid.NewGuid(), [id, id]);

        var result = _sut.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.MenuItemIds);
    }
}
