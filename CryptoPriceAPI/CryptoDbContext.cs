using Microsoft.EntityFrameworkCore;

namespace CryptoPriceAPI
{
    public class CryptoDbContext : DbContext
    {
        public CryptoDbContext(DbContextOptions<CryptoDbContext> options) : base(options) { }

        public DbSet<CryptoSymbol> CryptoSymbols { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CryptoSymbol>()
                .Property(s => s.Symbol)
                .IsRequired() // Symbol is required
                .HasMaxLength(4); // Symbol can be a maximum of 4 characters

            modelBuilder.Entity<CryptoSymbol>()
                .Property(s => s.Name)
                .IsRequired() // Name is required
                .HasMaxLength(25); // Name can be a maximum of 25 characters

            // Ensure uniqueness for Symbol
            modelBuilder.Entity<CryptoSymbol>()
                .HasIndex(s => s.Symbol)
                .IsUnique(); // Symbol must be unique
        }
    }

    public class CryptoSymbol
    {
        public int Id { get; set; }

        // Enforced constraints via Fluent API
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
