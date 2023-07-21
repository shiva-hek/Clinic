using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.Interfaces;
using Domain.Models.Appointments.ValueObjects;

namespace Infrastructure.Persistence.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly AppointmentDbContext _dbContext;

    public PatientRepository(AppointmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Patient? Get(Guid id)
    {
        return _dbContext.Patients.FirstOrDefault(a => a.Id == id);
    }

    public Patient? Get(EmailAddress emailAddress)
    {
        return _dbContext.Patients.FirstOrDefault(a => a.EmailAddress == emailAddress);
    }
}