using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;
using Shared.Exceptions;

namespace Domain.Models.Appointments.Rules
{
    public class AppointmentMustBeInDoctorWorkingHoursRule : IRule
    {
        private readonly AppointmentTime _appointmentTime;
        private readonly Guid _doctorId;
        private readonly IDoctorTimeChecker _doctorTimeChecker;

        public AppointmentMustBeInDoctorWorkingHoursRule(
            AppointmentTime appointmentTime,
            Guid doctorId,
            IDoctorTimeChecker doctorTimeChecker
            )
        {
            AssertionConcern.AssertArgumentNotNull(appointmentTime, ErrorCode.IsNull(nameof(appointmentTime)));
            AssertionConcern.AssertArgumentNotNull(doctorTimeChecker, ErrorCode.IsNull(nameof(doctorTimeChecker)));
            AssertionConcern.AssertArgumentNotNull(doctorId, ErrorCode.IsNull(nameof(doctorId)));

            this._appointmentTime = appointmentTime;
            this._doctorTimeChecker = doctorTimeChecker;
            this._doctorId = doctorId;
        }

        public void Assert()
        {
            Assert(ErrorCode.OutOfDoctorWorkingHours);
        }

        public void Assert(ErrorCode errorCode)
        {
            if (!_doctorTimeChecker.IsValid(_appointmentTime.StartTime, _appointmentTime.EndTime, _doctorId))
                throw new ApiException(errorCode);
        }
    }
}
