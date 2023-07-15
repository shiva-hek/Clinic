using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;

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
        AssertionConcern.AssertArgumentNotNull(appointmentTime, $"The {nameof(appointmentTime)} must be provided.");
        AssertionConcern.AssertArgumentNotNull(appointmentOverlapChecker,
            $"The {nameof(appointmentOverlapChecker)} must be provided.");

        this._appointmentTime = appointmentTime;
        this._appointmentOverlapChecker = appointmentOverlapChecker;
    }

    public void Assert()
    {
        Assert(
            $@"There is a conflicting appointment with this one.");
    }

    public void Assert(string message)
    {
        if (!_appointmentOverlapChecker.HasNoConflict(_appointmentTime))
            throw new BusinessRuleViolationException(message ?? "");
    }
}