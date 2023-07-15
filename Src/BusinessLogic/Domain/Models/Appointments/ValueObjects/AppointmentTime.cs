

using Shared.Domain;

namespace Domain.Models.Appointments.ValueObjects
{
    public class AppointmentTime : ValueObject<AppointmentTime>
    {
        public DateTime StartTime { get; private set; }
        public TimeSpan Duration { get; private set; }
        public DateTime EndTime => StartTime + Duration;

        [Obsolete("Reserved for EF Core", true)]
        private AppointmentTime()
        {

        }

        public AppointmentTime(DateTime startTime, TimeSpan duration)
        {
            AssertionConcern.AssertArgumentIsTrue(StartTime > DateTime.Now, $"The {nameof(StartTime)} must be less than current date.");
            AssertionConcern.AssertArgumentIsTrue(duration.TotalMinutes > 0, "Duration must be greater than zero.");

            StartTime = startTime;
            Duration = duration;
        }

        public bool OverlapsWith(AppointmentTime other)
        {
            if (this.StartTime >= other.EndTime || this.EndTime <= other.StartTime)
            {
                return false;
            }

            return true;
        }
    }
}
