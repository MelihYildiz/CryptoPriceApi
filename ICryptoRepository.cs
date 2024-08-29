using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoPriceAPI.Models;

namespace CryptoPriceAPI.Repositories
{
    public interface ICryptoRepository
    {
        Task<IEnumerable<CryptoSymbol>> GetAllSymbolsAsync();
        Task<CryptoSymbol> GetSymbolByNameAsync(string symbol);
        Task AddSymbolAsync(CryptoSymbol newSymbol);
        Task UpdateSymbolAsync(CryptoSymbol updatedSymbol);
        Task DeleteSymbolAsync(CryptoSymbol symbolToDelete);
    }
}
