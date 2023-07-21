namespace Shared.Domain
{
    public sealed class BusinessRuleViolationException : Exception
    {
        public BusinessRuleViolationException(string message)
            : base(message)
        {
            Source = GetType().Name;
        }
    }
}