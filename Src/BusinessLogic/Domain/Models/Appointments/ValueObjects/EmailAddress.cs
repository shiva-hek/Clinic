using Shared.Domain;
using Shared.Exceptions;
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
        AssertionConcern.AssertArgumentNotEmpty(emailAddress, ErrorCode.IsNull(nameof(emailAddress)));
        AssertionConcern.AssertArgumentLength(emailAddress, 50,ErrorCode.EmailLength);
        AssertionConcern.AssertArgumentMatches(emailAddress.ToLower(), RegExHelper.Patterns.Email,ErrorCode.InvalidEmail);

        Value = emailAddress.Trim().ToLower();
    }

    public string Value { get; private set; }
}
