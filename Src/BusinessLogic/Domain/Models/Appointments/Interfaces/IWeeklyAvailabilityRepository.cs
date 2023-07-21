using Domain.Models.Appointments.Entities;

namespace Domain.Models.Appointments.Interfaces;

public interface IWeeklyAvailabilityRepository
{
    Task<List<WeeklyAvailability>> GetDoctorWorkingTime(Guid doctorId);
}