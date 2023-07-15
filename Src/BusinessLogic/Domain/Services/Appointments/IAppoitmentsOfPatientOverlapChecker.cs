using Domain.Models.Appointments.ValueObjects;
using Shared.Domain;

namespace Domain.Services.Appointments
{
    public interface IAppoitmentsOfPatientOverlapChecker : IDomainService
    {
        bool IsValid(Guid patientId, AppointmentTime appointmentTime);
    }
}
