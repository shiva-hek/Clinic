using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;

namespace Domain.Models.Appointments.Rules
{
    public class AppointmentsOfAPatientMustNotOverLapRule : IRule
    {
        private readonly Guid _patientId;
        private readonly AppointmentTime _appointmentTime;
        private readonly IAppoitmentsOfPatientOverlapChecker _appoitmentsOfPatientOverlapChecker;

        public AppointmentsOfAPatientMustNotOverLapRule(
            AppointmentTime appointmentTime,
            Guid patientId,
            IAppoitmentsOfPatientOverlapChecker appoitmentsOfPatientOverlapChecker
        )
        {
            AssertionConcern.AssertArgumentNotNull(appointmentTime, $"The {nameof(appointmentTime)} must be provided.");
            AssertionConcern.AssertArgumentNotNull(appoitmentsOfPatientOverlapChecker,
                $"The {nameof(appoitmentsOfPatientOverlapChecker)} must be provided.");
            AssertionConcern.AssertArgumentNotNull(patientId, $"The {nameof(patientId)} must be provided.");

            this._appointmentTime = appointmentTime;
            this._patientId = patientId;
            this._appoitmentsOfPatientOverlapChecker = appoitmentsOfPatientOverlapChecker;
        }

        public void Assert()
        {
            Assert(
                $@"The appointment time with ""{_appointmentTime.StartTime}"" and ""{_appointmentTime.EndTime}"" is not valid.");
        }

        public void Assert(string message)
        {
            if (!_appoitmentsOfPatientOverlapChecker.IsValid(_patientId, _appointmentTime))
                throw new BusinessRuleViolationException(message ?? "");
        }
    }
}