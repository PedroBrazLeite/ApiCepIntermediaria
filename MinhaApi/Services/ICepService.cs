using MinhaApi.Models;

namespace MinhaApi.Services;

public interface ICepService
{
    string? ProcessaCep(string cep);
    
    Task<(bool Sucesso, string Cep, EnderecoResponse? Resultado)> BuscarCepAsync(string cep);
    
    Task<(bool Sucesso, string Cep, EnderecoResponse? Resultado)[]> BuscarCepsAsync(string[] ceps);
}