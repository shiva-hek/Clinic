using Domain.Models.Appointments.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctors");
        builder.HasKey(d => d.Id);

        builder.Property(a =>a.Id)
            .HasColumnName("Id");
        
        builder.OwnsOne(d => d.Name, nameBuilder =>
        {
            nameBuilder.Property(n => n.Firstname)
                .HasColumnName("FirstName")
                .HasColumnType("varchar(20)")
                .IsRequired();

            nameBuilder.Property(n =>n.Lastname)
                .HasColumnName("LastName")
                .HasColumnType("varchar(30)")
                .IsRequired();
        }).Navigation(user => user.Name).IsRequired();

        builder.OwnsOne(c => c.EmailAddress, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .HasColumnType("varchar(50)")
                .IsRequired();

            email.HasIndex(e => e.Value)
                .IsUnique();
        }).Navigation(doctor => doctor.EmailAddress).IsRequired();
        
        builder.Property(d => d.DoctorType)
            .IsRequired()
            .HasColumnName("DoctorType");
        
        builder.HasMany(d => d.Availabilities)
            .WithOne()
            .HasForeignKey(w => w.DoctorId);

        builder.HasMany(d => d.Appointments)
            .WithOne()
            .HasForeignKey(a => a.DoctorId);
    }
}