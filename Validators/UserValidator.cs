using Currency.API.Application.Users.Commands;
using FluentValidation;
using Currency.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Currency.API.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly AppDbContext _db;

        public CreateUserValidator(AppDbContext db)
        {
            _db = db;

            RuleFor(x => x.userDTO.Name).NotEmpty();

            RuleFor(x => x.userDTO.Email)
                .EmailAddress()
                .NotEmpty()
                .MustAsync(async (email, cancellation) =>
                    !await _db.Users.AnyAsync(u => u.Email == email, cancellation))
                .WithMessage("El email ya existe.");

            RuleFor(x => x.userDTO.Password).NotEmpty();
        }
    }
}