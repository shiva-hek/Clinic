using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;
using Shared.Exceptions;

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
            AssertionConcern.AssertArgumentNotNull(patientId, ErrorCode.IsNull(nameof(patientId)));
            AssertionConcern.AssertArgumentNotNull(appointmentTime, ErrorCode.IsEmpty(nameof(appointmentTime)));
            AssertionConcern.AssertArgumentNotNull(appointmentNumberChecker,ErrorCode.IsEmpty(nameof(appointmentNumberChecker)));

            this._patientId = patientId;
            this._appointmentTime = appointmentTime;
            this._appointmentNumberChecker = appointmentNumberChecker;
        }

        public void Assert()
        {
            Assert(ErrorCode.NumberOfPatientAppointments);
        }

        public void Assert(ErrorCode errorCode)
        {
            if (!_appointmentNumberChecker.IsLessThanTwo(_patientId, _appointmentTime.StartTime))
                throw new ApiException(errorCode);
        }
    }
}