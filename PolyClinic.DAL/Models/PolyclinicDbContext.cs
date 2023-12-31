﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PolyClinic.DAL.Models;

public partial class PolyclinicDbContext : DbContext
{
    public PolyclinicDbContext()
    {
    }

    public PolyclinicDbContext(DbContextOptions<PolyclinicDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
        var config = builder.Build();
        var connectionString = config.GetConnectionString("PolyClinicDBConnectionString");
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentNo).HasName("pk_AppointmentNo");

            entity.Property(e => e.DateofAppointment).HasColumnType("date");
            entity.Property(e => e.DoctorId)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DoctorID");
            entity.Property(e => e.PatientId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PatientID");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_DoctorID");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_PatientID");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("pk_DoctorID");

            entity.Property(e => e.DoctorId)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DoctorID");
            entity.Property(e => e.DoctorName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Fees).HasColumnType("money");
            entity.Property(e => e.Specialization)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("pk_PatientID");

            entity.Property(e => e.PatientId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PatientID");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PatientName)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
