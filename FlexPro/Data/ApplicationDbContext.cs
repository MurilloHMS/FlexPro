using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FlexPro.Models;

namespace FlexPro.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<FlexPro.Models.Veiculo> Veiculo { get; set; } = default!;
        public DbSet<Abastecimento> Abastecimento { get; set; } = default!;
        public DbSet<Funcionarios> Funcionarios { get; set; } = default!;
        public DbSet<Categoria> Categoria { get; set; } = default!;
        public DbSet<Entidade> Entidade { get; set; } = default!;
        public DbSet<Receita> Receita { get; set; } = default!;
        public DbSet<Revisao> Revisao { get; set; } = default!;
        public DbSet<Produto> Produto { get; set; } = default!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Revisao>()
                .HasOne(r => r.Local)
                .WithMany()
                .HasForeignKey(r => r.LocalId);

            modelBuilder.Entity<Revisao>()
                .HasOne(r => r.Veiculo)
                .WithMany()
                .HasForeignKey(r => r.VeiculoId);

            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Receita)
                .WithMany()
                .HasForeignKey(p => p.IdReceita);
        }
    }
    