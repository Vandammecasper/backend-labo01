
namespace labo01.Validators;

public class WineValidator : AbstractValidator<Wine>
{
    public WineValidator()
    {
        RuleFor(wine => wine.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(wine => wine.Year).NotEmpty().GreaterThan(1900).LessThan(2023).WithMessage("Year is required");
    }
}

