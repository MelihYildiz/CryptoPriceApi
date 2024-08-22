using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoPriceAPI.DTOs;

namespace CryptoPriceAPI
{
    //Redis ile etkileşim sağlar
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _database;

        //Redis bağlantısı alır ve veritabanı nesnesini oluşturur
        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }

        // Yeni bir sembolü Redis'e ekleme
        public async Task AddSymbolAsync(CryptoSymbolDTO symbolDTO)
        {
            var symbolKey = $"symbol_{symbolDTO.Symbol}";
            var symbolValue = JsonConvert.SerializeObject(symbolDTO);
            // Sembolü Redis cache'ine ekler
            await _database.StringSetAsync(symbolKey, symbolValue);

            var allSymbolsKey = "all_symbols";
            // Tüm sembollerin listesini alır
            var symbolList = await _database.ListRangeAsync(allSymbolsKey);
            // Yeni sembolün listede olup olmadığını kontrol eder
            var symbolExists = symbolList.Any(x => x == symbolDTO.Symbol);
            if (!symbolExists)
            {
                await _database.ListRightPushAsync(allSymbolsKey, symbolDTO.Symbol);
            }
        }

        // Redis'ten tüm sembolleri getirir
        public async Task<IEnumerable<CryptoSymbolDTO>> GetAllSymbolsAsync()
        {
            var allSymbolsKey = "all_symbols";
            // Tüm sembollerin listesini alır
            var symbolKeys = await _database.ListRangeAsync(allSymbolsKey);

            var symbols = new List<CryptoSymbolDTO>();
            foreach (var symbol in symbolKeys)
            {
                var symbolKey = $"symbol_{symbol}";
                // Her sembolün verilerini alır
                var symbolData = await _database.StringGetAsync(symbolKey);
                if (symbolData.HasValue)
                {
                    // JSON verisini CryptoSymbolDTO nesnesine dönüştürür
                    var symbolDTO = JsonConvert.DeserializeObject<CryptoSymbolDTO>(symbolData);
                    symbols.Add(symbolDTO);
                }
            }

            return symbols;
        }

        // Redis'ten belirli bir sembolü getirir
        public async Task<CryptoSymbolDTO> GetSymbolByNameAsync(string symbol)
        {
            var symbolKey = $"symbol_{symbol}";
            // Belirli sembolün verilerini alır
            var symbolData = await _database.StringGetAsync(symbolKey);
            if (symbolData.HasValue)
            {
                // JSON verisini CryptoSymbolDTO nesnesine dönüştürür
                return JsonConvert.DeserializeObject<CryptoSymbolDTO>(symbolData);
            }
            return null; 
        }
    }
}
