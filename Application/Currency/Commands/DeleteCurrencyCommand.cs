using Currency.API.Data;
using MediatR;


namespace Currency.API.Application.Currency.Commands
{
    public record DeleteCurrencyCommand(int Id) : IRequest<bool>;

    public class DeleteCurrencyHandler : IRequestHandler<DeleteCurrencyCommand, bool>
    {
        private readonly AppDbContext _db;
        public DeleteCurrencyHandler(AppDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
        {
            var currency = await _db.Currencies.FindAsync(new object[] { request.Id }, cancellationToken);
            if (currency == null) return false;

            _db.Currencies.Remove(currency);

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
