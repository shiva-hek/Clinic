using Domain.Models.Appointments.Interfaces;
using Domain.Models.Appointments.ValueObjects;

namespace Domain.Services.Appointments
{
    public class AppointmentOverlapChecker : IAppointmentOverlapChecker
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentOverlapChecker(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
        
        public bool HasNoConflict(AppointmentTime appointmentTime)
        {
            return _appointmentRepository.GetConflictingAppointments(appointmentTime).Count == 0;
        }
    }
}
