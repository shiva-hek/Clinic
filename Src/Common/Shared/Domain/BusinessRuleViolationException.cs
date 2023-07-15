namespace Shared.Domain
{
    public class BusinessRuleViolationException : Exception
    {
        public BusinessRuleViolationException(string message)
            : base(message)
        {
            Source = GetType().Name;
        }
    }
}