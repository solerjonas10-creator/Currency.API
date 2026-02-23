using Currency.API.Application.Addresses.Commands;
using Currency.API.Data;
using Currency.API.Models;
using Currency.API.Models.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Currency.API.Application.Currency.Commands
{
    public record CreateCurrencyCommand(CurrencyDTO currencyDTO) : IRequest<int>;

    public class CreateCurrencyHandler : IRequestHandler<CreateCurrencyCommand, int>
    {
        private readonly AppDbContext _db;
        private readonly IValidator<CreateCurrencyCommand> _validator;

        public CreateCurrencyHandler(AppDbContext db, IValidator<CreateCurrencyCommand> validator)
        {
            _db = db;
            _validator = validator;
        }

        public async Task<int> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
        {
            // Validar Currency
            var result = await _validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid) throw new FluentValidation.ValidationException(result.Errors);

            var currency = new Models.Currency
            {
                Name = request.currencyDTO.Name,
                Code = request.currencyDTO.Code,
                RateToBase = request.currencyDTO.RateToBase
            };

            _db.Currencies.Add(currency);

            try
            {
                await _db.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException(ex.Message);

                throw;
            }

            return currency.Id;
        }
    }
}
