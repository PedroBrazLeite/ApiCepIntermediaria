using MinhaApi.Models;
using Refit;

namespace MinhaApi.Clients;

public interface ICep
{
    [Get("/v1/{cep}")]
    Task<EnderecoResponse> ObterPorCepAsync(string cep);
}