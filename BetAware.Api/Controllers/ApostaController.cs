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

    [HttpGet]
    public async Task<ActionResult<List<ApostaDTO>>> ListarApostas()
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value!;
        var apostas = await _apostaService.ListarApostasPorUsuarioAsync(username);
        return Ok(apostas);
    }

    [HttpGet("periodo")]
    public async Task<ActionResult<List<ApostaDTO>>> ListarApostasPorPeriodo(
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fim)
    {
        var apostas = await _apostaService.ListarApostasPorPeriodoAsync(inicio, fim);
        return Ok(apostas);
    }

    [HttpGet("usuario/periodo")]
    public async Task<ActionResult<List<ApostaDTO>>> ListarApostasPorUsuarioEPeriodo(
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fim)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value!;
        var apostas = await _apostaService.ListarApostasPorUsuarioEPeriodoAsync(username, inicio, fim);
        return Ok(apostas);
    }
}
