using System;
using System.Collections.Generic;
using DataAccess.DBContexts.Wfm.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DBContexts.Wfm;

public partial class WfmDbContext : DbContext
{
    //public WfmDbContext()
    //{
    //}

    public WfmDbContext(DbContextOptions<WfmDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<StaffingRequirement> StaffingRequirements { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseNpgsql("Host=localhost;Database=WfmDb;Username=postgres;Password=password1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("schedules_pkey");

            entity.Property(e => e.Status).HasDefaultValueSql("'Scheduled'::character varying");

            entity.HasOne(d => d.Agent).WithMany(p => p.Schedules).HasConstraintName("fk_agent");
        });

        modelBuilder.Entity<StaffingRequirement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("staffing_requirements_pkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
