using CryptoPriceAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoPriceAPI.Repositories
{
    public interface ICryptoRepository
    {
        Task<IEnumerable<CryptoSymbolDTO>> GetAllSymbolsAsync();
        Task<CryptoSymbolDTO> GetSymbolByNameAsync(string symbol);
        Task AddSymbolAsync(CryptoSymbolDTO newSymbolDTO); 
        Task UpdateSymbolAsync(CryptoSymbolDTO updatedSymbolDTO); 
        Task DeleteSymbolAsync(string symbol);
    }
}
