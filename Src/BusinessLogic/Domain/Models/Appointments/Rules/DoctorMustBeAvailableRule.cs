using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;

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
        AssertionConcern.AssertArgumentNotNull(appointmentTime, $"The {nameof(appointmentTime)} must be provided.");
        AssertionConcern.AssertArgumentNotNull(doctorAvailabilityChecker,
            $"The {nameof(doctorAvailabilityChecker)} must be provided.");
        AssertionConcern.AssertArgumentNotNull(doctorId, $"The {nameof(doctorId)} must be provided.");

        this._appointmentTime = appointmentTime;
        this._doctorId = doctorId;
        this._doctorAvailabilityChecker = doctorAvailabilityChecker;
    }

    public void Assert()
    {
        Assert($@"The doctor is scheduled for another appointment.");
    }

    public void Assert(string message)
    {
        if (!_doctorAvailabilityChecker.IsAvailable(_appointmentTime, _doctorId))
            throw new BusinessRuleViolationException(message ?? "");
    }
}