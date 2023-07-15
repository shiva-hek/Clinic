using Shared.Domain;

namespace Domain.Models.Appointments.ValueObjects
{
    public class Name : ValueObject<Name>
    {
        public string Firstname { get; private set; }

        public string Lastname { get; private set; }

        [Obsolete("Reserved for EF Core", true)]
        private Name()
        {

        }

        public Name(string firstName, string lastName)
        {
            AssertionConcern.AssertArgumentNotEmpty(firstName, $"The {nameof(firstName)} must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(lastName, $"The {nameof(lastName)} must be provided.");
            AssertionConcern.AssertArgumentLength(firstName, 20, $"The {nameof(firstName)} length must be 20 characters or less.");
            AssertionConcern.AssertArgumentLength(lastName, 30, $"The {nameof(lastName)} length must be 30 characters or less.");

            Lastname = lastName.Trim().ToLower();
            Firstname = firstName.Trim().ToLower();
        }
    }
}
