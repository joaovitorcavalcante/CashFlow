using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Expenses.Register;

public class RegisteExpenseValidatorTest
{
    [Fact]
    public void IsValidIsTrueAndErrorsEmpty_ValidRequest()
    {
        var validator = new RegisterExpenseValidator();
        
        var request = RequestRegisterExpenseJsonBuilder
            .Build();

        var result = validator.Validate(request);

        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("      ")]
    public void IsValidIsFalse_EmptyTitle(string invalidTitle)
    {
        var validator = new RegisterExpenseValidator();

        var request = RequestRegisterExpenseJsonBuilder
            .Build();

        request.Title = invalidTitle;

        var result = validator.Validate(request);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be(ResourceErrorMessages.TITLE_REQUIRED);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void ValidatorWithError_AmountLessThanZero(int invalidAmount)
    {
        var validator = new RegisterExpenseValidator();

        var request = RequestRegisterExpenseJsonBuilder
            .Build();

        request.Amount = invalidAmount;

        var result = validator.Validate(request);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
    }

    [Fact]
    public void ValidatorWithError_DateIsInFuture()
    {
        var validator = new RegisterExpenseValidator();

        var request = RequestRegisterExpenseJsonBuilder
            .Build();

        request.Date = DateTime.Now.AddDays(10);

        var result = validator.Validate(request);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be(ResourceErrorMessages.EXPENSES_CANNOT_FOR_THE_FUTURE);
    }

    [Fact]
    public void ValidatorWithError_PaymentTypeIsNotAEnum()
    {
        var validator = new RegisterExpenseValidator();

        var request = RequestRegisterExpenseJsonBuilder
            .Build();

        request.PaymentType = (PaymentType) 666;

        var result = validator.Validate(request);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.Errors[0].ErrorMessage.Should().Be(ResourceErrorMessages.PAYMENT_TYPE_INVALID);
    }
}