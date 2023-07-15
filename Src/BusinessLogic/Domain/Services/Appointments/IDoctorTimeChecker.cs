using Shared.Domain;

namespace Domain.Services.Appointments
{
    public interface IDoctorTimeChecker : IDomainService
    {
        bool IsValid(DateTime startTime, DateTime endTime, Guid doctorId);
    }
}
