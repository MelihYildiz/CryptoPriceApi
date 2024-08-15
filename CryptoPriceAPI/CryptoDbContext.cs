using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CryptoPriceAPI
{
    public class CryptoDbContext : DbContext
    {
        public CryptoDbContext(DbContextOptions<CryptoDbContext> options) : base(options) { }

        public DbSet<CryptoSymbol> CryptoSymbols { get; set; }
    }

    public class CryptoSymbol
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
    }
}