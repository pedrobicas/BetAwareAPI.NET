using BetAware.Business;
using BetAware.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BetAware.Api.Controllers;

[ApiController]
[Route("v1/apostas")]
[Authorize]
public class ApostaController : ControllerBase
{
    private readonly IApostaService _apostaService;

    public ApostaController(IApostaService apostaService)
    {
        _apostaService = apostaService;
    }

    /// <summary>
    /// Cria uma nova aposta
    /// </summary>
    /// <param name="apostaDto">Dados da aposta</param>
    /// <returns>Aposta criada</returns>
    [HttpPost]
    public async Task<ActionResult<ApostaDTO>> CriarAposta([FromBody] ApostaDTO apostaDto)
    {
        try
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value!;
            var result = await _apostaService.CriarApostaAsync(apostaDto, username);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Obtém uma aposta por ID
    /// </summary>
    /// <param name="id">ID da aposta</param>
    /// <returns>Dados da aposta</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApostaDTO>> ObterAposta(long id)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value!;
        var aposta = await _apostaService.ObterApostaPorIdAsync(id, username);
        
        if (aposta == null)
        {
            return NotFound(new { message = "Aposta não encontrada" });
        }
        
        return Ok(aposta);
    }

    /// <summary>
    /// Atualiza uma aposta existente
    /// </summary>
    /// <param name="id">ID da aposta</param>
    /// <param name="apostaDto">Novos dados da aposta</param>
    /// <returns>Aposta atualizada</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApostaDTO>> AtualizarAposta(long id, [FromBody] ApostaDTO apostaDto)
    {
        try
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value!;
            var result = await _apostaService.AtualizarApostaAsync(id, apostaDto, username);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deleta uma aposta
    /// </summary>
    /// <param name="id">ID da aposta</param>
    /// <returns>Resultado da operação</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarAposta(long id)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value!;
        var sucesso = await _apostaService.DeletarApostaAsync(id, username);
        
        if (!sucesso)
        {
            return NotFound(new { message = "Aposta não encontrada" });
        }
        
        return NoContent();
    }

    /// <summary>
    /// Lista todas as apostas do usuário logado
    /// </summary>
    /// <returns>Lista de apostas</returns>
    [HttpGet]
    public async Task<ActionResult<List<ApostaDTO>>> ListarApostas()
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value!;
        var apostas = await _apostaService.ListarApostasPorUsuarioAsync(username);
        return Ok(apostas);
    }

    /// <summary>
    /// Lista apostas por período (apenas para admins)
    /// </summary>
    /// <param name="inicio">Data de início</param>
    /// <param name="fim">Data de fim</param>
    /// <returns>Lista de apostas no período</returns>
    [HttpGet("periodo")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<List<ApostaDTO>>> ListarApostasPorPeriodo(
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fim)
    {
        var apostas = await _apostaService.ListarApostasPorPeriodoAsync(inicio, fim);
        return Ok(apostas);
    }

    /// <summary>
    /// Lista apostas do usuário por período
    /// </summary>
    /// <param name="inicio">Data de início</param>
    /// <param name="fim">Data de fim</param>
    /// <returns>Lista de apostas do usuário no período</returns>
    [HttpGet("usuario/periodo")]
    public async Task<ActionResult<List<ApostaDTO>>> ListarApostasPorUsuarioEPeriodo(
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fim)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value!;
        var apostas = await _apostaService.ListarApostasPorUsuarioEPeriodoAsync(username, inicio, fim);
        return Ok(apostas);
    }

    /// <summary>
    /// Pesquisa apostas com filtros avançados e paginação
    /// </summary>
    /// <param name="categoria">Filtro por categoria</param>
    /// <param name="jogo">Filtro por jogo</param>
    /// <param name="resultado">Filtro por resultado</param>
    /// <param name="dataInicio">Data de início</param>
    /// <param name="dataFim">Data de fim</param>
    /// <param name="valorMinimo">Valor mínimo</param>
    /// <param name="valorMaximo">Valor máximo</param>
    /// <param name="page">Página (padrão: 1)</param>
    /// <param name="pageSize">Tamanho da página (padrão: 10)</param>
    /// <param name="orderBy">Campo para ordenação (padrão: Data)</param>
    /// <param name="ascending">Ordenação crescente (padrão: false)</param>
    /// <returns>Lista paginada de apostas</returns>
    [HttpGet("pesquisar")]
    [Authorize(Roles = "ADMIN")]
    public async Task<ActionResult<List<ApostaDTO>>> PesquisarApostas(
        [FromQuery] string? categoria = null,
        [FromQuery] string? jogo = null,
        [FromQuery] string? resultado = null,
        [FromQuery] DateTime? dataInicio = null,
        [FromQuery] DateTime? dataFim = null,
        [FromQuery] double? valorMinimo = null,
        [FromQuery] double? valorMaximo = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string orderBy = "Data",
        [FromQuery] bool ascending = false)
    {
        var apostas = await _apostaService.PesquisarApostasAsync(categoria, jogo, resultado, 
            dataInicio, dataFim, valorMinimo, valorMaximo, page, pageSize, orderBy, ascending);
        return Ok(apostas);
    }
}
