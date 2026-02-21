using Microsoft.EntityFrameworkCore;
using Currency.API.Models;

namespace Currency.API.Data
{
    public class AppDbContext : DbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Address> Addresses { get; set; }
        DbSet<Models.Currency> Currencies { get; set; }
    }
}
