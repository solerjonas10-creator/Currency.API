using MediatR;
using Currency.API.Data;
using Currency.API.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace Currency.API.Application.Users.Commands;

public record UpdateUserCommand(int Id, UserDTO UserDto) : IRequest<bool>;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly AppDbContext _db;
    private readonly IValidator<UpdateUserCommand> _validator;

    public UpdateUserHandler(AppDbContext db, IValidator<UpdateUserCommand> validator)
    {
        _db = db;
        _validator = validator;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        // Buscar el usuario existente
        var user = await _db.Users.FindAsync(new object[] { request.Id }, cancellationToken);
        if (user == null) return false;

        // Actualizar campos si no vienen vacios en el DTO
        if (!string.IsNullOrWhiteSpace(request.UserDto.Name))
        {
            user.Name = request.UserDto.Name;
        }

        if (!string.IsNullOrWhiteSpace(request.UserDto.Email))
        {
            user.Email = request.UserDto.Email;
        }

        if (!string.IsNullOrWhiteSpace(request.UserDto.Password))
        {
            user.Password = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(request.UserDto.Password)));
        }

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
