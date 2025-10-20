using BetAware.Model;
using BetAware.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

    public async Task<ApostaDTO?> ObterApostaPorIdAsync(long id, string username)
    {
        var aposta = await _context.Apostas
            .Include(a => a.Usuario)
            .FirstOrDefaultAsync(a => a.Id == id && a.Usuario.Username == username);

        return aposta != null ? MapToDto(aposta) : null;
    }

    public async Task<ApostaDTO> AtualizarApostaAsync(long id, ApostaDTO apostaDto, string username)
    {
        var aposta = await _context.Apostas
            .Include(a => a.Usuario)
            .FirstOrDefaultAsync(a => a.Id == id && a.Usuario.Username == username);

        if (aposta == null)
        {
            throw new InvalidOperationException("Aposta não encontrada ou não pertence ao usuário");
        }

        aposta.Categoria = apostaDto.Categoria;
        aposta.Jogo = apostaDto.Jogo;
        aposta.Valor = apostaDto.Valor;
        aposta.Resultado = apostaDto.Resultado;
        aposta.Data = apostaDto.Data;

        await _context.SaveChangesAsync();

        return MapToDto(aposta);
    }

    public async Task<bool> DeletarApostaAsync(long id, string username)
    {
        var aposta = await _context.Apostas
            .Include(a => a.Usuario)
            .FirstOrDefaultAsync(a => a.Id == id && a.Usuario.Username == username);

        if (aposta == null)
        {
            return false;
        }

        _context.Apostas.Remove(aposta);
        await _context.SaveChangesAsync();
        return true;
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

    public async Task<List<ApostaDTO>> PesquisarApostasAsync(string? categoria = null, string? jogo = null, string? resultado = null,
        DateTime? dataInicio = null, DateTime? dataFim = null, double? valorMinimo = null, double? valorMaximo = null,
        int page = 1, int pageSize = 10, string orderBy = "Data", bool ascending = false)
    {
        var query = _context.Apostas.Include(a => a.Usuario).AsQueryable();

        // Aplicar filtros usando LINQ
        if (!string.IsNullOrEmpty(categoria))
            query = query.Where(a => a.Categoria.Contains(categoria));

        if (!string.IsNullOrEmpty(jogo))
            query = query.Where(a => a.Jogo.Contains(jogo));

        if (!string.IsNullOrEmpty(resultado))
            query = query.Where(a => a.Resultado == resultado);

        if (dataInicio.HasValue)
            query = query.Where(a => a.Data >= dataInicio.Value);

        if (dataFim.HasValue)
            query = query.Where(a => a.Data <= dataFim.Value);

        if (valorMinimo.HasValue)
            query = query.Where(a => a.Valor >= valorMinimo.Value);

        if (valorMaximo.HasValue)
            query = query.Where(a => a.Valor <= valorMaximo.Value);

        // Aplicar ordenação
        query = orderBy.ToLower() switch
        {
            "categoria" => ascending ? query.OrderBy(a => a.Categoria) : query.OrderByDescending(a => a.Categoria),
            "jogo" => ascending ? query.OrderBy(a => a.Jogo) : query.OrderByDescending(a => a.Jogo),
            "valor" => ascending ? query.OrderBy(a => a.Valor) : query.OrderByDescending(a => a.Valor),
            "resultado" => ascending ? query.OrderBy(a => a.Resultado) : query.OrderByDescending(a => a.Resultado),
            _ => ascending ? query.OrderBy(a => a.Data) : query.OrderByDescending(a => a.Data)
        };

        // Aplicar paginação
        var apostas = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
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
