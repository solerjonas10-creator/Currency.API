using Currency.API.Application.Addresses.Commands;
using Currency.API.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Currency.API.Validators
{
    public class CreateAddressValidator : AbstractValidator<CreateAddressCommand>
    {
        private readonly AppDbContext _db;
        public CreateAddressValidator(AppDbContext db)
        {
            _db = db;
            /* -- El requerimiento pide que devuelva 404, por lo que maneje esta validacion en el CreateAddressCommand.cs
            RuleFor(x => x.UserId)
            .MustAsync(async (id, cancellation) =>
                await db.Users.AnyAsync(u => u.Id == id, cancellation))
            .WithMessage("El usuario especificado no existe."); 
            */

            RuleFor(x => x.addressDTO.Street).NotEmpty();
            RuleFor(x => x.addressDTO.City).NotEmpty();
            RuleFor(x => x.addressDTO.Country).NotEmpty();
            RuleFor(x => x.addressDTO.ZipCode).NotEmpty();
        }
    }

    public class UpdateAddressValidator : AbstractValidator<UpdateAddressCommand>
    {
        private readonly AppDbContext _db;
        public UpdateAddressValidator(AppDbContext db)
        {
            _db = db;

            RuleFor(x => x.addressDTO.Street).NotEmpty();
            RuleFor(x => x.addressDTO.City).NotEmpty();
            RuleFor(x => x.addressDTO.Country).NotEmpty();
            RuleFor(x => x.addressDTO.ZipCode).NotEmpty();
        }
    }
}
