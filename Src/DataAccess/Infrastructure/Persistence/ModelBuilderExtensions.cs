using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static class ModelBuilderExtensions
{
    static readonly Guid DoctorId = new Guid("0735B919-BD3A-48E2-A2BF-DD699E561940");
    static readonly Guid PatientId = new Guid("504ECB9F-2A34-4FE9-9B89-24E2ACAC0620");

    public static void SeedDoctors(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>(b =>
        {
            b.HasData(new
            {
                Id = DoctorId,
                DoctorType = DoctorType.General,
            });

            b.OwnsOne(e => e.EmailAddress).HasData(new
            {
                DoctorId = DoctorId,
                Value = "doctor.smith@example.com",

            });

            b.OwnsOne(e => e.Name).HasData(new
            {
                DoctorId = DoctorId,
                Firstname = "Doctor",
                Lastname = "Smith.k",

            });
        });

        SeedWeeklyAvailabilities(modelBuilder, DoctorId);
    }

    public static void SeedWeeklyAvailabilities(this ModelBuilder modelBuilder, Guid doctorId)
    {
        var availabilities = new List<WeeklyAvailability>();

        for (DayOfWeek day = DayOfWeek.Monday; day <= DayOfWeek.Wednesday; day++)
        {
            availabilities.Add(new WeeklyAvailability(
                id: Guid.NewGuid(),
                day: day,
                startTime: new TimeSpan(9, 0, 0),
                endTime: new TimeSpan(12, 0, 0),
                doctorId: doctorId
                ));
        }

        foreach (var item in availabilities)
        {
            modelBuilder.Entity<WeeklyAvailability>(b =>
            {
                b.HasData(new
                {
                    Id = item.Id,
                    Day = item.Day,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    DoctorId = item.DoctorId
                });
            });
        }
    }

    public static void SeedPatients(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(b =>
        {
            b.HasData(new
            {
                Id = PatientId,
                Gender = Gender.Male,
            });

            b.OwnsOne(e => e.EmailAddress).HasData(new
            {
                PatientId = PatientId,
                Value = "marry@example.com",

            });

            b.OwnsOne(e => e.Name).HasData(new
            {
                PatientId = PatientId,
                Firstname = "Marry",
                Lastname = "Johnson",

            });
        });
    }
}
