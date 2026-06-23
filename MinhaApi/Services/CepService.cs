using MinhaApi.Models;
using Refit;
    
namespace MinhaApi.Services;
    
public static class CepService
{
    public interface ICep
    {
        [Get("/v1/{cep}")]
        Task<EnderecoResponse> ObterPorCep(string cep);
    }
        
    static ICep _client = RestService.For<ICep>("https://opencep.com");
        
    public static string SanitizarCep(string input)
    {
        return input.Replace("-", "").Trim();
    }
        
    public static bool ValidarCep(string input)
    {
        return input.Length == 8 && input.All(char.IsDigit);
    }
        
    public static string? ProcessaCep(string input)
    {
        var cleanIput =SanitizarCep(input);
        var validado = ValidarCep(cleanIput);
        if (validado == false)
        {
            return null;
        }
        return cleanIput;
    }
        
    public static async Task<(bool Sucesso, string Cep, EnderecoResponse? Resultado)> BuscarCepAsync(string cep)
    {
        try
        {

            var endereco = await _client.ObterPorCep(cep);
            return (true, cep, endereco);
        }
        catch (ApiException ex)
        {
            Console.WriteLine($"Erro de rede: {ex.Message}");

            return (false, cep, null);
        }
    }
        
    public static async Task<(bool Sucesso, string Cep, EnderecoResponse? Resultado)[]> BuscarCepsAsync(string[] ceps)
    {
        var tarefas = ceps
            .Select(BuscarCepAsync);
            
        return await Task.WhenAll(tarefas);
    }

 
}