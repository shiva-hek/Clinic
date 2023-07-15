using Shared.Domain;
using Shared.Tools;

namespace Domain.Models.Appointments.ValueObjects;

public class EmailAddress : ValueObject<EmailAddress>
{
    [Obsolete("Reserved for EF Core", true)]
    private EmailAddress()
    {

    }

    public EmailAddress(string emailAddress)
    {
        AssertionConcern.AssertArgumentNotEmpty(emailAddress, $"The {nameof(emailAddress)} must be provided.");
        AssertionConcern.AssertArgumentLength(emailAddress, 50,
            $"The {nameof(emailAddress)} length must be 50 characters or less.");
        AssertionConcern.AssertArgumentMatches(emailAddress.ToLower(), RegExHelper.Patterns.Email,
            $"The {nameof(emailAddress)} format is invalid.");

        Value = emailAddress.Trim().ToLower();
    }

    public string Value { get; private set; }
}
