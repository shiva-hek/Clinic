using Shared.Domain;

namespace Domain.Models.Appointments.Entities
{
    public class WeeklySchedule
    {
        public List<WeeklyAvailability> Availabilities { get; private set; }

        [Obsolete("Reserved for EF Core", true)]
        private WeeklySchedule() { }

        public WeeklySchedule(List<WeeklyAvailability> availabilities)
        {
            AssertionConcern.AssertArgumentNotNull(availabilities, "Weekly availabilities must be provided.");
            AssertionConcern.AssertArgumentIsTrue(availabilities.Count > 0, "At least one weekly availability must be specified.");

            Availabilities = availabilities;
        }
    }
}
