using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;
using Shared.Exceptions;

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
        AssertionConcern.AssertArgumentNotNull(appointmentTime, ErrorCode.IsNull(nameof(appointmentTime)));
        AssertionConcern.AssertArgumentNotNull(roomAvailabilityChecker, ErrorCode.IsNull(nameof(roomAvailabilityChecker)));
        AssertionConcern.AssertArgumentNotNull(roomId, ErrorCode.IsNull(nameof(roomId)));

        this._appointmentTime = appointmentTime;
        this._roomId = roomId;
        this._roomAvailabilityChecker = roomAvailabilityChecker;
    }

    public void Assert()
    {
        Assert(ErrorCode.UnavailableRoom);
    }

    public void Assert(ErrorCode errorCode)
    {
        if (!_roomAvailabilityChecker.IsAvailable(_appointmentTime, _roomId))
            throw new ApiException(errorCode);
    }
}