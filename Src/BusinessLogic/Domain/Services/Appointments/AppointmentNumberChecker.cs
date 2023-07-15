using Domain.Models.Appointments.Interfaces;

namespace Domain.Services.Appointments
{
    public class AppointmentNumberChecker : IAppointmentNumberChecker
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentNumberChecker(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public bool IsLessThanTwo(Guid patientId, DateTime startTime)
        {
            return _appointmentRepository.Count(patientId, startTime) < 2;
        }
    }
}