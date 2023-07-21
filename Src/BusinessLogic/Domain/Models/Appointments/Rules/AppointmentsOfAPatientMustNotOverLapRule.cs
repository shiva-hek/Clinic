using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;
using Shared.Exceptions;

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
            AssertionConcern.AssertArgumentNotNull(appointmentTime, ErrorCode.IsNull(nameof(appointmentTime)));
            AssertionConcern.AssertArgumentNotNull(appoitmentsOfPatientOverlapChecker,ErrorCode.IsNull(nameof(appoitmentsOfPatientOverlapChecker)));
            AssertionConcern.AssertArgumentNotNull(patientId, ErrorCode.IsNull(nameof(patientId)));

            this._appointmentTime = appointmentTime;
            this._patientId = patientId;
            this._appoitmentsOfPatientOverlapChecker = appoitmentsOfPatientOverlapChecker;
        }

        public void Assert()
        {
            Assert(ErrorCode.OverlapWithPatientAppointment);
        }

        public void Assert(ErrorCode error)
        {
            if (!_appoitmentsOfPatientOverlapChecker.IsValid(_patientId, _appointmentTime))
                throw new ApiException(error);
        }
    }
}