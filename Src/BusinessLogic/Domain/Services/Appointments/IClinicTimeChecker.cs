using Shared.Domain;

namespace Domain.Services.Appointments
{
    public interface IClinicTimeChecker : IDomainService
    {
        bool IsValid(DateTime startDate, DateTime endDate);
    }
}
