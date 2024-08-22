using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoPriceAPI.DTOs;

namespace CryptoPriceAPI
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _database;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }

        public async Task AddSymbolAsync(CryptoSymbolDTO symbolDTO)
        {
            var symbolKey = $"symbol_{symbolDTO.Symbol}";
            var symbolValue = JsonConvert.SerializeObject(symbolDTO);
            await _database.StringSetAsync(symbolKey, symbolValue);

            var allSymbolsKey = "all_symbols";
            var symbolList = await _database.ListRangeAsync(allSymbolsKey);
            var symbolExists = symbolList.Any(x => x == symbolDTO.Symbol);
            if (!symbolExists)
            {
                await _database.ListRightPushAsync(allSymbolsKey, symbolDTO.Symbol);
            }
        }

        public async Task<IEnumerable<CryptoSymbolDTO>> GetAllSymbolsAsync()
        {
            var allSymbolsKey = "all_symbols";
            var symbolKeys = await _database.ListRangeAsync(allSymbolsKey);

            var symbols = new List<CryptoSymbolDTO>();
            foreach (var symbol in symbolKeys)
            {
                var symbolKey = $"symbol_{symbol}";
                var symbolData = await _database.StringGetAsync(symbolKey);
                if (symbolData.HasValue)
                {
                    var symbolDTO = JsonConvert.DeserializeObject<CryptoSymbolDTO>(symbolData);
                    symbols.Add(symbolDTO);
                }
            }

            return symbols;
        }

        public async Task<CryptoSymbolDTO> GetSymbolByNameAsync(string symbol)
        {
            var symbolKey = $"symbol_{symbol}";
            var symbolData = await _database.StringGetAsync(symbolKey);
            if (symbolData.HasValue)
            {
                return JsonConvert.DeserializeObject<CryptoSymbolDTO>(symbolData);
            }
            return null;
        }
    }
}
