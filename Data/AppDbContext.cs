using Microsoft.EntityFrameworkCore;
using Currency.API.Models;

namespace Currency.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }

        // OBS: Uso Models.Currency en toda la API en vez de Solo Currency, porque mi namespace tambien se llama Currency, lo que crea conflicto...
        public DbSet<Models.Currency> Currencies { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Models.Currency>().HasIndex(u => u.Code).IsUnique();

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasOne(a => a.User)
                      .WithMany(u => u.Addresses)    // Un usuario tiene muchas direcciones
                      .HasForeignKey(a => a.UserId)
                      .OnDelete(DeleteBehavior.Cascade); // Si se borra al usuario, se borran sus direcciones
            });
        }
    }
}
