namespace Clinic.AcceptanceTests.Dtos
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public string Date { get; set; } = null!;
        public string Time { get; set; } = null!;
        public int Duration { get; set; }
    }
}
