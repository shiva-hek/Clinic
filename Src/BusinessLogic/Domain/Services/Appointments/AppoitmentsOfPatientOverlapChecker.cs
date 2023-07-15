using Domain.Models.Appointments.Interfaces;
using Domain.Models.Appointments.ValueObjects;

namespace Domain.Services.Appointments
{
    public class AppoitmentsOfPatientOverlapChecker : IAppoitmentsOfPatientOverlapChecker
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppoitmentsOfPatientOverlapChecker(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
        
        public bool IsValid(Guid patientId, AppointmentTime appointmentTime)
        {
            return _appointmentRepository.GetByRoomId(appointmentTime, patientId) == null;
        }
    }
}
