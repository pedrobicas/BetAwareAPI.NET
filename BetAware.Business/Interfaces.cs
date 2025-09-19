using BetAware.Model;

namespace BetAware.Business;

public interface IAuthService
{
    Task<JwtResponse> LoginAsync(LoginRequest request);
    Task RegisterAsync(RegisterRequest request);
}

public interface IApostaService
{
    Task<ApostaDTO> CriarApostaAsync(ApostaDTO apostaDto, string username);
    Task<List<ApostaDTO>> ListarApostasPorUsuarioAsync(string username);
    Task<List<ApostaDTO>> ListarApostasPorPeriodoAsync(DateTime inicio, DateTime fim);
    Task<List<ApostaDTO>> ListarApostasPorUsuarioEPeriodoAsync(string username, DateTime inicio, DateTime fim);
}

public interface IJwtTokenService
{
    string GenerateToken(Usuario usuario);
}
