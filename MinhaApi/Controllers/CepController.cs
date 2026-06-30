using MinhaApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MinhaApi.Controllers;

[ApiController]
[Route("ceps")]
public class CepsController : ControllerBase
{
    private readonly ICepService _cepService;

    public CepsController(ICepService cepService)
    {
        _cepService = cepService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCepsAsync([FromQuery(Name = "cep[]")] string[]? cep,CancellationToken cancellationToken)
    {
        if (cep == null || cep.Length == 0)
            return BadRequest("Nenhum CEP informado.");
        
        string?[] cepsValidos = cep
            .Select(_cepService.ProcessaCep)
            .Where(item => item != null)
            .ToArray();
       
        if (cepsValidos.Length == 0)
            return  BadRequest("Nenhum CEP válido informado.");
        
        if (cepsValidos.Length > 10)
            return BadRequest("Máximo de 10 CEPs por requisição.");
        
        try
        {
            var tarefas = cepsValidos.Select(cepValido  => _cepService.BuscarCepAsync(cepValido!, cancellationToken));
           
            var resultados = await Task.WhenAll(tarefas);
            
            var encontrados = resultados
                .Where(resposta => resposta.Sucesso)
                .Select(resposta => resposta.Resultado);
            
            return Ok(encontrados);
        }
        catch (TaskCanceledException)
        {
            return BadRequest("Tempo excedido ou requisição cancelada pelo usuário.");
        }
     
    }

    [HttpGet("{cep}")]
    public async Task<IActionResult> GetCepAsync([FromRoute] string cep, CancellationToken cancellationToken)
    {
        string? cepValido = _cepService.ProcessaCep(cep);

        if (cepValido == null)
        {
            return  BadRequest("Nenhum CEP válido informado.");
        }

        try
        {
            var resultado = await _cepService.BuscarCepAsync(cepValido,cancellationToken);
            
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