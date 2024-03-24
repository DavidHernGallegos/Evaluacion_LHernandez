using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DL;

public partial class PruebaTecnicaContext : DbContext
{
    public PruebaTecnicaContext()
    {
    }

    public PruebaTecnicaContext(DbContextOptions<PruebaTecnicaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Resuman> Resumen { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.; Database= PruebaTecnica; Trusted_Connection=True; User ID=sa; Password=pass@word1; TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Resuman>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.IdRegistradora)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Id_Registradora");
            entity.Property(e => e.IdTienda)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Id_Tienda");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable(tb => tb.HasTrigger("Insertar_Resumen"));

            entity.Property(e => e.FechaHora).HasColumnType("datetime");
            entity.Property(e => e.FechaHoraCreacion).HasColumnName("FechaHora_Creacion");
            entity.Property(e => e.IdRegistradora)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Id_Registradora");
            entity.Property(e => e.IdTienda)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Id_Tienda");
            entity.Property(e => e.Inpuesto).HasColumnType("money");
            entity.Property(e => e.Ticket1).HasColumnName("Ticket");
            entity.Property(e => e.Total).HasColumnType("money");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
