using Domain.Models.Appointments.Interfaces;
using Domain.Models.Appointments.ValueObjects;

namespace Domain.Services.Appointments
{
    public class PatientEmailAddressUniquenessChecker : IPatientEmailAddressUniquenessChecker
    {
        private readonly IPatientRepository _patientRepository;

        public PatientEmailAddressUniquenessChecker(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        public bool IsUnique(EmailAddress emailAddress)
        {
            return _patientRepository.Get(emailAddress) == null;
        }
    }
}
