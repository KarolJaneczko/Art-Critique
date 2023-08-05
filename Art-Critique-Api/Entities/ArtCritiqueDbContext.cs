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

    public virtual DbSet<TArtworkRating> TArtworkRatings { get; set; }

    public virtual DbSet<TArtworkReview> TArtworkReviews { get; set; }

    public virtual DbSet<TAvatar> TAvatars { get; set; }

    public virtual DbSet<TCustomPainting> TCustomPaintings { get; set; }

    public virtual DbSet<TPaintingGenre> TPaintingGenres { get; set; }

    public virtual DbSet<TProfile> TProfiles { get; set; }

    public virtual DbSet<TUser> TUsers { get; set; }

    public virtual DbSet<TUserArtwork> TUserArtworks { get; set; }

    public virtual DbSet<TUserRegistration> TUserRegistrations { get; set; }

    public virtual DbSet<TView> TViews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=Niewiem123;database=art-critique-db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TArtworkRating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PRIMARY");

            entity.ToTable("t_artwork_rating");

            entity.HasIndex(e => e.RatingId, "ratingId_UNIQUE").IsUnique();

            entity.Property(e => e.RatingId).HasColumnName("ratingId");
            entity.Property(e => e.ArtworkId).HasColumnName("artworkId");
            entity.Property(e => e.RatingValue).HasColumnName("ratingValue");
            entity.Property(e => e.UserId).HasColumnName("userId");
        });

        modelBuilder.Entity<TArtworkReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PRIMARY");

            entity.ToTable("t_artwork_review");

            entity.HasIndex(e => e.ReviewId, "reviewId_UNIQUE").IsUnique();

            entity.Property(e => e.ReviewId).HasColumnName("reviewId");
            entity.Property(e => e.ArtworkId).HasColumnName("artworkId");
            entity.Property(e => e.ReviewContent)
                .HasMaxLength(400)
                .HasColumnName("reviewContent");
            entity.Property(e => e.ReviewDate)
                .HasColumnType("datetime")
                .HasColumnName("reviewDate");
            entity.Property(e => e.ReviewTitle)
                .HasMaxLength(50)
                .HasColumnName("reviewTitle");
            entity.Property(e => e.UserId).HasColumnName("userId");
        });

        modelBuilder.Entity<TAvatar>(entity =>
        {
            entity.HasKey(e => e.AvatarId).HasName("PRIMARY");

            entity.ToTable("t_avatar");

            entity.HasIndex(e => e.AvatarId, "idt_avatar_UNIQUE").IsUnique();

            entity.Property(e => e.AvatarId).HasColumnName("AvatarID");
            entity.Property(e => e.AvatarPath).HasMaxLength(300);
        });

        modelBuilder.Entity<TCustomPainting>(entity =>
        {
            entity.HasKey(e => e.PaintingId).HasName("PRIMARY");

            entity.ToTable("t_custom_painting");

            entity.HasIndex(e => e.ArtworkId, "FK_ArtworkID_idx");

            entity.HasIndex(e => e.PaintingId, "idt_custom_painting_UNIQUE").IsUnique();

            entity.Property(e => e.PaintingId).HasColumnName("PaintingID");
            entity.Property(e => e.ArtworkId).HasColumnName("ArtworkID");
            entity.Property(e => e.PaintingPath).HasMaxLength(300);

            entity.HasOne(d => d.Artwork).WithMany(p => p.TCustomPaintings)
                .HasForeignKey(d => d.ArtworkId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArtworkID");
        });

        modelBuilder.Entity<TPaintingGenre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PRIMARY");

            entity.ToTable("t_painting_genre");

            entity.HasIndex(e => e.GenreId, "idt_painting_genre_UNIQUE").IsUnique();

            entity.Property(e => e.GenreId).HasColumnName("genreID");
            entity.Property(e => e.GenreDescription)
                .HasMaxLength(200)
                .HasColumnName("genreDescription");
            entity.Property(e => e.GenreName)
                .HasMaxLength(50)
                .HasColumnName("genreName");
        });

        modelBuilder.Entity<TProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PRIMARY");

            entity.ToTable("t_profile");

            entity.HasIndex(e => e.ProfileAvatarId, "FK_AvatarID_idx");

            entity.HasIndex(e => e.ProfileId, "profileID_UNIQUE").IsUnique();

            entity.HasIndex(e => e.UsId, "usID_UNIQUE").IsUnique();

            entity.Property(e => e.ProfileId).HasColumnName("profileID");
            entity.Property(e => e.ProfileAvatarId).HasColumnName("profileAvatarID");
            entity.Property(e => e.ProfileBirthdate)
                .HasColumnType("datetime")
                .HasColumnName("profileBirthdate");
            entity.Property(e => e.ProfileDescription)
                .HasMaxLength(400)
                .HasColumnName("profileDescription");
            entity.Property(e => e.ProfileFacebook)
                .HasMaxLength(100)
                .HasColumnName("profileFacebook");
            entity.Property(e => e.ProfileFullName)
                .HasMaxLength(100)
                .HasColumnName("profileFullName");
            entity.Property(e => e.ProfileInstagram)
                .HasMaxLength(100)
                .HasColumnName("profileInstagram");
            entity.Property(e => e.ProfileTwitter)
                .HasMaxLength(100)
                .HasColumnName("profileTwitter");
            entity.Property(e => e.UsId).HasColumnName("usID");

            entity.HasOne(d => d.ProfileAvatar).WithMany(p => p.TProfiles)
                .HasForeignKey(d => d.ProfileAvatarId)
                .HasConstraintName("FK_AvatarID");

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

        modelBuilder.Entity<TUserArtwork>(entity =>
        {
            entity.HasKey(e => e.ArtworkId).HasName("PRIMARY");

            entity.ToTable("t_user_artwork");

            entity.HasIndex(e => e.GenreId, "FK_genreID_idx");

            entity.HasIndex(e => e.UserId, "FK_userID_idx");

            entity.HasIndex(e => e.ArtworkId, "artworkID_UNIQUE").IsUnique();

            entity.Property(e => e.ArtworkId).HasColumnName("artworkID");
            entity.Property(e => e.ArtworkDate)
                .HasColumnType("datetime")
                .HasColumnName("artworkDate");
            entity.Property(e => e.ArtworkDescription)
                .HasMaxLength(500)
                .HasColumnName("artworkDescription");
            entity.Property(e => e.ArtworkTitle)
                .HasMaxLength(100)
                .HasColumnName("artworkTitle");
            entity.Property(e => e.ArtworkViews).HasColumnName("artworkViews");
            entity.Property(e => e.GenreId).HasColumnName("genreID");
            entity.Property(e => e.GenreOtherName)
                .HasMaxLength(100)
                .HasColumnName("genreOtherName");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Genre).WithMany(p => p.TUserArtworks)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_genreID");

            entity.HasOne(d => d.User).WithMany(p => p.TUserArtworks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_userID");
        });

        modelBuilder.Entity<TUserRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("t_user_registration");

            entity.HasIndex(e => e.ActivationCode, "ActivationCode_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Id, "Id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.UserId, "UserId_UNIQUE").IsUnique();

            entity.Property(e => e.ActivationCode).HasMaxLength(10);
            entity.Property(e => e.IsActivated).HasDefaultValueSql("'0'");
        });

        modelBuilder.Entity<TView>(entity =>
        {
            entity.HasKey(e => e.ViewId).HasName("PRIMARY");

            entity.ToTable("t_view");

            entity.HasIndex(e => e.ViewId, "viewId_UNIQUE").IsUnique();

            entity.Property(e => e.ViewId).HasColumnName("viewId");
            entity.Property(e => e.ArtworkId).HasColumnName("artworkId");
            entity.Property(e => e.UserId).HasColumnName("userId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
