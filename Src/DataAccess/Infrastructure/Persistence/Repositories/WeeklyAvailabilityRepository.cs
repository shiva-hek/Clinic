using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class WeeklyAvailabilityRepository :IWeeklyAvailabilityRepository
{
    private readonly AppointmentDbContext _dbContext;

    public WeeklyAvailabilityRepository(AppointmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<WeeklyAvailability>> GetDoctorWorkingTime(Guid doctorId)
    {
         return await _dbContext.WeeklyAvailabilities
            .Where(x => x.DoctorId == doctorId)
            .ToListAsync();
    }
}