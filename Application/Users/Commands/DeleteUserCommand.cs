using MediatR;
using Currency.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Currency.API.Application.Users.Commands;

public record DeleteUserCommand(int Id) : IRequest<bool>;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly AppDbContext _db;

    public DeleteUserHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _db.Users.FindAsync(new object[] { request.Id }, cancellationToken);

        if (user == null) return false;

        //user.IsActive = false; 
        _db.Users.Remove(user);

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException)
        {
            throw new InvalidOperationException("No se puede eliminar el usuario porque tiene registros asociados. Considere desactivarlo en su lugar.");
        }
    }
}
