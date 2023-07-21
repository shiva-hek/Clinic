using Domain.Models.Appointments.Common;

namespace Domain.Services.Appointments
{
    public class ClinicTimeChecker : IClinicTimeChecker
    {
        public bool IsValid(DateTime startDate, DateTime endDate)
        {
            List<DayOfWeek> workingDays = GetDaysBetween(AppConfig.ClinicWorkingStartDay, AppConfig.ClinicWorkingEndDay);

            bool isWorkingday = workingDays.Contains(startDate.DayOfWeek);

            if (isWorkingday &&
                startDate.TimeOfDay >= AppConfig.ClinicWorkingStartTime &&
                endDate.TimeOfDay <= AppConfig.ClinicWorkingEndTime)
            {
                return true;
            }

            return false;
        }


        private List<DayOfWeek> GetDaysBetween(DayOfWeek saturday, DayOfWeek wednesday)
        {
            List<DayOfWeek> daysBetween = new List<DayOfWeek>();

            int startIndex = (int)saturday;
            int endIndex = (int)wednesday;

            if (startIndex < endIndex)
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    daysBetween.Add((DayOfWeek)i);
                }
            }
            else
            {
                for (int i = startIndex; i <= (int)DayOfWeek.Saturday; i++)
                {
                    daysBetween.Add((DayOfWeek)i);
                }
                for (int i = 0; i <= endIndex; i++)
                {
                    daysBetween.Add((DayOfWeek)i);
                }
            }

            return daysBetween;
        }
    }
}