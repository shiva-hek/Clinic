using Domain.Models.Appointments.ValueObjects;
using Shared.Domain;

namespace Domain.Services.Appointments
{
    public interface IAppointmentDurationChecker : IDomainService
    {
        bool IsValid(Guid doctorId, AppointmentTime appointmentTime);
    }
}

