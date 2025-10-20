using BetAware.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BetAware.Api.Controllers;

[ApiController]
[Route("v1/external")]
[Authorize]
public class ExternalApiController : ControllerBase
{
    private readonly IExternalApiService _externalApiService;

    public ExternalApiController(IExternalApiService externalApiService)
    {
        _externalApiService = externalApiService;
    }

    /// <summary>
    /// Busca informações de endereço por CEP
    /// </summary>
    /// <param name="cep">CEP para consulta (8 dígitos)</param>
    /// <returns>Informações do endereço</returns>
    [HttpGet("cep/{cep}")]
    public async Task<ActionResult<CepResponse>> BuscarCep(string cep)
    {
        try
        {
            var resultado = await _externalApiService.BuscarCepAsync(cep);
            
            if (resultado == null)
            {
                return NotFound(new { message = "CEP não encontrado" });
            }
            
            return Ok(resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Obtém cotação de moedas
    /// </summary>
    /// <param name="origem">Moeda de origem (padrão: USD)</param>
    /// <param name="destino">Moeda de destino (padrão: BRL)</param>
    /// <returns>Cotação atual</returns>
    [HttpGet("cotacao")]
    public async Task<ActionResult<CotacaoResponse>> ObterCotacao(
        [FromQuery] string origem = "USD", 
        [FromQuery] string destino = "BRL")
    {
        try
        {
            var resultado = await _externalApiService.ObterCotacaoMoedaAsync(origem, destino);
            
            if (resultado == null)
            {
                return NotFound(new { message = "Cotação não encontrada" });
            }
            
            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Lista jogos esportivos disponíveis para apostas
    /// </summary>
    /// <returns>Lista de jogos com odds</returns>
    [HttpGet("jogos")]
    public async Task<ActionResult<List<JogoEsportivoResponse>>> ObterJogosEsportivos()
    {
        try
        {
            var jogos = await _externalApiService.ObterJogosEsportivosAsync();
            return Ok(jogos);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Obtém previsão do tempo para uma cidade
    /// </summary>
    /// <param name="cidade">Nome da cidade</param>
    /// <returns>Previsão do tempo atual</returns>
    [HttpGet("tempo/{cidade}")]
    public async Task<ActionResult<PrevisaoTempoResponse>> ObterPrevisaoTempo(string cidade)
    {
        try
        {
            var resultado = await _externalApiService.ObterPrevisaoTempoAsync(cidade);
            
            if (resultado == null)
            {
                return NotFound(new { message = "Previsão não encontrada para a cidade informada" });
            }
            
            return Ok(resultado);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }

    /// <summary>
    /// Endpoint para demonstrar integração com múltiplas APIs
    /// </summary>
    /// <param name="cep">CEP do usuário</param>
    /// <returns>Informações combinadas de localização, tempo e jogos</returns>
    [HttpGet("dashboard/{cep}")]
    public async Task<ActionResult<object>> ObterDashboard(string cep)
    {
        try
        {
            // Buscar informações do CEP
            var infoCep = await _externalApiService.BuscarCepAsync(cep);
            
            if (infoCep == null)
            {
                return BadRequest(new { message = "CEP inválido" });
            }

            // Buscar previsão do tempo para a cidade
            var previsaoTempo = await _externalApiService.ObterPrevisaoTempoAsync(infoCep.Localidade);
            
            // Buscar jogos disponíveis
            var jogos = await _externalApiService.ObterJogosEsportivosAsync();
            
            // Buscar cotação USD/BRL
            var cotacao = await _externalApiService.ObterCotacaoMoedaAsync();

            var dashboard = new
            {
                Localizacao = new
                {
                    Cidade = infoCep.Localidade,
                    Estado = infoCep.Uf,
                    Bairro = infoCep.Bairro
                },
                Tempo = previsaoTempo,
                JogosDisponiveis = jogos.Take(3), // Apenas os 3 primeiros
                CotacaoUSD = cotacao,
                DataConsulta = DateTime.Now
            };

            return Ok(dashboard);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Erro interno do servidor" });
        }
    }
}