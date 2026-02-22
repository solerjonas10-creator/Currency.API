using MediatR;
using Currency.API.Data;
using Currency.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Currency.API.Application.Users.Queries
{
    public record GetUserByIdQuery(int Id) : IRequest<User?>;

    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User?>
    {
        private readonly AppDbContext _db;

        public GetUserByIdHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
        }
    }
}
