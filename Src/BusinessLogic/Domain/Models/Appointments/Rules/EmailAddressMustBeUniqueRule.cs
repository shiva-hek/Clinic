using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;

namespace Domain.Models.Appointments.Rules;

public class EmailAddressMustBeUniqueRule : IRule
{
    private readonly EmailAddress _emailAddress;
    private readonly IDoctorEmailAddressUniquenessChecker _emailAddressUniquenessChecker;

    public EmailAddressMustBeUniqueRule(EmailAddress emailAddress, IDoctorEmailAddressUniquenessChecker emailAddressUniquenessChecker)
    {
        AssertionConcern.AssertArgumentNotNull(emailAddress, $"The {nameof(emailAddress)} must be provided.");
        AssertionConcern.AssertArgumentNotNull(emailAddressUniquenessChecker, $"The {nameof(emailAddressUniquenessChecker)} must be provided.");

        this._emailAddress = emailAddress;
        this._emailAddressUniquenessChecker = emailAddressUniquenessChecker;
    }
    public void Assert()
    {
        Assert($@"The email address ""{_emailAddress.Value}"" is using by someone else.");
    }

    public void Assert(string message)
    {
        if (!_emailAddressUniquenessChecker.IsUnique(_emailAddress))
            throw new BusinessRuleViolationException(message ?? "");
    }
}