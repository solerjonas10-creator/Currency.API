using Currency.API.Data;
using Currency.API.Models;
using Currency.API.Validators;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Currency.API.Application.Users.Commands
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly AppDbContext _db;
        private readonly IValidator<CreateUserCommand> _validator;

        public CreateUserHandler(AppDbContext db, IValidator<CreateUserCommand> validator)
        {
            _db = db;
            _validator = validator;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Valida User
            var result = await _validator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid) {
                // Retornar errores
                throw new FluentValidation.ValidationException(result.Errors);
            }

            var hashed = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(request.userDTO.Password)));

            var user = new User
            {
                Name = request.userDTO.Name,
                Email = request.userDTO.Email,
                Password = hashed,
                IsActive = true
            };

            

            _db.Users.Add(user);

            try
            {
                await _db.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null &&
                    (ex.InnerException.Message.Contains("UNIQUE") ||
                     ex.InnerException.Message.Contains("duplicate") ||
                     ex.Message.Contains("cannot insert duplicate")))
                {
                    throw new InvalidOperationException("El email ya existe.");
                }

                throw;
            }

            return user.Id;
        }
    }
}