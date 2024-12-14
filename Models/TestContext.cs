using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlazorAzureADB2CApp1.Models;

public partial class TestContext : DbContext
{
    public TestContext(DbContextOptions<TestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Childrens> Childrens { get; set; }

    public virtual DbSet<Classes> Classes { get; set; }

    public virtual DbSet<EmergencyContacts> EmergencyContacts { get; set; }

    public virtual DbSet<LinkedAccounts> LinkedAccounts { get; set; }

    public virtual DbSet<MessageTargets> MessageTargets { get; set; }

    public virtual DbSet<Messages> Messages { get; set; }

    public virtual DbSet<Parents> Parents { get; set; }

    public virtual DbSet<Routs> Routs { get; set; }

    public virtual DbSet<Teachers> Teachers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Childrens>(entity =>
        {
            entity.HasKey(e => e.ChildId).HasName("PK__Children__BEFA0716E2B16518");

            entity.Property(e => e.Birthday)
                .HasMaxLength(8)
                .IsFixedLength();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Parent).WithMany(p => p.Childrens)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Childrens__Paren__00200768");
        });

        modelBuilder.Entity<Classes>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ClassId)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<EmergencyContacts>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__Emergenc__5C66259BE111DBA2");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Relation).HasMaxLength(50);
            entity.Property(e => e.Tel).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Parent).WithMany(p => p.EmergencyContacts)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Emergency__Paren__7B5B524B");
        });

        modelBuilder.Entity<LinkedAccounts>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__LinkedAc__349DA5A6FDA3EDB3");

            entity.Property(e => e.AccountIdentifier).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Provider).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Parent).WithMany(p => p.LinkedAccounts)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK__LinkedAcc__Paren__75A278F5");
        });

        modelBuilder.Entity<MessageTargets>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.TargetType).HasMaxLength(10);
        });

        modelBuilder.Entity<Messages>(entity =>
        {
            entity.HasKey(e => e.MessageId);

            entity.Property(e => e.MessageId).ValueGeneratedNever();
            entity.Property(e => e.Context).HasColumnType("text");
            entity.Property(e => e.SenderType).HasMaxLength(10);
        });

        modelBuilder.Entity<Parents>(entity =>
        {
            entity.HasKey(e => e.ParentId).HasName("PK__Parents__D339516FF4DABD57");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrentAddress).HasMaxLength(255);
            entity.Property(e => e.DistrictName).HasMaxLength(255);
            entity.Property(e => e.EmailAddress).HasMaxLength(255);
            entity.Property(e => e.HomePhoneNumber).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PostalCo)
                .HasMaxLength(7)
                .IsFixedLength();
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Routs>(entity =>
        {
            entity.HasKey(e => e.RouteId).HasName("PK__Routs__80979B4DFB1786F4");

            entity.Property(e => e.CommuteCategory).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PhotoLocation).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Parent).WithMany(p => p.Routs)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Routs__ParentId__04E4BC85");
        });

        modelBuilder.Entity<Teachers>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AccessId).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
