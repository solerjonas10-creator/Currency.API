using Currency.API.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Currency.API.Application.Addresses.Commands
{
    public record DeleteAddressCommand(int Id) : IRequest<bool>;

    public class DeleteAddressHandler : IRequestHandler<DeleteAddressCommand, bool>
    {
        private readonly AppDbContext _db;

        public DeleteAddressHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var address = await _db.Addresses.FindAsync(new object[] { request.Id }, cancellationToken);

            if (address == null) return false;

            _db.Addresses.Remove(address);

            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
