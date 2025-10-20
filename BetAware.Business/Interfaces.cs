using BetAware.Model;

namespace BetAware.Business;

public interface IAuthService
{
    Task<JwtResponse> LoginAsync(LoginRequest request);
    Task RegisterAsync(RegisterRequest request);
}

public interface IUsuarioService
{
    Task<List<Usuario>> ListarUsuariosAsync();
    Task<Usuario?> ObterUsuarioPorIdAsync(long id);
    Task<Usuario> AtualizarUsuarioAsync(long id, Usuario usuario);
    Task<bool> DeletarUsuarioAsync(long id);
    Task<List<Usuario>> PesquisarUsuariosAsync(string? nome = null, string? username = null, string? email = null, 
        string? perfil = null, int page = 1, int pageSize = 10);
}

public interface IApostaService
{
    Task<ApostaDTO> CriarApostaAsync(ApostaDTO apostaDto, string username);
    Task<ApostaDTO?> ObterApostaPorIdAsync(long id, string username);
    Task<ApostaDTO> AtualizarApostaAsync(long id, ApostaDTO apostaDto, string username);
    Task<bool> DeletarApostaAsync(long id, string username);
    Task<List<ApostaDTO>> ListarApostasPorUsuarioAsync(string username);
    Task<List<ApostaDTO>> ListarApostasPorPeriodoAsync(DateTime inicio, DateTime fim);
    Task<List<ApostaDTO>> ListarApostasPorUsuarioEPeriodoAsync(string username, DateTime inicio, DateTime fim);
    Task<List<ApostaDTO>> PesquisarApostasAsync(string? categoria = null, string? jogo = null, string? resultado = null, 
        DateTime? dataInicio = null, DateTime? dataFim = null, double? valorMinimo = null, double? valorMaximo = null,
        int page = 1, int pageSize = 10, string orderBy = "Data", bool ascending = false);
}

public interface IExternalApiService
{
    Task<CepResponse?> BuscarCepAsync(string cep);
    Task<CotacaoResponse?> ObterCotacaoMoedaAsync(string moedaOrigem = "USD", string moedaDestino = "BRL");
    Task<List<JogoEsportivoResponse>> ObterJogosEsportivosAsync();
    Task<PrevisaoTempoResponse?> ObterPrevisaoTempoAsync(string cidade);
}

public interface IJwtTokenService
{
    string GenerateToken(Usuario usuario);
}
