using System.Text.RegularExpressions;

namespace Shared.Domain
{
    public class AssertionConcern
    {
        public static void AssertArgumentNotNull(object objValue, string message = null)
        {
            if (objValue is null)
                ThrowException(message);
        }

        public static void AssertArgumentNotEmpty(string stringValue, string message = null)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
                ThrowException(message);
        }

        public static void AssertArgumentIsTrue<TArgument>(TArgument value, Predicate<TArgument> predicate, string message = null)
        {
            if (predicate(value))
                return;

            ThrowException(message);
        }

        public static void AssertArgumentIsTrue(bool booleanValue, string message = null)
        {
            AssertArgumentIsTrue(booleanValue, p => booleanValue, message);
        }

        public static void AssertArgumentLength(string stringValue, int maximum, string message = null)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
                return;

            int len = stringValue.Trim().Length;
            if (len > maximum)
                ThrowException(message);
        }

        public static void AssertArgumentLength(string stringValue, int minimum, int maximum, string message = null)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
                return;

            int len = stringValue.Trim().Length;
            if (len < minimum || len > maximum)
                ThrowException(message);
        }

        public static void AssertArgumentMatches(string stringValue, string pattern, string message = null)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
                return;

            var regex = new Regex(pattern);
            if (!regex.IsMatch(stringValue.Trim()))
                ThrowException(message);
        }

        public static void AssertArgumentRange(int value, int minimum, int maximum, string message = null)
        {
            if (value < minimum || value > maximum)
                ThrowException(message);
        }

        public static void AssertRuleNotBroken(IRule businessRule, string message)
        {
            businessRule.Assert(message);
        }

        public static void AssertRuleNotBroken(IRule businessRule)
        {
            businessRule.Assert();
        }

        private static void ThrowException(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new InvalidOperationException();
            else
                throw new InvalidOperationException(message);
        }
    }
}
