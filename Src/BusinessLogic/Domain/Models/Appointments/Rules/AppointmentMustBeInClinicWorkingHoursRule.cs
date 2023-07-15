using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;

namespace Domain.Models.Appointments.Rules
{
    public class AppointmentMustBeInClinicWorkingHoursRule : IRule
    {
        private readonly AppointmentTime _appointmentTime;
        private readonly IClinicTimeChecker _appointmentTimeChecker;


        public AppointmentMustBeInClinicWorkingHoursRule(AppointmentTime appointmentTime, IClinicTimeChecker appointmentTimeChecker)
        {
            AssertionConcern.AssertArgumentNotNull(appointmentTime, $"The {nameof(appointmentTime)} must be provided.");
            AssertionConcern.AssertArgumentNotNull(appointmentTimeChecker, $"The {nameof(appointmentTimeChecker)} must be provided.");

            this._appointmentTime = appointmentTime;

            this._appointmentTimeChecker = appointmentTimeChecker;
        }

        public void Assert()
        {
            Assert($@"The appointment time with ""{_appointmentTime.StartTime}"" and ""{_appointmentTime.EndTime}"" is not valid.");
        }

        public void Assert(string message)
        {
            if (!_appointmentTimeChecker.IsValid(_appointmentTime.StartTime, _appointmentTime.EndTime))
                throw new BusinessRuleViolationException(message ?? "");
        }
    }
}
