using Microsoft.EntityFrameworkCore;

public class CryptoDbContext : DbContext
{
    public CryptoDbContext(DbContextOptions<CryptoDbContext> options) : base(options)
    {
    }

    public DbSet<CryptoCurrency> CryptoCurrencies { get; set; }
}
