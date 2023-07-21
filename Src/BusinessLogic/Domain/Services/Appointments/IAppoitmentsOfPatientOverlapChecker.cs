using Domain.Models.Appointments.ValueObjects;

namespace Domain.Services.Appointments
{
    public interface IAppoitmentsOfPatientOverlapChecker
    {
        bool IsValid(Guid patientId, AppointmentTime appointmentTime);
    }
}
