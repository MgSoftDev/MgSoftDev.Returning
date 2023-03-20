using System;
using System.Runtime.CompilerServices;
using MgSoftDev.Returning.Exceptions;

namespace MgSoftDev.Returning
{
    public class ErrorInfo
    {
        public const string UnhandledError = "UnhandledError";


        public string    ErrorMessage { get; set; }
        public string    ErrorCode    { get; set; }
        public string    MemberName   { get; set; }
        public string    FilePath     { get; set; }
        public int       LineNumber   { get; set; }
        public Exception TryException { get; set; }
        public object    Parameters   { get; set; }

        public ErrorInfo() { }

        public ErrorInfo(string errorMessage, Exception tryException = null, string errorCode = "", [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null,
                         [ CallerLineNumber ] int lineNumber = 0)
        {
            ErrorMessage = errorMessage;
            TryException = tryException;
            MemberName   = memberName;
            FilePath     = filePath;
            LineNumber   = lineNumber;
            ErrorCode    = errorCode;
        }

        public ErrorInfo(string                    errorMessage,    object parameters, Exception tryException = null, string errorCode = "", [ CallerMemberName ] string memberName = null,
                         [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
        {
            ErrorMessage = errorMessage;
            TryException = tryException;
            MemberName   = memberName;
            FilePath     = filePath;
            LineNumber   = lineNumber;
            ErrorCode    = errorCode;
            Parameters   = parameters;
        }

        public void Throw() { throw new ReturningErrorException((Returning)this); }


        #region Overrides of Object

        public override string ToString() { return $"ErrorMessage:{ErrorMessage}\n"             +
                                                   $"TryException:{TryException?.ToString()}\n" +
                                                   $"ErrorCode:{ErrorCode}\n"                   +
                                                   $"Parameters{Parameters != null}\n"          +
                                                   $"MemberName:{MemberName}\n"                 +
                                                   $"LineNumber:{LineNumber}\n"                 + 
                                                   $"FilePath{FilePath}"; 
        }

        #endregion
    }
}
