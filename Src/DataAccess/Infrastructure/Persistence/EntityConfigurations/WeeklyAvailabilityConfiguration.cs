using Domain.Models.Appointments.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class WeeklyAvailabilityConfiguration : IEntityTypeConfiguration<WeeklyAvailability>
{
    public void Configure(EntityTypeBuilder<WeeklyAvailability> builder)
    {
        builder.ToTable("WeeklyAvailabilities");
        builder.HasKey(w => w.Id);
        
        builder.Property(w =>w.Id)
            .HasColumnName("Id");

        builder.Property(w => w.DoctorId)
            .HasColumnName("DoctorId");
        
        builder.Property(w => w.Day)
            .IsRequired()
            .HasColumnName("Day");

        builder.Property(w => w.StartTime)
            .IsRequired()
            .HasColumnName("StartTime");

        builder.Property(w => w.EndTime)
            .IsRequired()
            .HasColumnName("EndTime");
    }
}