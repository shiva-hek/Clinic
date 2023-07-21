namespace Domain.Services.Appointments
{
    public interface IAppointmentNumberChecker
    {
        bool IsLessThanTwo(Guid patientId, DateTime startTime);
    }
}
