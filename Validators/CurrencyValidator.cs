using Currency.API.Application.Users.Commands;
using FluentValidation;
using Currency.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Currency.API.Validators
{
    public class CreateCurrencyValidator : AbstractValidator<CreateCurrencyCommand>
    {
        private readonly AppDbContext _db;

        public CreateCurrencyValidator(AppDbContext db)
        {
            _db = db;

            RuleFor(x => x.currencyDTO.Code)
                .NotEmpty()
                .MustAsync(async (code, cancellation) =>
                    !await _db.Currencies.AnyAsync(c => c.Code == code, cancellation))
                .WithMessage("El codigo ingresado ya esta en uso.");

            RuleFor(x => x.currencyDTO.Name).NotEmpty();

            RuleFor(x => x.currencyDTO.RateToBase)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
