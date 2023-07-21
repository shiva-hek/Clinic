using Domain.Models.Appointments.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patients");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id");
        
        builder.OwnsOne(p => p.Name, nameBuilder =>
        {
            nameBuilder.Property(n => n.Firstname)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasColumnName("FirstName");

            nameBuilder.Property(n => n.Lastname)
                .IsRequired()
                .HasColumnType("varchar(50)")
                .HasColumnName("LastName");
        }).Navigation(user => user.Name).IsRequired();

        builder.Property(p => p.Gender)
            .IsRequired()
            .HasColumnName("Gender");

        builder.OwnsOne(p => p.EmailAddress, emailBuilder =>
        {
            emailBuilder.Property(e => e.Value)
                .IsRequired()
                .HasColumnName("Email");
        });

        builder.OwnsOne(c => c.EmailAddress, email =>
        {
            email.Property(p => p.Value)
               .HasColumnName("Email")
               .HasColumnType("varchar(50)")
               .IsRequired();

            email.HasIndex(p => p.Value)
            .IsUnique();
        }).Navigation(user => user.EmailAddress).IsRequired();
    }
}