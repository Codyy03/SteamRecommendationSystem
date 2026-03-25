using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace steam_discovery_platform.Server.Models;

public partial class SteamDbContext : DbContext
{
    public SteamDbContext()
    {
    }

    public SteamDbContext(DbContextOptions<SteamDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Developer> Developers { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Platform> Platforms { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLibrary> UserLibraries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=steamdb;Username=postgres;Password=admin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.Appid).HasName("applications_pkey");

            entity.ToTable("applications");

            entity.Property(e => e.Appid)
                .ValueGeneratedNever()
                .HasColumnName("appid");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .HasColumnName("currency");
            entity.Property(e => e.FinalPrice).HasColumnName("final_price");
            entity.Property(e => e.HeaderImage).HasColumnName("header_image");
            entity.Property(e => e.IsFree)
                .HasDefaultValue(false)
                .HasColumnName("is_free");
            entity.Property(e => e.MetacriticScore).HasColumnName("metacritic_score");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.PcRequirements)
                .HasColumnType("jsonb")
                .HasColumnName("pc_requirements");
            entity.Property(e => e.RecommendationsTotal)
                .HasDefaultValue(0)
                .HasColumnName("recommendations_total");
            entity.Property(e => e.ReleaseDate).HasColumnName("release_date");
            entity.Property(e => e.ShortDescription).HasColumnName("short_description");
            entity.Property(e => e.SupportsLinux)
                .HasDefaultValue(false)
                .HasColumnName("supports_linux");
            entity.Property(e => e.SupportsMac)
                .HasDefaultValue(false)
                .HasColumnName("supports_mac");
            entity.Property(e => e.SupportsWindows)
                .HasDefaultValue(true)
                .HasColumnName("supports_windows");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");

            entity.HasMany(d => d.Categories).WithMany(p => p.Apps)
                .UsingEntity<Dictionary<string, object>>(
                    "ApplicationCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("fk_app_cat_cat"),
                    l => l.HasOne<Application>().WithMany()
                        .HasForeignKey("Appid")
                        .HasConstraintName("fk_app_cat_app"),
                    j =>
                    {
                        j.HasKey("Appid", "CategoryId").HasName("application_categories_pkey");
                        j.ToTable("application_categories");
                        j.IndexerProperty<int>("Appid").HasColumnName("appid");
                        j.IndexerProperty<int>("CategoryId").HasColumnName("category_id");
                    });

            entity.HasMany(d => d.Developers).WithMany(p => p.Apps)
                .UsingEntity<Dictionary<string, object>>(
                    "ApplicationDeveloper",
                    r => r.HasOne<Developer>().WithMany()
                        .HasForeignKey("DeveloperId")
                        .HasConstraintName("fk_app_dev_dev"),
                    l => l.HasOne<Application>().WithMany()
                        .HasForeignKey("Appid")
                        .HasConstraintName("fk_app_dev_app"),
                    j =>
                    {
                        j.HasKey("Appid", "DeveloperId").HasName("application_developers_pkey");
                        j.ToTable("application_developers");
                        j.IndexerProperty<int>("Appid").HasColumnName("appid");
                        j.IndexerProperty<int>("DeveloperId").HasColumnName("developer_id");
                    });

            entity.HasMany(d => d.Platforms).WithMany(p => p.Apps)
                .UsingEntity<Dictionary<string, object>>(
                    "ApplicationPlatform",
                    r => r.HasOne<Platform>().WithMany()
                        .HasForeignKey("PlatformId")
                        .HasConstraintName("fk_app_plat_plat"),
                    l => l.HasOne<Application>().WithMany()
                        .HasForeignKey("Appid")
                        .HasConstraintName("fk_app_plat_app"),
                    j =>
                    {
                        j.HasKey("Appid", "PlatformId").HasName("application_platforms_pkey");
                        j.ToTable("application_platforms");
                        j.IndexerProperty<int>("Appid").HasColumnName("appid");
                        j.IndexerProperty<int>("PlatformId").HasColumnName("platform_id");
                    });
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Developer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("developers_pkey");

            entity.ToTable("developers");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genres_pkey");

            entity.ToTable("genres");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasMany(d => d.Apps).WithMany(p => p.Genres)
                .UsingEntity<Dictionary<string, object>>(
                    "ApplicationGenre",
                    r => r.HasOne<Application>().WithMany()
                        .HasForeignKey("Appid")
                        .HasConstraintName("fk_app_gen_app"),
                    l => l.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .HasConstraintName("fk_app_gen_gen"),
                    j =>
                    {
                        j.HasKey("GenreId", "Appid").HasName("application_genres_pkey");
                        j.ToTable("application_genres");
                        j.IndexerProperty<int>("GenreId").HasColumnName("genre_id");
                        j.IndexerProperty<int>("Appid").HasColumnName("appid");
                    });
        });

        modelBuilder.Entity<Platform>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("platforms_pkey");

            entity.ToTable("platforms");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("publishers_pkey");

            entity.ToTable("publishers");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasMany(d => d.Apps).WithMany(p => p.Publishers)
                .UsingEntity<Dictionary<string, object>>(
                    "ApplicationPublisher",
                    r => r.HasOne<Application>().WithMany()
                        .HasForeignKey("Appid")
                        .HasConstraintName("fk_app_pub_app"),
                    l => l.HasOne<Publisher>().WithMany()
                        .HasForeignKey("PublisherId")
                        .HasConstraintName("fk_app_pub_pub"),
                    j =>
                    {
                        j.HasKey("PublisherId", "Appid").HasName("application_publishers_pkey");
                        j.ToTable("application_publishers");
                        j.IndexerProperty<int>("PublisherId").HasColumnName("publisher_id");
                        j.IndexerProperty<int>("Appid").HasColumnName("appid");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasMany(d => d.Apps).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserIgnoredGame",
                    r => r.HasOne<Application>().WithMany()
                        .HasForeignKey("Appid")
                        .HasConstraintName("user_ignored_games_appid_fkey"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("user_ignored_games_user_id_fkey"),
                    j =>
                    {
                        j.HasKey("UserId", "Appid").HasName("user_ignored_games_pkey");
                        j.ToTable("user_ignored_games");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("user_id");
                        j.IndexerProperty<int>("Appid").HasColumnName("appid");
                    });
        });

        modelBuilder.Entity<UserLibrary>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.Appid }).HasName("user_libraries_pkey");

            entity.ToTable("user_libraries");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Appid).HasColumnName("appid");
            entity.Property(e => e.AddedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("added_at");
            entity.Property(e => e.IsFavorite)
                .HasDefaultValue(false)
                .HasColumnName("is_favorite");

            entity.HasOne(d => d.App).WithMany(p => p.UserLibraries)
                .HasForeignKey(d => d.Appid)
                .HasConstraintName("fk_app");

            entity.HasOne(d => d.User).WithMany(p => p.UserLibraries)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
