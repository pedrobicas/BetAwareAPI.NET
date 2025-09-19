using System.ComponentModel.DataAnnotations;

namespace BetAware.Model;

public class LoginRequest
{
    [Required(ErrorMessage = "O username é obrigatório")]
    public string Username { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "A senha é obrigatória")]
    public string Senha { get; set; } = string.Empty;
}

public class RegisterRequest
{
    [Required(ErrorMessage = "O username é obrigatório")]
    public string Username { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "O CPF é obrigatório")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter 11 dígitos")]
    public string Cpf { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "O CEP é obrigatório")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "CEP deve conter 8 dígitos")]
    public string Cep { get; set; } = string.Empty;
    
    public string? Endereco { get; set; }
    
    [Required(ErrorMessage = "A senha é obrigatória")]
    public string Senha { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;
}

public class JwtResponse
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Perfil { get; set; } = string.Empty;
}

public class ApostaDTO
{
    public long? Id { get; set; }
    
    [Required(ErrorMessage = "A categoria é obrigatória")]
    public string Categoria { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "O jogo é obrigatório")]
    public string Jogo { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "O valor é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser positivo")]
    public double Valor { get; set; }
    
    [Required(ErrorMessage = "O resultado é obrigatório")]
    public string Resultado { get; set; } = "PENDENTE";
    
    [Required(ErrorMessage = "A data é obrigatória")]
    public DateTime Data { get; set; }
    
    public string? Username { get; set; }
}
