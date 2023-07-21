namespace Domain.Services.Appointments
{
    public interface IClinicTimeChecker
    {
        bool IsValid(DateTime startDate, DateTime endDate);
    }
}
