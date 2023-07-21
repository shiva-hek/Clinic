using Domain.Models.Appointments.ValueObjects;

namespace Domain.Services.Appointments
{
    public interface IAppointmentDurationChecker
    {
        bool IsValid(Guid doctorId, AppointmentTime appointmentTime);
    }
}

