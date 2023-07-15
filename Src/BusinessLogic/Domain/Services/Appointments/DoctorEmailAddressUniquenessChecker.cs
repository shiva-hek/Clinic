using Domain.Models.Appointments.Interfaces;
using Domain.Models.Appointments.ValueObjects;

namespace Domain.Services.Appointments;

public class DoctorEmailAddressUniquenessChecker: IDoctorEmailAddressUniquenessChecker
{
    private readonly IDoctorRepository _doctorRepository;

    public DoctorEmailAddressUniquenessChecker(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }
    public bool IsUnique(EmailAddress emailAddress)
    {
        return _doctorRepository.Get(emailAddress) == null;
    }
}