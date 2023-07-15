namespace Domain.Services.Appointments
{
    internal class ClinicTimeChecker : IClinicTimeChecker
    {
        public bool IsValid(DateTime startDate, DateTime endDate)
        {
            // Check if the appointment is within the allowed time range
            if (startDate.DayOfWeek >= DayOfWeek.Saturday && startDate.DayOfWeek <= DayOfWeek.Wednesday &&
                startDate.TimeOfDay >= new TimeSpan(9, 0, 0) && endDate.TimeOfDay <= new TimeSpan(18, 0, 0))
            {
                return true;
            }

            return false;
        }
    }
}
