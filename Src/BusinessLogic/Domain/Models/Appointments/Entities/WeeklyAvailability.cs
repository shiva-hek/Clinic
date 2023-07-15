using Shared.Domain;

namespace Domain.Models.Appointments.Entities
{
    public class WeeklyAvailability
    {
        public DayOfWeek Day { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        [Obsolete("Reserved for EF Core", true)]
        private WeeklyAvailability() { }

        public WeeklyAvailability(DayOfWeek day, TimeSpan startTime, TimeSpan endTime)
        {
            AssertionConcern.AssertArgumentIsTrue(startTime < endTime, "Start time cannot be later than end time.");

            Day = day;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
