using Microsoft.EntityFrameworkCore;

namespace CryptoPriceAPI
{
    // Veritabanı ile etkileşim sağlar
    public class CryptoDbContext : DbContext
    {
        // DbContext seçeneklerini alır
        public CryptoDbContext(DbContextOptions<CryptoDbContext> options) : base(options) { }

        // Db'deki CryptoSymbols tablosu
        public DbSet<CryptoSymbol> CryptoSymbols { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // CryptoSymbol varlık konfigürasyonları
            modelBuilder.Entity<CryptoSymbol>()
                .Property(s => s.Symbol)
                .IsRequired() 
                .HasMaxLength(4); 

            modelBuilder.Entity<CryptoSymbol>()
                .Property(s => s.Name)
                .IsRequired() 
                .HasMaxLength(25);

            // Symbol alanı için benzersizliği kontrol eder.
            modelBuilder.Entity<CryptoSymbol>()
                .HasIndex(s => s.Symbol)
                .IsUnique(); 
        }
    }

    //Veritabanı tablosunun yapısını temsil eder
    public class CryptoSymbol
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
