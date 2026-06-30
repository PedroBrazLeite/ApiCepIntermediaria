using MinhaApi.Clients;
using MinhaApi.Models;
using Refit;

namespace MinhaApi.Services;

public class CepService : ICepService
{
    private readonly ICep _client;

    public CepService(ICep client)
    {
        _client = client;
    }
    
    public string SanitizarCep(string input)
    {
        return input.Replace("-", "").Trim();
    }
        
    public bool ValidarCep(string input)
    {
        return input.Length == 8 && input.All(char.IsDigit);
    }
        
    public string? ProcessaCep(string input)
    {
        var cleanIput =SanitizarCep(input);
        var validado = ValidarCep(cleanIput);
        if (validado == false)
        {
            return null;
        }
        return cleanIput;
    }
        
    public async Task<(bool Sucesso, string Cep, EnderecoResponse? Resultado)> BuscarCepAsync(string cep)
    {
        try
        {

            var endereco = await _client.ObterPorCepAsync(cep);
            return (true, cep, endereco);
        }
        catch (ApiException ex)
        {
            Console.WriteLine($"Erro de rede: {ex.Message}");

            return (false, cep, null);
        }
    }
        
    public async Task<(bool Sucesso, string Cep, EnderecoResponse? Resultado)[]> BuscarCepsAsync(string[] ceps)
    {
        var tarefas = ceps
            .Select(BuscarCepAsync);
            
        return await Task.WhenAll(tarefas);
    }
}