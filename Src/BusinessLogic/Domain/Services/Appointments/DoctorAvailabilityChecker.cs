using Domain.Models.Appointments.Interfaces;
using Domain.Models.Appointments.ValueObjects;

namespace Domain.Services.Appointments;

public class DoctorAvailabilityChecker: IDoctorAvailabilityChecker
{
    private readonly IAppointmentRepository _appointmentRepository;

    public DoctorAvailabilityChecker(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public bool IsAvailable(AppointmentTime appointmentTime, Guid doctorId)
    {
        // Check if the doctor has not another appointment in the selected time
        return _appointmentRepository.GetConflictingAppointmentByDoctorId(appointmentTime, doctorId) == null;
    }
}