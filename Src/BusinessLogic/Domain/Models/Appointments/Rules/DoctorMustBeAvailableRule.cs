using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;
using Shared.Exceptions;

namespace Domain.Models.Appointments.Rules;

public class DoctorMustBeAvailableRule : IRule
{
    private readonly AppointmentTime _appointmentTime;
    private readonly Guid _doctorId;
    private readonly IDoctorAvailabilityChecker _doctorAvailabilityChecker;


    public DoctorMustBeAvailableRule(
        AppointmentTime appointmentTime,
        Guid doctorId,
        IDoctorAvailabilityChecker doctorAvailabilityChecker
    )
    {
        AssertionConcern.AssertArgumentNotNull(appointmentTime, ErrorCode.IsNull(nameof(appointmentTime)));
        AssertionConcern.AssertArgumentNotNull(doctorAvailabilityChecker,
            ErrorCode.IsNull(nameof(doctorAvailabilityChecker)));
        AssertionConcern.AssertArgumentNotNull(doctorId, ErrorCode.IsNull(nameof(doctorId)));

        this._appointmentTime = appointmentTime;
        this._doctorId = doctorId;
        this._doctorAvailabilityChecker = doctorAvailabilityChecker;
    }

    public void Assert()
    {
        Assert(ErrorCode.DoctorIsBusy);
    }

    public void Assert(ErrorCode error)
    {
        if (!_doctorAvailabilityChecker.IsAvailable(_appointmentTime, _doctorId))
            throw new ApiException(error);
    }
}