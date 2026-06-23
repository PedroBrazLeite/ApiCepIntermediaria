using MinhaApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MinhaApi.Controllers;
[ApiController]
[Route("ceps")]
public class CepsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCeps([FromQuery] string ceps)
    {
        string?[] cepsValidos = ceps.Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(CepService.ProcessaCep)
            .Where(item => item != null)
            .ToArray();
       
        if (cepsValidos.Length == 0)
            return  BadRequest("Nenhum CEP válido informado.");
        
        if (cepsValidos.Length > 10)
            return BadRequest("Máximo de 10 CEPs por requisição.");
        
      
        try
        {
            var resultados = await CepService
                .BuscarCepsAsync(cepsValidos!);
            var encontrados = resultados.Where(r => r.Sucesso).Select(r => r.Resultado);

            if (!encontrados.Any())
                return NotFound("Nenhum CEP encontrado.");

            return Ok(encontrados);
        }
        catch (TaskCanceledException)
        {
            return BadRequest("tempo excedido");
        }
     
    }

    [HttpGet("{cep}")]
    public async Task<IActionResult> GetCep([FromRoute] string cep)
    {
        string? cepValido = CepService.ProcessaCep(cep);

        if (cepValido == null)
        {
            return  BadRequest("Nenhum CEP válido informado.");
        }

        try
        {
            var resultado = await CepService.BuscarCepAsync(cepValido);
            
            if (!resultado.Sucesso)
                return NotFound($"CEP {cepValido} não encontrado.");

            return Ok(resultado.Resultado);
        }
        catch (TaskCanceledException)
        {
            return BadRequest("tempo excedido");
        }
        
    }
}