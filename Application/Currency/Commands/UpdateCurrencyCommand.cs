using Currency.API.Models.DTOs;
using MediatR;
using FluentValidation;
using Currency.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Currency.API.Application.Currency.Commands
{
    public record UpdateCurrencyCommand(int Id, CurrencyDTO currencyDTO) : IRequest<bool>;

    public class UpdateCurrencyHandler : IRequestHandler<UpdateCurrencyCommand, bool>
    {
        private readonly AppDbContext _db;
        public UpdateCurrencyHandler(AppDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
        {
            // Aqui no es necesario el validator por que valido si deja el campo vacio, ignora ese campo
            // Buscar la currency existente
            var currency = await _db.Currencies.FindAsync(new object[] { request.Id }, cancellationToken);
            if (currency == null) return false;

            // Actualiza los campos modificados
            if (!string.IsNullOrWhiteSpace(request.currencyDTO.Name))
            {
                currency.Name = request.currencyDTO.Name;
            }
            if (!string.IsNullOrWhiteSpace(request.currencyDTO.Code))
            {
                currency.Code = request.currencyDTO.Code;
            }
            if (request.currencyDTO.RateToBase != 0)
            {
                currency.RateToBase = request.currencyDTO.RateToBase;
            }
            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}