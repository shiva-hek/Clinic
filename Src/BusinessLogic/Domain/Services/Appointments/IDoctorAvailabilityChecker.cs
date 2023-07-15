using Domain.Models.Appointments.ValueObjects;
using Shared.Domain;

namespace Domain.Services.Appointments;

public interface IDoctorAvailabilityChecker: IDomainService
{
    bool IsAvailable(AppointmentTime appointmentTime, Guid doctorId);
}