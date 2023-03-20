using MgSoftDev.Returning.Interfaces;

namespace MgSoftDev.Returning.Exceptions
{
    public class ReturningErrorException : System.Exception
    {
        public ErrorInfo  Error  { get; set; }
        public IReturning Result { get; set; }

        public ReturningErrorException(IReturning result)
        {
            Result = result;
            Error  = Result.ErrorInfo;
        }

        public override string ToString()
        {
            return
                $"ErrorMessage:{Error?.ErrorMessage}\nErrorCode:{Error?.ErrorCode}\nMemberName:{Error?.MemberName}\nLineNumber:{Error?.LineNumber}\nFilePath:{Error?.FilePath}\nTryException:{Error?.TryException}";
        }
    }
}
