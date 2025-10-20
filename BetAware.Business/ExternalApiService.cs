using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace BetAware.Business;

public class ExternalApiService : IExternalApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExternalApiService> _logger;

    public ExternalApiService(HttpClient httpClient, ILogger<ExternalApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<CepResponse?> BuscarCepAsync(string cep)
    {
        try
        {
            // Remover caracteres não numéricos do CEP
            cep = new string(cep.Where(char.IsDigit).ToArray());
            
            if (cep.Length != 8)
            {
                throw new ArgumentException("CEP deve conter 8 dígitos");
            }

            var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
            
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var cepData = JsonSerializer.Deserialize<CepResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return cepData?.Erro != true ? cepData : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar CEP: {Cep}", cep);
            return null;
        }
    }

    public async Task<CotacaoResponse?> ObterCotacaoMoedaAsync(string moedaOrigem = "USD", string moedaDestino = "BRL")
    {
        try
        {
            // Usando API gratuita do ExchangeRate-API
            var response = await _httpClient.GetAsync($"https://api.exchangerate-api.com/v4/latest/{moedaOrigem}");
            
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var cotacaoData = JsonSerializer.Deserialize<ExchangeRateResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (cotacaoData?.Rates?.ContainsKey(moedaDestino) == true)
            {
                return new CotacaoResponse
                {
                    MoedaOrigem = moedaOrigem,
                    MoedaDestino = moedaDestino,
                    Taxa = cotacaoData.Rates[moedaDestino],
                    DataAtualizacao = DateTime.UtcNow
                };
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter cotação: {MoedaOrigem} -> {MoedaDestino}", moedaOrigem, moedaDestino);
            return null;
        }
    }

    public async Task<List<JogoEsportivoResponse>> ObterJogosEsportivosAsync()
    {
        try
        {
            // Simulando dados de jogos esportivos (em produção, usar API real como ESPN, etc.)
            await Task.Delay(500); // Simular latência de API

            var jogos = new List<JogoEsportivoResponse>
            {
                new JogoEsportivoResponse
                {
                    Id = 1,
                    Categoria = "Futebol",
                    TimeCasa = "Flamengo",
                    TimeVisitante = "Palmeiras",
                    DataJogo = DateTime.Now.AddDays(1),
                    OddsCasa = 2.1m,
                    OddsEmpate = 3.2m,
                    OddsVisitante = 3.8m,
                    Status = "Agendado"
                },
                new JogoEsportivoResponse
                {
                    Id = 2,
                    Categoria = "Basquete",
                    TimeCasa = "Lakers",
                    TimeVisitante = "Warriors",
                    DataJogo = DateTime.Now.AddDays(2),
                    OddsCasa = 1.9m,
                    OddsEmpate = null, // Basquete não tem empate
                    OddsVisitante = 2.0m,
                    Status = "Agendado"
                },
                new JogoEsportivoResponse
                {
                    Id = 3,
                    Categoria = "Tênis",
                    TimeCasa = "Djokovic",
                    TimeVisitante = "Nadal",
                    DataJogo = DateTime.Now.AddHours(6),
                    OddsCasa = 1.7m,
                    OddsEmpate = null,
                    OddsVisitante = 2.3m,
                    Status = "Agendado"
                }
            };

            return jogos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter jogos esportivos");
            return new List<JogoEsportivoResponse>();
        }
    }

    public async Task<PrevisaoTempoResponse?> ObterPrevisaoTempoAsync(string cidade)
    {
        try
        {
            // Simulando dados de previsão do tempo (em produção, usar API real como OpenWeatherMap)
            await Task.Delay(300); // Simular latência de API

            var random = new Random();
            var temperaturas = new[] { 18, 22, 25, 28, 31, 24, 20 };
            var condicoes = new[] { "Ensolarado", "Parcialmente nublado", "Nublado", "Chuvoso", "Tempestade" };

            return new PrevisaoTempoResponse
            {
                Cidade = cidade,
                Temperatura = temperaturas[random.Next(temperaturas.Length)],
                Condicao = condicoes[random.Next(condicoes.Length)],
                Umidade = random.Next(40, 90),
                DataConsulta = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter previsão do tempo para: {Cidade}", cidade);
            return null;
        }
    }
}

// DTOs para APIs externas
public class CepResponse
{
    public string Cep { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Complemento { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Localidade { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public string Ibge { get; set; } = string.Empty;
    public string Gia { get; set; } = string.Empty;
    public string Ddd { get; set; } = string.Empty;
    public string Siafi { get; set; } = string.Empty;
    public bool Erro { get; set; }
}

public class CotacaoResponse
{
    public string MoedaOrigem { get; set; } = string.Empty;
    public string MoedaDestino { get; set; } = string.Empty;
    public decimal Taxa { get; set; }
    public DateTime DataAtualizacao { get; set; }
}

public class ExchangeRateResponse
{
    public string Base { get; set; } = string.Empty;
    public Dictionary<string, decimal> Rates { get; set; } = new();
}

public class JogoEsportivoResponse
{
    public int Id { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public string TimeCasa { get; set; } = string.Empty;
    public string TimeVisitante { get; set; } = string.Empty;
    public DateTime DataJogo { get; set; }
    public decimal OddsCasa { get; set; }
    public decimal? OddsEmpate { get; set; }
    public decimal OddsVisitante { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class PrevisaoTempoResponse
{
    public string Cidade { get; set; } = string.Empty;
    public int Temperatura { get; set; }
    public string Condicao { get; set; } = string.Empty;
    public int Umidade { get; set; }
    public DateTime DataConsulta { get; set; }
}