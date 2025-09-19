using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetAware.Model;

public class Usuario
{
    [Key]
    public long Id { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [StringLength(255)]
    public string Nome { get; set; } = string.Empty;
    
    [Required]
    [StringLength(11)]
    public string Cpf { get; set; } = string.Empty;
    
    [Required]
    [StringLength(8)]
    public string Cep { get; set; } = string.Empty;
    
    [StringLength(255)]
    public string? Endereco { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Senha { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string Perfil { get; set; } = "USER";
    
    // Navigation property
    public virtual ICollection<Aposta> Apostas { get; set; } = new List<Aposta>();
}

public class Aposta
{
    [Key]
    public long Id { get; set; }
    
    [Required]
    [ForeignKey("Usuario")]
    public long UsuarioId { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Categoria { get; set; } = string.Empty;
    
    [Required]
    [StringLength(255)]
    public string Jogo { get; set; } = string.Empty;
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public double Valor { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Resultado { get; set; } = "PENDENTE";
    
    [Required]
    public DateTime Data { get; set; }
    
    // Navigation property
    public virtual Usuario Usuario { get; set; } = null!;
}

public enum ResultadoAposta
{
    GANHOU,
    PERDEU,
    PENDENTE,
    CANCELADA
}

public enum Perfil
{
    USER,
    ADMIN
}
