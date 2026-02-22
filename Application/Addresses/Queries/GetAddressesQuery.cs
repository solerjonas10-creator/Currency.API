using Currency.API.Application.Users.Queries;
using Currency.API.Data;
using Currency.API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Currency.API.Application.Addresses.Queries
{
    public record GetAddressesQuery(int userId) : IRequest<List<Address>>;

    public class GetAddressesHandler : IRequestHandler<GetAddressesQuery, List<Address>>
    {
        private readonly AppDbContext _db;

        public GetAddressesHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Address>> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
        {
            return await _db.Addresses
                .Where(a => a.UserId == request.userId)
                .ToListAsync(cancellationToken);
        }
    }
}
