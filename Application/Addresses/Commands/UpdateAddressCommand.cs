using Currency.API.Data;
using Currency.API.Models;
using Currency.API.Models.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Currency.API.Application.Addresses.Commands
{
    public record UpdateAddressCommand(int Id, AddressDTO addressDTO) : IRequest<bool>;

    public class UpdateAddressHandler : IRequestHandler<UpdateAddressCommand, bool>
    {
        private readonly AppDbContext _db;
        private readonly IValidator<UpdateAddressCommand> _validator;

        public UpdateAddressHandler(AppDbContext db, IValidator<UpdateAddressCommand> validator)
        {
            _db = db;
            _validator = validator;
        }

        public async Task<bool> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            // Aqui no es necesario el validator por que valido si deja el campo vacio, ignora ese campo

            // Buscar la direccion existente
            var address = await _db.Addresses.FindAsync(new object[] { request.Id }, cancellationToken);
            if (address == null) return false;

            // Actualiza los campos modificados
            if (!string.IsNullOrWhiteSpace(request.addressDTO.Street))
            {
                address.Street = request.addressDTO.Street;
            }

            if (!string.IsNullOrWhiteSpace(request.addressDTO.City))
            {
                address.City = request.addressDTO.City;
            }

            if (!string.IsNullOrWhiteSpace(request.addressDTO.Country))
            {
                address.Country = request.addressDTO.Country;
            }

            if (!string.IsNullOrWhiteSpace(request.addressDTO.ZipCode))
            {
                address.ZipCode = request.addressDTO.ZipCode;
            }

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
    
}
