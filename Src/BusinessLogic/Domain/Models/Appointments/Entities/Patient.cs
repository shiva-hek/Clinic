using Domain.Models.Appointments.Enums;
using Domain.Models.Appointments.Rules;
using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;

namespace Domain.Models.Appointments.Entities
{
    public class Patient : BaseEntity
    {
        public Name Name { get; private set; }
        public Gender Gender { get; private set; }
        public EmailAddress EmailAddress { get; private set; }

        [Obsolete("Reserved for EF Core", true)]
        private Patient()
        {
        }

        public Patient(
            Guid id,
            Name name,
            Gender gender,
            EmailAddress emailAddress,
            IPatientEmailAddressUniquenessChecker emailAddressUniquenessChecker)
        {
            AssertionConcern.AssertRuleNotBroken(
                new PatientEmailAddressMustBeUniqueRule(emailAddress, emailAddressUniquenessChecker));

            Id = id;
            Name = name;
            Gender = gender;
            EmailAddress = emailAddress;
        }
    }
}