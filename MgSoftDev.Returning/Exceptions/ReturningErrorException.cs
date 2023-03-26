namespace MgSoftDev.Returning.Exceptions
{
    public class ReturningErrorException : System.Exception
    {
        public ErrorInfo  Error  { get; set; }
        public Returning Result { get; set; }

        public ReturningErrorException(Returning result)
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
