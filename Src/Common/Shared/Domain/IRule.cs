namespace Shared.Domain
{
    public interface IRule
    {
        void Assert();
        void Assert(string message);
    }
}
