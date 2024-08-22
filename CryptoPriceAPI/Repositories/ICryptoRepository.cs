using CryptoPriceAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoPriceAPI.Repositories
{
    public interface ICryptoRepository
    {
        Task<IEnumerable<CryptoSymbolDTO>> GetAllSymbolsAsync();
        Task<CryptoSymbolDTO> GetSymbolByNameAsync(string symbol);
        Task AddSymbolAsync(CryptoSymbolDTO newSymbolDTO); // Güncellenmiş parametre tipi
        Task UpdateSymbolAsync(CryptoSymbolDTO updatedSymbolDTO); // Güncellenmiş parametre tipi
        Task DeleteSymbolAsync(string symbol);
    }
}
