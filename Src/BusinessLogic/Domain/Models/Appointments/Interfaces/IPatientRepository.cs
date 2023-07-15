using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.ValueObjects;
using Shared.Domain;

namespace Domain.Models.Appointments.Interfaces;

public interface IPatientRepository : IRepository
{
    Patient Get(Guid id);
    Patient Get(EmailAddress emailAddress);
}