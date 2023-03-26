namespace MgSoftDev.Returning.Exceptions
{
    public class ReturningUnfinishedException : System.Exception
    {
        public UnfinishedInfo Error  { get; set; }
        public Returning      Result { get; set; }

        public ReturningUnfinishedException( Returning result )
        {
            Result = result;
            Error = Result.UnfinishedInfo;
        }

        public override string ToString()
        {
            return $"Title:{Error?.Title}\nMensaje:{Error?.Mensaje}\nErrorCode:{Error?.ErrorCode}\nType:{Error?.Type}";

        }
    }
}
