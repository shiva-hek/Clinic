using Domain.Models.Appointments.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityConfigurations;

public class RoomConfiguration: IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");
        builder.HasKey(d => d.Id);
        
        builder.Property(a =>a.Id)
            .HasColumnName("Id");
        
        builder.Property(r => r.Title)
            .IsRequired()
            .HasColumnName("title");
    }
}