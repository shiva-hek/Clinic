using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.Interfaces;

namespace Domain.Services.Appointments
{
    public class DoctorTimeChecker : IDoctorTimeChecker
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorTimeChecker(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public bool IsValid(DateTime startTime, DateTime endTime, Guid doctorId)
        {
            Doctor doctor = _doctorRepository.Get(doctorId);
            DayOfWeek dayOfWeek = startTime.DayOfWeek;
            WeeklyAvailability? availability =
                doctor.WeeklySchedule.Availabilities.FirstOrDefault(a => a.Day == dayOfWeek);

            if (availability != null)
            {
                DateTime doctorStartTime = startTime.Date.Add(availability.StartTime);
                DateTime doctorEndTime = startTime.Date.Add(availability.EndTime);

                if (startTime >= doctorStartTime && endTime <= doctorEndTime)
                {
                    return true;
                }
                else return false;
            }

            return true;
        }
    }
}