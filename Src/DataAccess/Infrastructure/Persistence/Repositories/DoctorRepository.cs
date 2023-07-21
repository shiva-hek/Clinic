using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.Interfaces;
using Domain.Models.Appointments.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class DoctorRepository :IDoctorRepository
{
    private readonly AppointmentDbContext _dbContext;

    public DoctorRepository(AppointmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Doctor? Get(Guid id)
    {
        return _dbContext.Doctors
            .Include(a=>a.Availabilities)
            .FirstOrDefault(a => a.Id == id);
    }

    public Doctor? Get(EmailAddress emailAddress)
    {
        return _dbContext.Doctors.FirstOrDefault(a => a.EmailAddress==emailAddress);
    }

    
}