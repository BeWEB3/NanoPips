using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Common
{
    public class ExchangeException : Exception
    {
        public ErrorCode ErrorCode { get; private set; }
        public string ErrorMessage { get; private set; }

        public ExchangeException(ErrorCode code, Exception innerException) : base(code.ToString(), innerException)
        {
            ErrorCode = code;
            ErrorMessage = code.ToString().Replace("_", " ");
        }
        public  ExchangeException(string message)
        {
            ErrorMessage = message;
        }
        public static void Throw(ErrorCode code,Exception innerException)
        {
            throw new ExchangeException(code, innerException);
        }
        public static void Throw(string msg)
        {
            throw new ExchangeException(msg);
        }

    }
}
