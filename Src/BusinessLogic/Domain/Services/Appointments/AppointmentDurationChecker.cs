using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.Enums;
using Domain.Models.Appointments.Interfaces;
using Domain.Models.Appointments.ValueObjects;

namespace Domain.Services.Appointments
{
    public class AppointmentDurationChecker : IAppointmentDurationChecker
    {
        private readonly IDoctorRepository _doctorRepository;

        public AppointmentDurationChecker(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public bool IsValid(Guid doctorId, AppointmentTime appointmentTime)
        {
            Doctor? doctor = _doctorRepository.Get(doctorId);
            TimeSpan expectedDuration = TimeSpan.FromMinutes(appointmentTime.Duration.Minutes);

            if (doctor!.DoctorType == DoctorType.General)
            {
                if (expectedDuration < TimeSpan.FromMinutes(5) || expectedDuration > TimeSpan.FromMinutes(15))
                {
                    return false;
                }
            }
            else
            {
                if (expectedDuration < TimeSpan.FromMinutes(10) || expectedDuration > TimeSpan.FromMinutes(30))
                {
                    return false;
                }
            }

            return true;
        }
    }
}