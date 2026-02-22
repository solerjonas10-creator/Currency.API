using Currency.API.Application.Users.Queries;
using Currency.API.Data;
using Currency.API.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Currency.API.Application.Currency.Queries
{
    public record GetCurrenciesQuery() : IRequest<List<Models.Currency>>;
    public class GetCurrenciesHandler : IRequestHandler<GetCurrenciesQuery, List<Models.Currency>>
    {
        private readonly AppDbContext _db;

        public GetCurrenciesHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Models.Currency>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
        {
            return await _db.Currencies
                .ToListAsync(cancellationToken);
        }
    }
}
