namespace Application.Appointments.Commands.CreateEarliestAppointment
{
    public class CreateEarliestAppointmentResultDto
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime StartTime { get; set; }
    }
}
