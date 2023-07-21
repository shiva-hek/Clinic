using System.Globalization;

namespace Shared.Exceptions
{
    public class ApiException : Exception
    {
        public string Code { get; set; }

        public ApiException()
        {
        }

        public ApiException(ErrorCode errorCode)
            : this(errorCode.Description, errorCode.Code)
        {

        }

        public ApiException(string message, string code) : base(message)
        {
            Code = code;
        }

        public ApiException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
