using Domain.Models.Appointments.Enums;
using Domain.Models.Appointments.Rules;
using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;

namespace Domain.Models.Appointments.Entities
{
    public class Doctor : BaseEntity
    {
        public Name Name { get; private set; }
        public DoctorType DoctorType { get; private set; }
        public EmailAddress EmailAddress { get; private set; }
        public List<WeeklyAvailability> Availabilities { get; private set; }

        public virtual ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();


        [Obsolete("Reserved for EF Core", true)]
        private Doctor()
        {
        }

        public Doctor(
            Guid id,
            Name name,
            DoctorType doctorType,
            List<WeeklyAvailability> availabilities ,
            EmailAddress emailAddress,
            IDoctorEmailAddressUniquenessChecker emailAddressUniquenessChecker)
        {
            AssertionConcern.AssertRuleNotBroken(
                new EmailAddressMustBeUniqueRule(emailAddress, emailAddressUniquenessChecker));

            Id = id;
            Name = name;
            DoctorType = doctorType;
            Availabilities = availabilities;
            EmailAddress = emailAddress;
        }
    }
}
