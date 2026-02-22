using Currency.API.Data;
using Currency.API.Models;
using Currency.API.Models.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.IO;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;

namespace Currency.API.Application.Addresses.Commands
{
    public record CreateAddressCommand(int UserId, AddressDTO addressDTO) : IRequest<int>;

    public class CreateAddressHandler : IRequestHandler<CreateAddressCommand, int>
    {
        private readonly AppDbContext _db;
        private readonly IValidator<CreateAddressCommand> _validator;

        public CreateAddressHandler(AppDbContext db, IValidator<CreateAddressCommand> validator)
        {
            _db = db;
            _validator = validator;
        }

        public async Task<int> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            // Valida Address
            var result = await _validator.ValidateAsync(request, cancellationToken);
            if (!result.IsValid) throw new FluentValidation.ValidationException(result.Errors);


            var userExists = await _db.Users.AnyAsync(u => u.Id == request.UserId, cancellationToken);
            if (!userExists)
            {
                throw new KeyNotFoundException($"El usuario con ID {request.UserId} no existe.");
            }

            var address = new Address
            {
                UserId = request.UserId,
                Street = request.addressDTO.Street,
                City = request.addressDTO.City,
                Country = request.addressDTO.Country,
                ZipCode = request.addressDTO.ZipCode
            };



            _db.Addresses.Add(address);

            try
            {
                await _db.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException(ex.Message);

                throw;
            }

            return address.Id;
        }
    }
}
