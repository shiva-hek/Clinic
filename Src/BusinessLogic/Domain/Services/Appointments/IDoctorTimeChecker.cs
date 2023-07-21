namespace Domain.Services.Appointments
{
    public interface IDoctorTimeChecker
    {
        bool IsValid(DateTime startTime, DateTime endTime, Guid doctorId);
    }
}
