using Domain.Models.Appointments.ValueObjects;
using Shared.Domain;

namespace Domain.Services.Appointments
{
    public interface IAppointmentOverlapChecker : IDomainService
    {
        bool HasNoConflict(AppointmentTime appointmentTime);
    }
}
