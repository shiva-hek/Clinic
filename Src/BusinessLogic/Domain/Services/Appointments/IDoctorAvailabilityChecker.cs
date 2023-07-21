using Domain.Models.Appointments.ValueObjects;

namespace Domain.Services.Appointments;

public interface IDoctorAvailabilityChecker
{
    bool IsAvailable(AppointmentTime appointmentTime, Guid doctorId);
}