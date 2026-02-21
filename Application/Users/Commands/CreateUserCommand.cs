using MediatR;
using Currency.API.Models.DTOs;

namespace Currency.API.Application.Users.Commands
{
    public record CreateUserCommand(UserDTO userDTO) : IRequest<int>;
}
