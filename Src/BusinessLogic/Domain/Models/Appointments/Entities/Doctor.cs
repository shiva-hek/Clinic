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
        public WeeklySchedule WeeklySchedule { get; private set; }
        public EmailAddress EmailAddress { get; private set; }
        public virtual ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();


        [Obsolete("Reserved for EF Core", true)]
        private Doctor()
        {
        }

        public Doctor(
            Guid id,
            Name name,
            DoctorType doctorType,
            WeeklySchedule weeklySchedul,
            EmailAddress emailAddress,
            IDoctorEmailAddressUniquenessChecker emailAddressUniquenessChecker)
        {
            AssertionConcern.AssertArgumentNotNull(Id, $"The {nameof(Id)} must be provided.");
            AssertionConcern.AssertArgumentNotNull(name, $"The {nameof(name)} must be provided.");
            AssertionConcern.AssertArgumentNotNull(doctorType, $"The {nameof(doctorType)} must be provided.");
            AssertionConcern.AssertArgumentNotNull(weeklySchedul, $"The {nameof(weeklySchedul)} must be provided.");
            AssertionConcern.AssertArgumentNotNull(emailAddress, $"The {nameof(emailAddress)} must be provided.");
            AssertionConcern.AssertRuleNotBroken(
                new EmailAddressMustBeUniqueRule(emailAddress, emailAddressUniquenessChecker));

            Id = id;
            Name = name;
            DoctorType = doctorType;
            WeeklySchedule = weeklySchedul;
            EmailAddress = emailAddress;
        }
    }
}
