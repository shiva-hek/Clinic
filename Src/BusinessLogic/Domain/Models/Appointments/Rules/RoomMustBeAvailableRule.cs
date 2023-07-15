using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;

namespace Domain.Models.Appointments.Rules;

public class RoomMustBeAvailableRule : IRule
{
    private readonly AppointmentTime _appointmentTime;
    private readonly Guid _roomId;
    private readonly IRoomAvailabilityChecker _roomAvailabilityChecker;


    public RoomMustBeAvailableRule(
        AppointmentTime appointmentTime,
        Guid roomId,
        IRoomAvailabilityChecker roomAvailabilityChecker
    )
    {
        AssertionConcern.AssertArgumentNotNull(appointmentTime, $"The {nameof(appointmentTime)} must be provided.");
        AssertionConcern.AssertArgumentNotNull(roomAvailabilityChecker,
            $"The {nameof(roomAvailabilityChecker)} must be provided.");
        AssertionConcern.AssertArgumentNotNull(roomId, $"The {nameof(roomId)} must be provided.");

        this._appointmentTime = appointmentTime;
        this._roomId = roomId;
        this._roomAvailabilityChecker = roomAvailabilityChecker;
    }

    public void Assert()
    {
        Assert($@"The room is scheduled for another appointment.");
    }

    public void Assert(string message)
    {
        if (!_roomAvailabilityChecker.IsAvailable(_appointmentTime, _roomId))
            throw new BusinessRuleViolationException(message ?? "");
    }
}