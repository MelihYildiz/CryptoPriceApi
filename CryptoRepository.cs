using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoPriceAPI.Models;

namespace CryptoPriceAPI.Repositories
{
    public class CryptoRepository : ICryptoRepository
    {
        private readonly CryptoDbContext _context;

        public CryptoRepository(CryptoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CryptoSymbol>> GetAllSymbolsAsync()
        {
            return await _context.CryptoSymbols.ToListAsync();
        }

        public async Task<CryptoSymbol> GetSymbolByNameAsync(string symbol)
        {
            return await _context.CryptoSymbols.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }

        public async Task AddSymbolAsync(CryptoSymbol newSymbol)
        {
            _context.CryptoSymbols.Add(newSymbol);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSymbolAsync(CryptoSymbol updatedSymbol)
        {
            _context.CryptoSymbols.Update(updatedSymbol);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSymbolAsync(CryptoSymbol symbolToDelete)
        {
            _context.CryptoSymbols.Remove(symbolToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
