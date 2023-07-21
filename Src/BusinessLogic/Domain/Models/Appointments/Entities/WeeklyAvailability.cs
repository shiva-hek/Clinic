using Shared.Domain;
using Shared.Exceptions;

namespace Domain.Models.Appointments.Entities
{
    public class WeeklyAvailability :  BaseEntity
    { 
        public Guid DoctorId { get; private set; }
        public DayOfWeek Day { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        [Obsolete("Reserved for EF Core", true)]
        private WeeklyAvailability() { }

        public WeeklyAvailability(Guid id,DayOfWeek day, TimeSpan startTime, TimeSpan endTime,Guid doctorId)
        {
            AssertionConcern.AssertArgumentIsTrue(startTime < endTime,ErrorCode.InvalidStatrtTimeInWeeklyAvailability());

            Id = id;
            Day = day;
            StartTime = startTime;
            EndTime = endTime;
            DoctorId = doctorId;
        }
    }
}
