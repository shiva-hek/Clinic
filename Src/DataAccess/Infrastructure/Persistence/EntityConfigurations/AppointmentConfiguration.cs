using Domain.Models.Appointments.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");
        builder.HasKey(a => a.Id);

        builder.Property(a =>a.Id)
            .HasColumnName("Id");
        
        builder.OwnsOne(
            a => a.AppointmentTime, appointmentTimeBuilder =>
            {
                appointmentTimeBuilder.Property(at => at.StartTime)
                    .IsRequired()
                    .HasColumnName("StartTime");

                appointmentTimeBuilder.Property(at => at.Duration)
                    .IsRequired()
                    .HasColumnName("Duration");

                appointmentTimeBuilder.Property(at => at.EndTime)
                    .IsRequired()
                    .HasColumnName("EndTime");
            });

        builder.Property(a => a.DoctorId)
            .IsRequired()
            .HasColumnName("DoctorId");

        builder.Property(a => a.PatientId)
            .IsRequired()
            .HasColumnName("PatientId");

        builder.Property(a => a.RoomId)
            .IsRequired(false)
            .HasColumnName("RoomId");
        
        
    }
}