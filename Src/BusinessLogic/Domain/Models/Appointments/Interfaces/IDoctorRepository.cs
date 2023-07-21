using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.ValueObjects;
using Shared.Domain;

namespace Domain.Models.Appointments.Interfaces;

public interface IDoctorRepository : IRepository<Doctor>
{
    Doctor? Get(EmailAddress emailAddress);
}