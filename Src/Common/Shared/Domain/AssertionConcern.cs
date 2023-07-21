using Shared.Exceptions;
using System.Text.RegularExpressions;

namespace Shared.Domain
{
    public class AssertionConcern
    {
        public static void AssertArgumentNotNull(object objValue, ErrorCode errorCode)
        {
            if (objValue is null)
                ThrowException(errorCode);
        }

        public static void AssertArgumentNotEmpty(string stringValue, ErrorCode errorCode)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
                ThrowException(errorCode);
        }

        public static void AssertArgumentIsTrue<TArgument>(TArgument value, Predicate<TArgument> predicate, ErrorCode errorCode)
        {
            if (predicate(value))
                return;

            ThrowException(errorCode);
        }

        public static void AssertArgumentIsTrue(bool booleanValue, ErrorCode errorCode)
        {
            AssertArgumentIsTrue(booleanValue, p => booleanValue, errorCode);
        }

        public static void AssertArgumentLength(string stringValue, int maximum, ErrorCode errorCode)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
                return;

            int len = stringValue.Trim().Length;
            if (len > maximum)
                ThrowException(errorCode);
        }

        public static void AssertArgumentLength(string stringValue, int minimum, int maximum, ErrorCode errorCode)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
                return;

            int len = stringValue.Trim().Length;
            if (len < minimum || len > maximum)
                ThrowException(errorCode);
        }

        public static void AssertArgumentMatches(string stringValue, string pattern, ErrorCode errorCode)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
                return;

            var regex = new Regex(pattern);
            if (!regex.IsMatch(stringValue.Trim()))
                ThrowException(errorCode);
        }

        public static void AssertArgumentRange(int value, int minimum, int maximum, ErrorCode errorCode)
        {
            if (value < minimum || value > maximum)
                ThrowException(errorCode);
        }

        public static void AssertRuleNotBroken(IRule businessRule, ErrorCode errorCode)
        {
            businessRule.Assert(errorCode);
        }

        public static void AssertRuleNotBroken(IRule businessRule)
        {
            businessRule.Assert();
        }

        private static void ThrowException(ErrorCode errorCode)
        {
            if (errorCode is null)
                throw new ApiException();
            else
                throw new ApiException(errorCode);

            //if (string.IsNullOrWhiteSpace(errorCode))
            //    throw new InvalidOperationException();
            //else
            //    throw new InvalidOperationException(errorCode);
        }
    }
}
