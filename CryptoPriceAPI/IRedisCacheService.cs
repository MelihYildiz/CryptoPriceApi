using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoPriceAPI.DTOs;

namespace CryptoPriceAPI
{
    public interface IRedisCacheService
    {
        Task AddSymbolAsync(CryptoSymbolDTO symbolDTO);
        Task<IEnumerable<CryptoSymbolDTO>> GetAllSymbolsAsync();
        Task<CryptoSymbolDTO> GetSymbolByNameAsync(string symbol);
    }
}
