using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;
using Shared.Exceptions;

namespace Domain.Models.Appointments.Rules
{
    public class AppointmentDurationMustBeValidRule : IRule
    {
        private readonly AppointmentTime _appointmentTime;
        private readonly Guid _doctorId;
        private readonly IAppointmentDurationChecker _appointmentDurationCheck;

        public AppointmentDurationMustBeValidRule(AppointmentTime appointmentTime, Guid doctorId, IAppointmentDurationChecker appointmentDurationCheck)
        {
            AssertionConcern.AssertArgumentNotNull(appointmentTime, ErrorCode.IsNull(nameof(appointmentTime)));
            AssertionConcern.AssertArgumentNotNull(doctorId, ErrorCode.IsNull(nameof(doctorId)));
            AssertionConcern.AssertArgumentNotNull(appointmentDurationCheck, ErrorCode.IsNull(nameof(appointmentDurationCheck)));

            this._appointmentTime = appointmentTime;
            this._doctorId = doctorId;
            this._appointmentDurationCheck = appointmentDurationCheck;
        }

        public void Assert()
        {
            Assert(ErrorCode.InvalidDurationWithDoctorType);
        }

        public void Assert(ErrorCode errorCode)
        {
            if (!_appointmentDurationCheck.IsValid(_doctorId, _appointmentTime))
                throw new ApiException(errorCode);
        }
    }
}
