using System.ComponentModel.DataAnnotations;

namespace CryptoPriceAPI.DTOs
{
    public class CryptoSymbolDTO
    {
        public int Id { get; set; }

        [Required]
        [ValidSymbol]
        public string Symbol { get; set; }

        [Required]
        [ValidName]
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}

