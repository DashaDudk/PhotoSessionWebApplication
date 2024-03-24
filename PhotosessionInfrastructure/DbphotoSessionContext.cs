using System;
using System.Collections.Generic;
using PhotosessionDomain.Model;
using Microsoft.EntityFrameworkCore;

//namespace PhotosessionDomain.Model;
namespace PhotosessionInfrastructure;

public partial class DbphotoSessionContext : DbContext
{
    public DbphotoSessionContext()
    {
    }

    public DbphotoSessionContext(DbContextOptions<DbphotoSessionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Photosession> Photosessions { get; set; }

    public virtual DbSet<PhotosessionLocation> PhotosessionLocations { get; set; }

    public virtual DbSet<PhotosessionStatus> PhotosessionStatuses { get; set; }

    public virtual DbSet<PhotosessionType> PhotosessionTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPhotosession> UserPhotosessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSqlLocalDb; Database=DBPhotoSession; Trusted_Connection=True; TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Photosession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PhotoSessions");

            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.PhotosessionLocation).WithMany(p => p.Photosessions)
                .HasForeignKey(d => d.PhotosessionLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhotoSessions_Locations");

            entity.HasOne(d => d.PhotosessionStatus).WithMany(p => p.Photosessions)
                .HasForeignKey(d => d.PhotosessionStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhotoSessions_PhotoSessionStatuses");

            entity.HasOne(d => d.PhotosessionType).WithMany(p => p.Photosessions)
                .HasForeignKey(d => d.PhotosessionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhotoSessions_PhotoSessionTypes");
        });

        modelBuilder.Entity<PhotosessionLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Locations");

            entity.Property(e => e.CityName).HasMaxLength(20);
        });

        modelBuilder.Entity<PhotosessionStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PhotoSessionStatuses");

            entity.Property(e => e.StatusName).HasMaxLength(20);
        });

        modelBuilder.Entity<PhotosessionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PhotoSessionTypes");

            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(20);
        });

        modelBuilder.Entity<UserPhotosession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserPhotoSessions");

            entity.HasOne(d => d.Photosession).WithMany(p => p.UserPhotosessions)
                .HasForeignKey(d => d.PhotosessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPhotoSessions_PhotoSessions");

            entity.HasOne(d => d.User).WithMany(p => p.UserPhotosessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPhotoSessions_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
