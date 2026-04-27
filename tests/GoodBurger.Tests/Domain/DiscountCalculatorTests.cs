using GoodBurger.Api.Domain.Services;
using GoodBurger.Tests.Fakers;
using Shouldly;

namespace GoodBurger.Tests.Domain;

public class DiscountCalculatorTests
{
    private readonly DiscountCalculator _sut = new();

    [Fact]
    public void Calculate_ExactMatch_ReturnsDiscount()
    {
        var idA = Guid.NewGuid();
        var idB = Guid.NewGuid();
        var idC = Guid.NewGuid();
        var combo = ComboFaker.Create(20m, idA, idB, idC);

        var result = _sut.Calculate([combo], [idA, idB, idC]);

        result.ShouldBe(20m);
    }

    [Fact]
    public void Calculate_NoMatch_ReturnsZero()
    {
        var idA = Guid.NewGuid();
        var idB = Guid.NewGuid();
        var idC = Guid.NewGuid();
        var combo = ComboFaker.Create(20m, idA, idB);

        var result = _sut.Calculate([combo], [idA, idC]);

        result.ShouldBe(0m);
    }

    [Fact]
    public void Calculate_OrderIsSubset_ReturnsZero()
    {
        var idA = Guid.NewGuid();
        var idB = Guid.NewGuid();
        var idC = Guid.NewGuid();
        var combo = ComboFaker.Create(20m, idA, idB, idC);

        var result = _sut.Calculate([combo], [idA, idB]);

        result.ShouldBe(0m);
    }

    [Fact]
    public void Calculate_OrderIsSuperset_ReturnsZero()
    {
        var idA = Guid.NewGuid();
        var idB = Guid.NewGuid();
        var idC = Guid.NewGuid();
        var combo = ComboFaker.Create(20m, idA, idB);

        var result = _sut.Calculate([combo], [idA, idB, idC]);

        result.ShouldBe(0m);
    }

    [Fact]
    public void Calculate_MultipleCombos_ReturnsMax()
    {
        var idA = Guid.NewGuid();
        var idB = Guid.NewGuid();
        var combo10 = ComboFaker.Create(10m, idA, idB);
        var combo20 = ComboFaker.Create(20m, idA, idB);

        var result = _sut.Calculate([combo10, combo20], [idA, idB]);

        result.ShouldBe(20m);
    }

    [Fact]
    public void Calculate_NoCombos_ReturnsZero()
    {
        var result = _sut.Calculate([], [Guid.NewGuid()]);

        result.ShouldBe(0m);
    }
}
