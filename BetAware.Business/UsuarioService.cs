using BetAware.Model;
using BetAware.Data;
using Microsoft.EntityFrameworkCore;

namespace BetAware.Business;

public class UsuarioService : IUsuarioService
{
    private readonly ApplicationDbContext _context;

    public UsuarioService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Usuario>> ListarUsuariosAsync()
    {
        return await _context.Usuarios
            .OrderBy(u => u.Nome)
            .ToListAsync();
    }

    public async Task<Usuario?> ObterUsuarioPorIdAsync(long id)
    {
        return await _context.Usuarios
            .Include(u => u.Apostas)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Usuario> AtualizarUsuarioAsync(long id, Usuario usuario)
    {
        var usuarioExistente = await _context.Usuarios.FindAsync(id);
        
        if (usuarioExistente == null)
        {
            throw new InvalidOperationException("Usuário não encontrado");
        }

        // Verificar se username ou email já existem em outros usuários
        var existeUsername = await _context.Usuarios
            .AnyAsync(u => u.Id != id && u.Username == usuario.Username);
        
        if (existeUsername)
        {
            throw new InvalidOperationException("Username já está em uso");
        }

        var existeEmail = await _context.Usuarios
            .AnyAsync(u => u.Id != id && u.Email == usuario.Email);
        
        if (existeEmail)
        {
            throw new InvalidOperationException("Email já está em uso");
        }

        usuarioExistente.Username = usuario.Username;
        usuarioExistente.Nome = usuario.Nome;
        usuarioExistente.Cpf = usuario.Cpf;
        usuarioExistente.Cep = usuario.Cep;
        usuarioExistente.Endereco = usuario.Endereco;
        usuarioExistente.Email = usuario.Email;
        usuarioExistente.Perfil = usuario.Perfil;
        
        // Não atualizar senha aqui por segurança
        
        await _context.SaveChangesAsync();
        return usuarioExistente;
    }

    public async Task<bool> DeletarUsuarioAsync(long id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        
        if (usuario == null)
        {
            return false;
        }

        // Verificar se o usuário tem apostas
        var temApostas = await _context.Apostas.AnyAsync(a => a.UsuarioId == id);
        
        if (temApostas)
        {
            throw new InvalidOperationException("Não é possível deletar usuário com apostas associadas");
        }

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Usuario>> PesquisarUsuariosAsync(string? nome = null, string? username = null, 
        string? email = null, string? perfil = null, int page = 1, int pageSize = 10)
    {
        var query = _context.Usuarios.AsQueryable();

        // Aplicar filtros usando LINQ
        if (!string.IsNullOrEmpty(nome))
            query = query.Where(u => u.Nome.Contains(nome));

        if (!string.IsNullOrEmpty(username))
            query = query.Where(u => u.Username.Contains(username));

        if (!string.IsNullOrEmpty(email))
            query = query.Where(u => u.Email.Contains(email));

        if (!string.IsNullOrEmpty(perfil))
            query = query.Where(u => u.Perfil == perfil);

        // Aplicar paginação
        var usuarios = await query
            .OrderBy(u => u.Nome)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return usuarios;
    }
}