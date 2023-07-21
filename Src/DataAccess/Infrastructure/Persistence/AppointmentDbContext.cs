using Domain.Models.Appointments.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppointmentDbContext : DbContext
{
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<WeeklyAvailability> WeeklyAvailabilities { get; set; }

    public AppointmentDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppointmentDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
        modelBuilder.SeedDoctors();
        modelBuilder.SeedPatients();
    }
}