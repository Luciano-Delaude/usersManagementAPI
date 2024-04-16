using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace usersManagementAPI.Models;

public partial class UserManagementDbContext : DbContext
{
    public UserManagementDbContext()
    {
    }

    public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC20F62368");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserName)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
