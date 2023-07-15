using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;

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
            AssertionConcern.AssertArgumentNotNull(appointmentTime, $"The {nameof(appointmentTime)} must be provided.");
            AssertionConcern.AssertArgumentNotNull(doctorTimeChecker, $"The {nameof(doctorTimeChecker)} must be provided.");
            AssertionConcern.AssertArgumentNotNull(doctorId, $"The {nameof(doctorId)} must be provided.");

            this._appointmentTime = appointmentTime;
            this._doctorTimeChecker = doctorTimeChecker;
            this._doctorId = doctorId;
        }

        public void Assert()
        {
            Assert($@"The appointment time with ""{_appointmentTime.StartTime}"" and ""{_appointmentTime.EndTime}"" is not valid.");
        }

        public void Assert(string message)
        {
            if (!_doctorTimeChecker.IsValid(_appointmentTime.StartTime, _appointmentTime.EndTime, _doctorId))
                throw new BusinessRuleViolationException(message ?? "");
        }
    }
}
