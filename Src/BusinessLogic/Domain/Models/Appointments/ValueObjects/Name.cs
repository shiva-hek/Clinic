using Shared.Domain;
using Shared.Exceptions;

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
            AssertionConcern.AssertArgumentNotEmpty(firstName, ErrorCode.IsEmpty(nameof(firstName)));
            AssertionConcern.AssertArgumentNotEmpty(lastName, ErrorCode.IsEmpty(nameof(lastName)));
            AssertionConcern.AssertArgumentLength(firstName, 20, ErrorCode.FirstnameLength);
            AssertionConcern.AssertArgumentLength(lastName, 30, ErrorCode.LastnameLength);

            Lastname = lastName.Trim().ToLower();
            Firstname = firstName.Trim().ToLower();
        }
    }
}
