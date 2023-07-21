using Domain.Models.Appointments.ValueObjects;

namespace Domain.Services.Appointments;

public interface IPatientEmailAddressUniquenessChecker
{
    bool IsUnique(EmailAddress emailAddress);
}