using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;
using Shared.Exceptions;

namespace Domain.Models.Appointments.Rules
{
    public class AppointmentMustBeInClinicWorkingHoursRule : IRule
    {
        private readonly AppointmentTime _appointmentTime;
        private readonly IClinicTimeChecker _appointmentTimeChecker;


        public AppointmentMustBeInClinicWorkingHoursRule(AppointmentTime appointmentTime, IClinicTimeChecker appointmentTimeChecker)
        {
            AssertionConcern.AssertArgumentNotNull(appointmentTime, ErrorCode.IsNull(nameof(appointmentTime)));
            AssertionConcern.AssertArgumentNotNull(appointmentTimeChecker, ErrorCode.IsNull(nameof(appointmentTimeChecker)));

            this._appointmentTime = appointmentTime;

            this._appointmentTimeChecker = appointmentTimeChecker;
        }

        public void Assert()
        {
            Assert(ErrorCode.OutOfClinicWorkingHours);
        }

        public void Assert(ErrorCode errorCode)
        {
            if (!_appointmentTimeChecker.IsValid(_appointmentTime.StartTime, _appointmentTime.EndTime))
                throw new ApiException(errorCode);
        }

    }
}
