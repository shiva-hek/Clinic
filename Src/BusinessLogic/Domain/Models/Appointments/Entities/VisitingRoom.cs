using Shared.Domain;

namespace Domain.Models.Appointments.Entities;

public class VisitingRoom : BaseEntity
{
    public string Title { get; private set; }

    [Obsolete("Reserved for EF Core", true)]
    private VisitingRoom()
    {
    }

    public VisitingRoom(
        Guid id,
        string title)
    {
        Id = id;
        Title = title;
    }
}