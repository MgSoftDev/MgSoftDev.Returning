using System;
using System.Runtime.CompilerServices;
using MgSoftDev.ReturningCore.Exceptions;

namespace MgSoftDev.ReturningCore
{
    public class ErrorInfo
    {
        public const string UnhandledError = "UnhandledError";


        public string             ErrorMessage { get; set; }
        public string             ErrorCode    { get; set; }
        public string             MemberName   { get; set; }
        public string             FilePath     { get; set; }
        public int                LineNumber   { get; set; }
        public ErrorInfoException TryException { get; set; }
        public object             Parameters   { get; set; }

        public ErrorInfo() { }

        public ErrorInfo(string errorMessage, Exception tryException = null, string errorCode = "", [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null,
                         [ CallerLineNumber ] int lineNumber = 0)
        {
            ErrorMessage = errorMessage;
            TryException = new ErrorInfoException(tryException);
            MemberName   = memberName;
            FilePath     = filePath;
            LineNumber   = lineNumber;
            ErrorCode    = errorCode;
        }

        public ErrorInfo(string                    errorMessage,    object parameters, Exception tryException = null, string errorCode = "", [ CallerMemberName ] string memberName = null,
                         [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
        {
            ErrorMessage = errorMessage;
            TryException = new ErrorInfoException(tryException);
            MemberName   = memberName;
            FilePath     = filePath;
            LineNumber   = lineNumber;
            ErrorCode    = errorCode;
            Parameters   = parameters;
        }

        public void Throw() { throw new ReturningErrorException(this); }


        #region Overrides of Object

        public override string ToString() { return $"ErrorMessage:{ErrorMessage}\n"             +
                                                   $"TryException:{TryException}\n" +
                                                   $"ErrorCode:{ErrorCode}\n"                   +
                                                   $"Parameters{Parameters != null}\n"          +
                                                   $"MemberName:{MemberName}\n"                 +
                                                   $"LineNumber:{LineNumber}\n"                 + 
                                                   $"FilePath{FilePath}"; 
        }

        #endregion
    }

    public class ErrorInfoException
    {
        public ErrorInfoException(Exception ex)
        {
            Message = ex.Message;
            Source  = ex.Source;
            StackTrace = ex.StackTrace;
            ExceptionType = ex.GetType().FullName;
            if (ex.InnerException != null)
                InnerException = new ErrorInfoException()
                {
                    Message = ex.InnerException.Message,
                    Source  = ex.InnerException.Source,
                    StackTrace = ex.InnerException.StackTrace,
                    ExceptionType = ex.InnerException.GetType().FullName,
                };
        }

        public ErrorInfoException()
        {
            
        }
        public string             Message        { get; set; }
        public string             Source         { get; set; }
        public string             StackTrace     { get; set; }
        public string             ExceptionType  { get; set; }
        public ErrorInfoException InnerException { get; set; }
    }
}
