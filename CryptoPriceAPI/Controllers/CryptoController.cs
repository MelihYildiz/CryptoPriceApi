using CryptoPriceAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CryptoController : ControllerBase
{
    private readonly CryptoDbContext _context;

    public CryptoController(CryptoDbContext context)
    {
        _context = context;
    }

    [HttpGet("symbols")]
    public async Task<IActionResult> GetSymbols()
    {
        var symbols = await _context.CryptoSymbols.ToListAsync();
        return Ok(symbols);
    }

    [HttpGet("{cryptoSymbol}")]
    public async Task<IActionResult> GetPrice(string cryptoSymbol)
    {
        var symbol = await _context.CryptoSymbols
            .FirstOrDefaultAsync(c => c.Symbol.Equals(cryptoSymbol, StringComparison.OrdinalIgnoreCase));
        if (symbol == null)
        {
            return NotFound();
        }

        var randomPrice = new Random().Next(1000, 10000); // Örnek fiyat
        return Ok(new { symbol = symbol.Symbol, price = $"{randomPrice} USD" });
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddSymbol([FromBody] CryptoSymbol newSymbol)
    {
        if (await _context.CryptoSymbols.AnyAsync(c => c.Symbol == newSymbol.Symbol))
        {
            return Conflict("Symbol already exists.");
        }

        _context.CryptoSymbols.Add(newSymbol);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPrice), new { cryptoSymbol = newSymbol.Symbol }, newSymbol);
    }
}
