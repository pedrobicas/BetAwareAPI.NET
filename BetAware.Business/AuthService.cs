using BetAware.Model;
using BetAware.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BetAware.Business;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(ApplicationDbContext context, IJwtTokenService jwtTokenService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<JwtResponse> LoginAsync(LoginRequest request)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (usuario == null || !VerifyPassword(request.Senha, usuario.Senha))
        {
            throw new UnauthorizedAccessException("Credenciais inválidas");
        }

        var token = _jwtTokenService.GenerateToken(usuario);

        return new JwtResponse
        {
            Token = token,
            Username = usuario.Username,
            Nome = usuario.Nome,
            Perfil = usuario.Perfil
        };
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        // Verificar se já existe usuário com username, email ou CPF
        // Usando FirstOrDefaultAsync em vez de AnyAsync para evitar problemas com literais booleanos no Oracle
        var existingUser = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Username == request.Username || 
                                     u.Email == request.Email || 
                                     u.Cpf == request.Cpf);

        if (existingUser != null)
        {
            throw new InvalidOperationException("Usuário já existe com estes dados");
        }

        var usuario = new Usuario
        {
            Username = request.Username,
            Nome = request.Nome,
            Cpf = request.Cpf,
            Cep = request.Cep,
            Endereco = request.Endereco,
            Senha = HashPassword(request.Senha),
            Email = request.Email,
            Perfil = "USER"
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == hashedPassword;
    }
}
