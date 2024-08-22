using CryptoPriceAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CryptoPriceAPI.Repositories
{
    public class CryptoRepository : ICryptoRepository
    {
        private readonly CryptoDbContext _context;
        private readonly IDistributedCache _cache;
        private static readonly string CacheKeyAllSymbols = "all_symbols";
        private static readonly string CacheKeyPrefixSymbol = "symbol_";

        // DbContext ve Cache bağlarını alır.
        public CryptoRepository(CryptoDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // Tüm kripto sembollerini getirir; öncelikle cache'den kontrol eder.
        public async Task<IEnumerable<CryptoSymbolDTO>> GetAllSymbolsAsync()
        {
            var cachedData = await _cache.GetStringAsync(CacheKeyAllSymbols);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<IEnumerable<CryptoSymbolDTO>>(cachedData);
            }

            // Cache'de veri yoksa, veritabanından çekme
            var symbols = await _context.CryptoSymbols
                .Select(s => new CryptoSymbolDTO
                {
                    Id = s.Id,
                    Symbol = s.Symbol,
                    Name = s.Name,
                    Price = s.Price
                }).ToListAsync();

            // Cache'e veri ekleme
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
            await _cache.SetStringAsync(CacheKeyAllSymbols, JsonSerializer.Serialize(symbols), options);
            return symbols;
        }

        // Belirli bir sembolü getirir; öncelikle cache'den kontrol eder.
        public async Task<CryptoSymbolDTO> GetSymbolByNameAsync(string symbol)
        {
            var cacheKey = $"{CacheKeyPrefixSymbol}{symbol}";
            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonSerializer.Deserialize<CryptoSymbolDTO>(cachedData);
            }

            // Cache'de veri yoksa, veritabanından çekme
            var cryptoSymbol = await _context.CryptoSymbols
                .Where(s => s.Symbol == symbol)
                .Select(s => new CryptoSymbolDTO
                {
                    Id = s.Id,
                    Symbol = s.Symbol,
                    Name = s.Name,
                    Price = s.Price
                }).FirstOrDefaultAsync();

            if (cryptoSymbol == null)
            {
                return null;
            }

            // Cache'e veri ekleme
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(cryptoSymbol), options);
            return cryptoSymbol;
        }

        // Yeni bir kripto sembolü ekler.
        public async Task AddSymbolAsync(CryptoSymbolDTO newSymbolDTO)
        {
            var newSymbol = new CryptoSymbol
            {
                Symbol = newSymbolDTO.Symbol,
                Name = newSymbolDTO.Name,
                Price = newSymbolDTO.Price
            };

            _context.CryptoSymbols.Add(newSymbol);
            await _context.SaveChangesAsync();

            // Cache'i güncelleme: tüm semboller ve yeni bir sembol için
            await _cache.RemoveAsync(CacheKeyAllSymbols);
            await _cache.SetStringAsync($"{CacheKeyPrefixSymbol}{newSymbol.Symbol}", JsonSerializer.Serialize(newSymbolDTO), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
        }

        // Mevcut bir kripto sembolünü günceller.
        public async Task UpdateSymbolAsync(CryptoSymbolDTO updatedSymbolDTO)
        {
            var existingSymbol = await _context.CryptoSymbols.FindAsync(updatedSymbolDTO.Id);
            if (existingSymbol == null)
            {
                return;
            }

            existingSymbol.Symbol = updatedSymbolDTO.Symbol;
            existingSymbol.Name = updatedSymbolDTO.Name;
            existingSymbol.Price = updatedSymbolDTO.Price;

            _context.CryptoSymbols.Update(existingSymbol);
            await _context.SaveChangesAsync();

            // Cache'i güncelleme: tüm semboller ve güncellenmiş sembol için
            await _cache.RemoveAsync(CacheKeyAllSymbols);
            await _cache.SetStringAsync($"{CacheKeyPrefixSymbol}{updatedSymbolDTO.Symbol}", JsonSerializer.Serialize(updatedSymbolDTO), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
        }

        // Belirli bir kripto sembolünü siler.
        public async Task DeleteSymbolAsync(string symbol)
        {
            var existingSymbol = await _context.CryptoSymbols
                .Where(s => s.Symbol == symbol)
                .FirstOrDefaultAsync();

            if (existingSymbol == null)
            {
                return;
            }

            _context.CryptoSymbols.Remove(existingSymbol);
            await _context.SaveChangesAsync();

            // Cache'den veriyi kaldırma
            await _cache.RemoveAsync(CacheKeyAllSymbols);
            await _cache.RemoveAsync($"{CacheKeyPrefixSymbol}{symbol}");
        }
    }
}
