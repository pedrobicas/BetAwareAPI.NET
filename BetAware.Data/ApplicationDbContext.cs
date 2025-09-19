using BetAware.Model;
using Microsoft.EntityFrameworkCore;

namespace BetAware.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Aposta> Apostas { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Cpf).IsUnique();
                
                entity.Property(e => e.Username).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Cpf).IsRequired().HasMaxLength(11);
                entity.Property(e => e.Cep).IsRequired().HasMaxLength(8);
                entity.Property(e => e.Endereco).HasMaxLength(255);
                entity.Property(e => e.Senha).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Perfil).IsRequired().HasMaxLength(50).HasDefaultValue("USER");
            });
            
            modelBuilder.Entity<Aposta>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UsuarioId);
                entity.HasIndex(e => e.Data);
                entity.HasIndex(e => e.Categoria);
                
                entity.Property(e => e.Categoria).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Jogo).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Valor).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Resultado).IsRequired().HasMaxLength(50).HasDefaultValue("PENDENTE");
                entity.Property(e => e.Data).IsRequired();
                
                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Apostas)
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
