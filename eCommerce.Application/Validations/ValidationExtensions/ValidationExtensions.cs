using FluentValidation;
namespace eCommerce.Application.Validators.ValidationExtensions
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> ValidEmail<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("A valid email is required.");
        }
        public static IRuleBuilderOptions<T, string> ValidName<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name cannot exceed 100 characters.");
        }
        public static IRuleBuilderOptions<T, string> StrongPassword<T>(
        this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]")
                .WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d")
                .WithMessage("Password must contain at least one number.")
                .Matches(@"[^\w]")
                .WithMessage("Password must contain at least one special character.");
        }
        public static IRuleBuilderOptions<T, int> ValidId<T>(
        this IRuleBuilder<T, int> ruleBuilder)
        {
           return ruleBuilder
                .NotEmpty()
                .WithMessage("Id is required.")
                .GreaterThan(0)
                .WithMessage("Id must be greater than zero.");
        }
    }
}
