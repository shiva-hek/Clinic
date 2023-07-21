using Shared.Domain;

namespace Domain.Models.Appointments.Entities;

public class Room : BaseEntity
{
    public string Title { get; private set; }

    [Obsolete("Reserved for EF Core", true)]
    private Room()
    {
    }

    public Room(
        Guid id,
        string title)
    {
        Id = id;
        Title = title;
    }
}