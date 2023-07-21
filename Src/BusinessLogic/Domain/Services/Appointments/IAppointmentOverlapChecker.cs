using Domain.Models.Appointments.ValueObjects;

namespace Domain.Services.Appointments
{
    public interface IAppointmentOverlapChecker
    {
        bool HasNoConflict(AppointmentTime appointmentTime);
    }
}
