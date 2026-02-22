using Currency.API.Application.Currency.Conversion;
using Currency.API.Application.Users.Commands;
using Currency.API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Currency.API.Validators
{
    public class CalculateConversionValidator : AbstractValidator<CurrencyConversionCalculate>
    {
        private readonly AppDbContext _db;

        public CalculateConversionValidator(AppDbContext db)
        {
            _db = db;

            RuleFor(x => x.conversionDTO.fromCurrencyCode)
                .NotEmpty()
                .MustAsync(async (code, cancellation) =>
                    await _db.Currencies.AnyAsync(c => c.Code == code, cancellation)
                ).WithMessage("El codigo de fromCurrencyCode no existe.");

            RuleFor(x => x.conversionDTO.fromCurrencyCode)
                .NotEmpty()
                .MustAsync(async (code, cancellation) =>
                    await _db.Currencies.AnyAsync(c => c.Code == code, cancellation)
                ).WithMessage("El codigo de toCurrencyCode no existe.");

            RuleFor(x => x.conversionDTO.amount)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
