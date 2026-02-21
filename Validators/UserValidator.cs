using Currency.API.Application.Users.Commands;
using FluentValidation;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.userDTO.Name).NotEmpty();
        RuleFor(x => x.userDTO.Email).EmailAddress().NotEmpty();
    }
}