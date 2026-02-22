using MediatR;
using Microsoft.EntityFrameworkCore;
using Currency.API.Data;
using Currency.API.Models;

namespace Currency.API.Application.Users.Queries
{
    // filtro opcional
    public record GetUsersQuery(bool? IsActive = null) : IRequest<List<User>>;

    public class GetUsersHandler : IRequestHandler<GetUsersQuery, List<User>>
    {
        private readonly AppDbContext _db;

        public GetUsersHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var query = _db.Users.AsQueryable();

            // Aplica el filtro solo si se envía en el parámetro
            if (request.IsActive.HasValue)
            {
                query = query.Where(u => u.IsActive == request.IsActive.Value);
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}
