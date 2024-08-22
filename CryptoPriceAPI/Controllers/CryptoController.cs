using CryptoPriceAPI.DTOs;
using CryptoPriceAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoPriceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoController : ControllerBase
    {
        private readonly ICryptoRepository _cryptoRepository;

        public CryptoController(ICryptoRepository cryptoRepository)
        {
            _cryptoRepository = cryptoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CryptoSymbolDTO>>> Get()
        { 
            var symbols = await _cryptoRepository.GetAllSymbolsAsync();
            return Ok(symbols);
        }

        [HttpGet("{symbol}")]
        public async Task<ActionResult<CryptoSymbolDTO>> Get(string symbol)
        {
            var cryptoSymbol = await _cryptoRepository.GetSymbolByNameAsync(symbol);
            if (cryptoSymbol == null)
            {
                return NotFound();
            }
            return Ok(cryptoSymbol);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CryptoSymbolDTO newSymbolDTO)
        {
            if (newSymbolDTO == null)
            {
                return BadRequest();
            }

            await _cryptoRepository.AddSymbolAsync(newSymbolDTO);
            return CreatedAtAction(nameof(Get), new { symbol = newSymbolDTO.Symbol }, newSymbolDTO);
        }

        [HttpPut("{symbol}")]
        public async Task<ActionResult> Put(string symbol, [FromBody] CryptoSymbolDTO updatedSymbolDTO)
        {
            if (symbol != updatedSymbolDTO.Symbol)
            {
                return BadRequest();
            }

            await _cryptoRepository.UpdateSymbolAsync(updatedSymbolDTO);
            return NoContent();
        }

        [HttpDelete("{symbol}")]
        public async Task<ActionResult> Delete(string symbol)
        {
            await _cryptoRepository.DeleteSymbolAsync(symbol);
            return NoContent();
        }
    }
}
