using Domain.Models.Appointments.ValueObjects;

namespace Domain.Services.Appointments;

public interface IRoomAvailabilityChecker
{
    bool IsAvailable(AppointmentTime appointmentTime, Guid roomId);
}