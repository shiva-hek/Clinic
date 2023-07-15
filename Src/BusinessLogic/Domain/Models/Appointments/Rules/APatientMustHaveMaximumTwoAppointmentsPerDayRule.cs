using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;

namespace Domain.Models.Appointments.Rules
{
    public class APatientMustHaveMaximumTwoAppointmentsPerDayRule : IRule
    {
        private readonly Guid _patientId;
        private readonly AppointmentTime _appointmentTime;
        private readonly IAppointmentNumberChecker _appointmentNumberChecker;

        public APatientMustHaveMaximumTwoAppointmentsPerDayRule(
            Guid patientId,
            AppointmentTime appointmentTime,
            IAppointmentNumberChecker appointmentNumberChecker)
        {
            AssertionConcern.AssertArgumentNotNull(patientId, $"The {nameof(patientId)} must be provided.");
            AssertionConcern.AssertArgumentNotNull(appointmentTime, $"The {nameof(appointmentTime)} must be provided.");
            AssertionConcern.AssertArgumentNotNull(appointmentNumberChecker,
                $"The {nameof(appointmentNumberChecker)} must be provided.");

            this._patientId = patientId;
            this._appointmentTime = appointmentTime;
            this._appointmentNumberChecker = appointmentNumberChecker;
        }

        public void Assert()
        {
            Assert(
                $@"The patient can not have more appointments for this day.");
        }

        public void Assert(string message)
        {
            if (!_appointmentNumberChecker.IsLessThanTwo(_patientId, _appointmentTime.StartTime))
                throw new BusinessRuleViolationException(message ?? "");
        }
    }
}