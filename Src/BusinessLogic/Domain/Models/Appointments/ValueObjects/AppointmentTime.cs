

using Shared.Domain;
using Shared.Exceptions;

namespace Domain.Models.Appointments.ValueObjects
{
    public class AppointmentTime : ValueObject<AppointmentTime>
    {
        public DateTime StartTime { get; private set; }
        public TimeSpan Duration { get; private set; }
        public DateTime EndTime
        {
            get
            {
                return StartTime + Duration;
            }
            private set { }
        }

        [Obsolete("Reserved for EF Core", true)]
        private AppointmentTime()
        {

        }

        public AppointmentTime(DateTime startTime, TimeSpan duration)
        {
            AssertionConcern.AssertArgumentIsTrue(startTime > DateTime.Now, ErrorCode.StartTimeValidation);
            AssertionConcern.AssertArgumentIsTrue(duration.TotalMinutes > 0, ErrorCode.DurationValidation);

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
