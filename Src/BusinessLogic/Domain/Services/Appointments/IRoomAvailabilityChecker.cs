using Domain.Models.Appointments.ValueObjects;
using Shared.Domain;

namespace Domain.Services.Appointments;

public interface IRoomAvailabilityChecker : IDomainService
{
    bool IsAvailable(AppointmentTime appointmentTime, Guid roomId);
}