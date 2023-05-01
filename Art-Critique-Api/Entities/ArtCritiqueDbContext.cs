using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Art_Critique_Api.Entities;

public partial class ArtCritiqueDbContext : DbContext
{
    public ArtCritiqueDbContext()
    {
    }

    public ArtCritiqueDbContext(DbContextOptions<ArtCritiqueDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TProfile> TProfiles { get; set; }

    public virtual DbSet<TUser> TUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=Niewiem123;database=art-critique-db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PRIMARY");

            entity.ToTable("t_profile");

            entity.HasIndex(e => e.ProfileId, "profileID_UNIQUE").IsUnique();

            entity.HasIndex(e => e.UsId, "usID_UNIQUE").IsUnique();

            entity.Property(e => e.ProfileId).HasColumnName("profileID");
            entity.Property(e => e.ProfileAvatarId)
                .HasDefaultValueSql("'1'")
                .HasColumnName("profileAvatarID");
            entity.Property(e => e.ProfileBirthdate)
                .HasColumnType("datetime")
                .HasColumnName("profileBirthdate");
            entity.Property(e => e.ProfileDescription)
                .HasMaxLength(400)
                .HasColumnName("profileDescription");
            entity.Property(e => e.ProfileFacebook)
                .HasMaxLength(100)
                .HasColumnName("profileFacebook");
            entity.Property(e => e.ProfileInstagram)
                .HasMaxLength(100)
                .HasColumnName("profileInstagram");
            entity.Property(e => e.ProfileName)
                .HasMaxLength(100)
                .HasColumnName("profileName");
            entity.Property(e => e.ProfileTwitter)
                .HasMaxLength(100)
                .HasColumnName("profileTwitter");
            entity.Property(e => e.UsId).HasColumnName("usID");

            entity.HasOne(d => d.Us).WithOne(p => p.TProfile)
                .HasForeignKey<TProfile>(d => d.UsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_usID");
        });

        modelBuilder.Entity<TUser>(entity =>
        {
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
