using Domain.Models.Appointments.ValueObjects;
using Shared.Domain;

namespace Domain.Services.Appointments;

public interface IDoctorEmailAddressUniquenessChecker : IDomainService
{
    bool IsUnique(EmailAddress emailAddress);
}