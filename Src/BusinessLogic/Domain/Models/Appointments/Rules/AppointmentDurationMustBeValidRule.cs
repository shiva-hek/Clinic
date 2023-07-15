using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;

namespace Domain.Models.Appointments.Rules
{
    public class AppointmentDurationMustBeValidRule : IRule
    {
        private readonly AppointmentTime _appointmentTime;
        private readonly Guid _doctorId;
        private readonly IAppointmentDurationChecker _appointmentDurationCheck;

        public AppointmentDurationMustBeValidRule(AppointmentTime appointmentTime, Guid doctorId, IAppointmentDurationChecker appointmentDurationCheck)
        {
            AssertionConcern.AssertArgumentNotNull(appointmentTime, $"The {nameof(appointmentTime)} must be provided.");
            AssertionConcern.AssertArgumentNotNull(doctorId, $"The {nameof(doctorId)} must be provided.");
            AssertionConcern.AssertArgumentNotNull(appointmentDurationCheck, $"The {nameof(appointmentDurationCheck)} must be provided.");

            this._appointmentTime = appointmentTime;
            this._doctorId = doctorId;
            this._appointmentDurationCheck = appointmentDurationCheck;
        }

        public void Assert()
        {
            Assert($@"The appointment duration with this doctor is not valid.");
        }

        public void Assert(string message)
        {
            if (!_appointmentDurationCheck.IsValid(_doctorId, _appointmentTime))
                throw new BusinessRuleViolationException(message ?? "");
        }
    }
}
