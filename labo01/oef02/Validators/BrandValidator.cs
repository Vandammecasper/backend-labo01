namespace oef02.Validators;

public class BrandValidators : AbstractValidator<Brand>
{
    public BrandValidators()
    {
        RuleFor(b => b.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(b => b.Country).NotEmpty().WithMessage("Country is required");
        RuleFor(b => b.Logo).NotEmpty().WithMessage("Logo is required");
    }
}
