using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Models;

public partial class BitsContext : DbContext
{
    public BitsContext()
    {
    }

    public BitsContext(DbContextOptions<BitsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppConfig> AppConfigs { get; set; }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=127.0.0.1;uid=root;pwd=Asdf13579!;database=bits", ServerVersion.Parse("8.1.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<AppConfig>(entity =>
        {
            entity.HasKey(e => e.BreweryId).HasName("PRIMARY");

            entity.ToTable("app_config");

            entity.Property(e => e.BreweryId)
                .ValueGeneratedNever()
                .HasColumnName("brewery_id");
            entity.Property(e => e.BreweryLogo)
                .HasMaxLength(50)
                .HasColumnName("brewery_logo");
            entity.Property(e => e.BreweryName)
                .HasMaxLength(200)
                .HasColumnName("brewery_name");
            entity.Property(e => e.Color1)
                .HasMaxLength(10)
                .HasColumnName("color_1");
            entity.Property(e => e.Color2)
                .HasMaxLength(10)
                .HasColumnName("color_2");
            entity.Property(e => e.Color3)
                .HasMaxLength(10)
                .HasColumnName("color_3");
            entity.Property(e => e.ColorBlack)
                .HasMaxLength(10)
                .HasColumnName("color_black");
            entity.Property(e => e.ColorWhite)
                .HasMaxLength(10)
                .HasColumnName("color_white");
            entity.Property(e => e.DefaultUnits)
                .HasMaxLength(50)
                .HasDefaultValueSql("'metric'")
                .HasColumnName("default_units");
            entity.Property(e => e.HomePageBackgroundImage)
                .HasMaxLength(50)
                .HasColumnName("home_page_background_image");
            entity.Property(e => e.HomePageText)
                .HasMaxLength(5000)
                .HasColumnName("home_page_text");
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.AppUserId).HasName("PRIMARY");

            entity.ToTable("app_user");

            entity.Property(e => e.AppUserId).HasColumnName("app_user_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
