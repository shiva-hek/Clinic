namespace Domain.Models.Appointments.Common;

public static class AppConfig
{
    public const DayOfWeek ClinicWorkingStartDay = DayOfWeek.Saturday;
    public const DayOfWeek ClinicWorkingEndDay = DayOfWeek.Wednesday;
    public const int TimeLimitationToSetAppointment = 6;

    public static readonly TimeSpan ClinicWorkingStartTime = new TimeSpan(9, 0, 0);
    public static readonly TimeSpan ClinicWorkingEndTime = new TimeSpan(18, 0, 0);
}
