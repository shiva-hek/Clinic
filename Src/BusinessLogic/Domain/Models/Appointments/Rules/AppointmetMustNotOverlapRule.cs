using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;
using Shared.Exceptions;

namespace Domain.Models.Appointments.Rules;

public class AppointmetMustNotOverlapRule : IRule
{
    private readonly AppointmentTime _appointmentTime;
    private readonly IAppointmentOverlapChecker _appointmentOverlapChecker;

    public AppointmetMustNotOverlapRule(
        AppointmentTime appointmentTime,
        IAppointmentOverlapChecker appointmentOverlapChecker
    )
    {
        AssertionConcern.AssertArgumentNotNull(appointmentTime, ErrorCode.IsNull(nameof(appointmentTime)));
        AssertionConcern.AssertArgumentNotNull(appointmentOverlapChecker, ErrorCode.IsNull(nameof(appointmentOverlapChecker)));

        this._appointmentTime = appointmentTime;
        this._appointmentOverlapChecker = appointmentOverlapChecker;
    }

    public void Assert()
    {
        Assert(ErrorCode.Overlap);
    }

    public void Assert(ErrorCode errorCode)
    {
        if (!_appointmentOverlapChecker.HasNoConflict(_appointmentTime))
            throw new ApiException(errorCode);
    }
}