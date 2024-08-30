using System;
using System.Collections.Generic;
using DailyDynamo.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DailyDynamo.DAL.Context;

public partial class DailyDynamoDatabaseContext : DbContext
{
    public DailyDynamoDatabaseContext()
    {
    }

    public DailyDynamoDatabaseContext(DbContextOptions<DailyDynamoDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<ApplicationRole> ApplicationRoles { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Designation> Designations { get; set; }

    public virtual DbSet<LookupElement> LookupElements { get; set; }

    public virtual DbSet<LookupHeader> LookupHeaders { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<WorkDiary> WorkDiaries { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Employee_Account_Id");

            entity.ToTable("Account", "Employee");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ApplicationRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Admin_ApplicationRole_Id");

            entity.ToTable("ApplicationRole", "Admin");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Pk_Admin_Department_Id");

            entity.ToTable("Department", "Admin");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DepartmentCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Designation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Admin_Designation_Id");

            entity.ToTable("Designation", "Admin");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DesignationName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<LookupElement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_List_LookupElement_Id");

            entity.ToTable("LookupElement", "List");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GroupName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LookupElementCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_List_LookupElement_CreatedBy_Employee_Profile_Id");

            entity.HasOne(d => d.LookupHeader).WithMany(p => p.LookupElements)
                .HasForeignKey(d => d.LookupHeaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_List_LookupElement_LookupHeaderId_List_LookupHeader_Id");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.LookupElementUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_List_LookupElement_UpdatedBy_Employee_Profile_Id");
        });

        modelBuilder.Entity<LookupHeader>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_List_LookupHeader_Id");

            entity.ToTable("LookupHeader", "List");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.KeyName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.LookupHeaderCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_List_LookupHeader_CreatedBy_Employee_Profile_Id");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.LookupHeaderUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_List_LookupHeader_UpdatedBy_Employee_Profile_Id");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Employee_Profile_Id");

            entity.ToTable("Profile", "Employee");

            entity.HasIndex(e => e.EmailId, "UQ_Employee_Profile_EmailID").IsUnique();

            entity.HasIndex(e => e.MobileNo, "UQ_Employee_Profile_MobileNo").IsUnique();

            entity.HasIndex(e => e.AccountId, "UQ__Profile__349DA5A7693BF3C3").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmailId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EmailID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ProfileImageUrl).HasMaxLength(200);
            entity.Property(e => e.UpdatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithOne(p => p.Profile)
                .HasForeignKey<Profile>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Profile_AccountId_Employee_Account_Id");

            entity.HasOne(d => d.Department).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_Employee_Profile_DepartmentId_Admin_Department_Id");

            entity.HasOne(d => d.Designation).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.DesignationId)
                .HasConstraintName("FK_Employee_Profile_DesignationId_Admin_Designation_Id");

            entity.HasOne(d => d.Gender).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.GenderId)
                .HasConstraintName("FK_Employee_Profile_GenderId_List_LookupElement_Id");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_Employee_Profile_ManagerId_Employee_Profile_Id");

            entity.HasOne(d => d.Role).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Profile_RoleId_Admin_Role_Id");
        });

        modelBuilder.Entity<WorkDiary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Employee_WorkDiary_Id");

            entity.ToTable("WorkDiary", "Employee");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ChallengesFaced)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.NextDayPlan)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TaskAccomplished)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WorkDiaryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_WorkDiary_CreatedBy_Employee_Profile_Id");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.WorkDiaryUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_WorkDiary_UpdatedBy_Employee_Profile_Id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
