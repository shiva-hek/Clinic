using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;
using Shared.Exceptions;

namespace Domain.Models.Appointments.Rules;

public class EmailAddressMustBeUniqueRule : IRule
{
    private readonly EmailAddress _emailAddress;
    private readonly IDoctorEmailAddressUniquenessChecker _emailAddressUniquenessChecker;

    public EmailAddressMustBeUniqueRule(EmailAddress emailAddress, IDoctorEmailAddressUniquenessChecker emailAddressUniquenessChecker)
    {
        AssertionConcern.AssertArgumentNotNull(emailAddress, ErrorCode.IsNull(nameof(emailAddress)));

        this._emailAddress = emailAddress;
        this._emailAddressUniquenessChecker = emailAddressUniquenessChecker;
    }
    public void Assert()
    {
        Assert(ErrorCode.DoctorDuplicateEmail);
    }

    public void Assert(ErrorCode errorCode)
    {
        if (!_emailAddressUniquenessChecker.IsUnique(_emailAddress))
            throw new ApiException(errorCode);
    }
}