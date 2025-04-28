using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StargateAPI.Models;

public partial class AstroActsContext : DbContext
{
    public AstroActsContext()
    {
    }

    public AstroActsContext(DbContextOptions<AstroActsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AstronautDetail> AstronautDetails { get; set; }

    public virtual DbSet<AstronautDuty> AstronautDuties { get; set; }

    public virtual DbSet<DutyTitle> DutyTitles { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Rank> Ranks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=JAMISON-DELL;Initial Catalog=AstroActs;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AstronautDetail>(entity =>
        {
            entity.ToTable("AstronautDetail");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DutyTitleId).HasColumnName("DutyTitleID");
            entity.Property(e => e.PersonId).HasColumnName("PersonID");
            entity.Property(e => e.RankId).HasColumnName("RankID");

            entity.HasOne(d => d.DutyTitle).WithOne(p => p.AstronautDetails)
                .HasForeignKey<AstronautDetail>(d => d.DutyTitleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AstronautDetail_DutyTitle");

            //entity.HasOne(d => d.Person).WithOne(p => p.AstronautDetails)
            //    .HasForeignKey<AstronautDetail>(d => d.PersonId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_AstronautDetail_Person");

            entity.HasOne(d => d.Rank).WithOne(p => p.AstronautDetails)
                .HasForeignKey<AstronautDetail>(d => d.RankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AstronautDetail_Rank");
        });

        modelBuilder.Entity<AstronautDuty>(entity =>
        {
            entity.ToTable("AstronautDuty");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DutyTitleId).HasColumnName("DutyTitleID");
            entity.Property(e => e.PersonId).HasColumnName("PersonID");
            entity.Property(e => e.RankId).HasColumnName("RankID");

            entity.HasOne(d => d.DutyTitle).WithMany(p => p.AstronautDuties)
                .HasForeignKey(d => d.DutyTitleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AstronautDuty_DutyTitle");

            //entity.HasOne(d => d.Person).WithMany(p => p.AstronautDuties)
            //    .HasForeignKey(d => d.PersonId)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_AstronautDuty_Person");

            entity.HasOne(d => d.Rank).WithMany(p => p.AstronautDuties)
                .HasForeignKey(d => d.RankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AstronautDuty_Rank");
        });

        modelBuilder.Entity<DutyTitle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Duty");

            entity.ToTable("DutyTitle");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Person");

            entity.HasIndex(e => new { e.FirstName, e.LastName }, "UQ_FullName").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Rank>(entity =>
        {
            entity.ToTable("Rank");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
