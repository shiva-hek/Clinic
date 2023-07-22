using Domain.Models.Appointments.ValueObjects;

namespace Domain.Services.Appointments;

public interface IDoctorEmailAddressUniquenessChecker
{
    bool IsUnique(EmailAddress emailAddress);
}