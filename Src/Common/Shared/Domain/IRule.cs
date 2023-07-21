using Shared.Exceptions;

namespace Shared.Domain
{
    public interface IRule
    {
        void Assert();
        //void Assert(string message);
        void Assert(ErrorCode errorCode);
    }
}
