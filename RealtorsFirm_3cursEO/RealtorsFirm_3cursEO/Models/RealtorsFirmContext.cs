using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace RealtorsFirm_3cursEO.Model;

public partial class RealtorsFirmContext : DbContext
{
    public RealtorsFirmContext()
    {
    }

    public RealtorsFirmContext(DbContextOptions<RealtorsFirmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Estate> Estates { get; set; }

    public virtual DbSet<Price> Prices { get; set; }

    public virtual DbSet<RoleEmployee> RoleEmployees { get; set; }

    public virtual DbSet<StatusTransaction> StatusTransactions { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TypeEstate> TypeEstates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=realtors_firm", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.39-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("PRIMARY");

            entity.ToTable("client");

            entity.Property(e => e.IdClient).HasColumnName("id_client");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(45)
                .HasColumnName("firstname");
            entity.Property(e => e.IsDelected)
                .HasDefaultValueSql("b'0'")
                .HasColumnType("bit(1)")
                .HasColumnName("is_delected");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Passport)
                .HasMaxLength(10)
                .HasColumnName("passport");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(45)
                .HasColumnName("patronymic");
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.IdEmployee).HasName("PRIMARY");

            entity.ToTable("employee");

            entity.HasIndex(e => e.IdRole, "id_role");

            entity.Property(e => e.IdEmployee).HasColumnName("id_employee");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(45)
                .HasColumnName("firstname");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.IsDelected)
                .HasDefaultValueSql("b'0'")
                .HasColumnType("bit(1)")
                .HasColumnName("is_delected");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Passport)
                .HasMaxLength(10)
                .HasColumnName("passport");
            entity.Property(e => e.Password)
                .HasMaxLength(45)
                .HasColumnName("password");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(45)
                .HasColumnName("patronymic");
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .HasColumnName("phone");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employee_ibfk_1");
        });

        modelBuilder.Entity<Estate>(entity =>
        {
            entity.HasKey(e => e.IdEstate).HasName("PRIMARY");

            entity.ToTable("estate");

            entity.HasIndex(e => e.IdClient, "id_client");

            entity.HasIndex(e => e.IdType, "id_type");

            entity.Property(e => e.IdEstate).HasColumnName("id_estate");
            entity.Property(e => e.Address)
                .HasMaxLength(120)
                .HasColumnName("address");
            entity.Property(e => e.Area).HasColumnName("area");
            entity.Property(e => e.Cost)
                .HasMaxLength(12)
                .HasColumnName("cost");
            entity.Property(e => e.IdClient).HasColumnName("id_client");
            entity.Property(e => e.IdType).HasColumnName("id_type");
            entity.Property(e => e.IsDelected)
                .HasDefaultValueSql("b'0'")
                .HasColumnType("bit(1)")
                .HasColumnName("is_delected");
            entity.Property(e => e.NumberRooms).HasColumnName("number_rooms");
            entity.Property(e => e.Photo).HasColumnName("photo");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Estates)
                .HasForeignKey(d => d.IdClient)
                .HasConstraintName("estate_ibfk_1");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.Estates)
                .HasForeignKey(d => d.IdType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("estate_ibfk_2");
        });

        modelBuilder.Entity<Price>(entity =>
        {
            entity.HasKey(e => e.IdPrice).HasName("PRIMARY");

            entity.ToTable("price");

            entity.Property(e => e.IdPrice).HasColumnName("id_price");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<RoleEmployee>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PRIMARY");

            entity.ToTable("role_employee");

            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<StatusTransaction>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("PRIMARY");

            entity.ToTable("status_transaction");

            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.IdTransaction).HasName("PRIMARY");

            entity.ToTable("transaction");

            entity.HasIndex(e => e.IdClient, "id_client");

            entity.HasIndex(e => e.IdEmployee, "id_employee");

            entity.HasIndex(e => e.IdEstate, "id_estate");

            entity.HasIndex(e => e.IdPrice, "id_price");

            entity.HasIndex(e => e.IdStatus, "id_status");

            entity.Property(e => e.IdTransaction).HasColumnName("id_transaction");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.IdClient).HasColumnName("id_client");
            entity.Property(e => e.IdEmployee).HasColumnName("id_employee");
            entity.Property(e => e.IdEstate).HasColumnName("id_estate");
            entity.Property(e => e.IdPrice).HasColumnName("id_price");
            entity.Property(e => e.IdStatus).HasColumnName("id_status");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transaction_ibfk_1");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.IdEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transaction_ibfk_2");

            entity.HasOne(d => d.IdEstateNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.IdEstate)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transaction_ibfk_3");

            entity.HasOne(d => d.IdPriceNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.IdPrice)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transaction_ibfk_5");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transaction_ibfk_4");
        });

        modelBuilder.Entity<TypeEstate>(entity =>
        {
            entity.HasKey(e => e.IdType).HasName("PRIMARY");

            entity.ToTable("type_estate");

            entity.Property(e => e.IdType).HasColumnName("id_type");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
