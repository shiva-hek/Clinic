namespace Application.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentResultDto
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime StartTime { get; set; }
    }
}
