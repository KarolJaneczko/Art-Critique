using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Art_Critique_Api.Entities;

public partial class ArtCritiqueDbContext : DbContext {
    public ArtCritiqueDbContext() {
    }

    public ArtCritiqueDbContext(DbContextOptions<ArtCritiqueDbContext> options)
        : base(options) {
    }

    public virtual DbSet<TUser> TUser { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=Niewiem123;database=art-critique-db");

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<TUser>(entity => {
            entity.HasKey(e => e.UsId).HasName("PRIMARY");

            entity.ToTable("t_user");

            entity.HasIndex(e => e.UsEmail, "usEmail_UNIQUE").IsUnique();

            entity.HasIndex(e => e.UsId, "usID_UNIQUE").IsUnique();

            entity.HasIndex(e => e.UsLogin, "usLogin").IsUnique();

            entity.Property(e => e.UsId).HasColumnName("usID");
            entity.Property(e => e.UsDateCreated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("usDateCreated");
            entity.Property(e => e.UsEmail)
                .HasMaxLength(100)
                .HasColumnName("usEmail");
            entity.Property(e => e.UsLogin)
                .HasMaxLength(30)
                .HasColumnName("usLogin");
            entity.Property(e => e.UsPassword)
                .HasMaxLength(30)
                .HasColumnName("usPassword");
            entity.Property(e => e.UsSignedIn).HasColumnName("usSignedIn");
            entity.Property(e => e.UsSignedInToken)
                .HasMaxLength(30)
                .HasColumnName("usSignedInToken");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
