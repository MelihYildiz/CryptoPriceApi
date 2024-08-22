using System.ComponentModel.DataAnnotations;

public class CryptoSymbol
{
    public int Id { get; set; }

    [Required]
    [ValidSymbol] // Sembol için özel doğrulama
    public string Symbol { get; set; }

    [Required]
    [ValidName] // Name için özel doğrulama
    public string Name { get; set; }

    public decimal Price { get; set; }
}
