using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterExpenseJsonBuilder
{
    public static RequestRegisterExpenseJson Build()
    {
        return new Faker<RequestRegisterExpenseJson>()
            .RuleFor(r => r.Title, Faker => Faker.Commerce.ProductName())
            .RuleFor(r => r.Description, Faker => Faker.Commerce.ProductDescription())
            .RuleFor(r => r.Amount, Faker => Faker.Random.Decimal(min: 1, max: 100))
            .RuleFor(r => r.PaymentType, Faker => Faker.PickRandom<PaymentType>())
            .RuleFor(r => r.Date, Faker => Faker.Date.Past(1));
    }
}