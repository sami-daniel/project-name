﻿using Microsoft.EntityFrameworkCore;

namespace Entidades;

public partial class SystemCardapioContext : DbContext
{
    public SystemCardapioContext()
    {
    }

    public SystemCardapioContext(DbContextOptions<SystemCardapioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Endereco> Enderecos { get; set; }

    public virtual DbSet<Restaurante> Restaurantes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=System_Cardapio;uid=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.2.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Endereco>(entity =>
        {
            entity.HasKey(e => e.IdEndereco).HasName("PRIMARY");

            entity.Property(e => e.Uf).IsFixedLength();
        });

        modelBuilder.Entity<Restaurante>(entity =>
        {
            entity.HasKey(e => e.Cnpj).HasName("PRIMARY");

            entity.Property(e => e.Cnpj).IsFixedLength();

            entity.HasOne(d => d.FkIdEnderecoNavigation).WithMany(p => p.Restaurantes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("restaurante_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}