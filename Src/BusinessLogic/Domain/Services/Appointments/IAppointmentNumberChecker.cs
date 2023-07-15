using Shared.Domain;

namespace Domain.Services.Appointments
{
    public interface IAppointmentNumberChecker : IDomainService
    {
        bool IsLessThanTwo(Guid patientId, DateTime startTime);
    }
}
