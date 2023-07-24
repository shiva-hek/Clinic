namespace Clinic.AcceptanceTests.Dtos
{
    public class WeeklyAvailibilityDto
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public string Day { get; set; } = null!;
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
    }
}
