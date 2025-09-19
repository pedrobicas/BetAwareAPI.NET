using BetAware.Model;
using BetAware.Data;
using Microsoft.EntityFrameworkCore;

namespace BetAware.Business;

public class ApostaService : IApostaService
{
    private readonly ApplicationDbContext _context;

    public ApostaService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApostaDTO> CriarApostaAsync(ApostaDTO apostaDto, string username)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Username == username);

        if (usuario == null)
        {
            throw new InvalidOperationException("Usuário não encontrado");
        }

        var aposta = new Aposta
        {
            UsuarioId = usuario.Id,
            Categoria = apostaDto.Categoria,
            Jogo = apostaDto.Jogo,
            Valor = apostaDto.Valor,
            Resultado = apostaDto.Resultado,
            Data = apostaDto.Data
        };

        _context.Apostas.Add(aposta);
        await _context.SaveChangesAsync();

        return new ApostaDTO
        {
            Id = aposta.Id,
            Categoria = aposta.Categoria,
            Jogo = aposta.Jogo,
            Valor = aposta.Valor,
            Resultado = aposta.Resultado,
            Data = aposta.Data,
            Username = username
        };
    }

    public async Task<List<ApostaDTO>> ListarApostasPorUsuarioAsync(string username)
    {
        var apostas = await _context.Apostas
            .Include(a => a.Usuario)
            .Where(a => a.Usuario.Username == username)
            .OrderByDescending(a => a.Data)
            .ToListAsync();

        return apostas.Select(MapToDto).ToList();
    }

    public async Task<List<ApostaDTO>> ListarApostasPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        var apostas = await _context.Apostas
            .Include(a => a.Usuario)
            .Where(a => a.Data >= inicio && a.Data <= fim)
            .OrderByDescending(a => a.Data)
            .ToListAsync();

        return apostas.Select(MapToDto).ToList();
    }

    public async Task<List<ApostaDTO>> ListarApostasPorUsuarioEPeriodoAsync(string username, DateTime inicio, DateTime fim)
    {
        var apostas = await _context.Apostas
            .Include(a => a.Usuario)
            .Where(a => a.Usuario.Username == username && a.Data >= inicio && a.Data <= fim)
            .OrderByDescending(a => a.Data)
            .ToListAsync();

        return apostas.Select(MapToDto).ToList();
    }

    private ApostaDTO MapToDto(Aposta aposta)
    {
        return new ApostaDTO
        {
            Id = aposta.Id,
            Categoria = aposta.Categoria,
            Jogo = aposta.Jogo,
            Valor = aposta.Valor,
            Resultado = aposta.Resultado,
            Data = aposta.Data,
            Username = aposta.Usuario.Username
        };
    }
}
