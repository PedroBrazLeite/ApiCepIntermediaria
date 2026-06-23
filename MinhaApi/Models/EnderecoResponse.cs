using System.Text.Json.Serialization;

namespace MinhaApi.Models;

public class EnderecoResponse
{

    [JsonPropertyName("cep")] public string? Cep { get; init; }
    [JsonPropertyName("logradouro")] public string? Logradouro { get; init; }
    [JsonPropertyName("bairro")] public string? Bairro { get; init; }
    [JsonPropertyName("localidade")] public string? Localidade { get; init; }
    [JsonPropertyName("uf")] public string? Uf { get; init; }

    public override string ToString()
    {
        if ( string.IsNullOrEmpty(Logradouro)  || string.IsNullOrEmpty(Bairro))
        {
            return $"CEP: {Cep} | {Localidade}/{Uf}";
        }

        return $"CEP: {Cep} | {Logradouro}, {Bairro} — {Localidade}/{Uf}";
    }
}