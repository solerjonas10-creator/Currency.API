using Currency.API.Application.Addresses.Commands;
using Currency.API.Data;
using Currency.API.Models;
using Currency.API.Models.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Currency.API.Application.Currency.Conversion
{
    public record CurrencyConversionCalculate(CurrencyConversionDTO conversionDTO) : IRequest<CurrencyConversion>;

    public class CurrencyConversionHandler : IRequestHandler<CurrencyConversionCalculate, CurrencyConversion>
    {
        private readonly AppDbContext _db;
        private readonly IValidator<CurrencyConversionCalculate> _validator;

        public CurrencyConversionHandler(AppDbContext db, IValidator<CurrencyConversionCalculate> validator)
        {
            _db = db;
            _validator = validator;
        }

        public async Task<CurrencyConversion> Handle(CurrencyConversionCalculate request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var from = await _db.Currencies
                .FirstOrDefaultAsync(c => c.Code == request.conversionDTO.fromCurrencyCode, cancellationToken);
            if (from == null) throw new ValidationException("Error de Moneda de origen."); 
            
            var to = await _db.Currencies
                .FirstOrDefaultAsync(c => c.Code == request.conversionDTO.toCurrencyCode, cancellationToken);
            if (to == null) throw new ValidationException("Error de Moneda de destino.");

            var montoBase = request.conversionDTO.amount * from.RateToBase;

            var conversion = new CurrencyConversion
            {
                fromCurrencyCode = request.conversionDTO.fromCurrencyCode,
                toCurrencyCode = request.conversionDTO.toCurrencyCode,
                amount = request.conversionDTO.amount,
                convertedAmount = montoBase / to.RateToBase
            };

            return conversion;
        }
    }
}
