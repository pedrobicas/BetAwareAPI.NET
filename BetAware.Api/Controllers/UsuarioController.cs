using BetAware.Business;
using BetAware.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BetAware.Api.Controllers;

[ApiController]
[Route("v1/usuarios")]
[Authorize(Roles = "ADMIN")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Lista todos os usuários (apenas para admins)
    /// </summary>
    /// <returns>Lista de usuários</returns>
    [HttpGet]
    public async Task<ActionResult<List<Usuario>>> ListarUsuarios()
    {
        var usuarios = await _usuarioService.ListarUsuariosAsync();
        
        // Remover senhas da resposta por segurança
        foreach (var usuario in usuarios)
        {
            usuario.Senha = string.Empty;
        }
        
        return Ok(usuarios);
    }

    /// <summary>
    /// Obtém um usuário por ID (apenas para admins)
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <returns>Dados do usuário</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> ObterUsuario(long id)
    {
        var usuario = await _usuarioService.ObterUsuarioPorIdAsync(id);
        
        if (usuario == null)
        {
            return NotFound(new { message = "Usuário não encontrado" });
        }
        
        // Remover senha da resposta por segurança
        usuario.Senha = string.Empty;
        
        return Ok(usuario);
    }

    /// <summary>
    /// Atualiza um usuário existente (apenas para admins)
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <param name="usuario">Novos dados do usuário</param>
    /// <returns>Usuário atualizado</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<Usuario>> AtualizarUsuario(long id, [FromBody] Usuario usuario)
    {
        try
        {
            var result = await _usuarioService.AtualizarUsuarioAsync(id, usuario);
            
            // Remover senha da resposta por segurança
            result.Senha = string.Empty;
            
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deleta um usuário (apenas para admins)
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <returns>Resultado da operação</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarUsuario(long id)
    {
        try
        {
            var sucesso = await _usuarioService.DeletarUsuarioAsync(id);
            
            if (!sucesso)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }
            
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Pesquisa usuários com filtros e paginação (apenas para admins)
    /// </summary>
    /// <param name="nome">Filtro por nome</param>
    /// <param name="username">Filtro por username</param>
    /// <param name="email">Filtro por email</param>
    /// <param name="perfil">Filtro por perfil</param>
    /// <param name="page">Página (padrão: 1)</param>
    /// <param name="pageSize">Tamanho da página (padrão: 10)</param>
    /// <returns>Lista paginada de usuários</returns>
    [HttpGet("pesquisar")]
    public async Task<ActionResult<List<Usuario>>> PesquisarUsuarios(
        [FromQuery] string? nome = null,
        [FromQuery] string? username = null,
        [FromQuery] string? email = null,
        [FromQuery] string? perfil = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var usuarios = await _usuarioService.PesquisarUsuariosAsync(nome, username, email, perfil, page, pageSize);
        
        // Remover senhas da resposta por segurança
        foreach (var usuario in usuarios)
        {
            usuario.Senha = string.Empty;
        }
        
        return Ok(usuarios);
    }
}