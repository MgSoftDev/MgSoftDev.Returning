using System;

namespace MgSoftDev.Returning.Helper
{
    public static class Extension
    {
        public static Returning<T> ToReturning<T>(this Returning value)=>
        new ()
        {
            ErrorInfo      = value.ErrorInfo,
            UnfinishedInfo = value.UnfinishedInfo
        };
        public static ReturningList<T> ToReturningList<T>(this Returning value)=>
        new ()
        {
            ErrorInfo      = value.ErrorInfo,
            UnfinishedInfo = value.UnfinishedInfo
        };
        public static void OkNotNullAction<T>(this Returning<T> returning, Action<T> action )
        {
            if (returning.OkNotNull)
                action(returning.Value);
        }


        public static string Format( this string val, params object[] args )
        {
            if( val == null  ) return "";

            return args == null ? val : string.Format( val, args );
        }

        public static ReturningEnums.LogLevel ToLogLevel(this UnfinishedInfo.NotifyType value)
        {
            switch ( value )
            {
                case UnfinishedInfo.NotifyType.Information: return ReturningEnums.LogLevel.Info;
                case UnfinishedInfo.NotifyType.Warning:     return ReturningEnums.LogLevel.Warn;
                case UnfinishedInfo.NotifyType.Error:       return ReturningEnums.LogLevel.Error;
                case UnfinishedInfo.NotifyType.Success:       return ReturningEnums.LogLevel.Info;
                default:                                    return ReturningEnums.LogLevel.Info;
            }
        }

        public static Returning Optimize(this Returning value)
        {
            if (value.ErrorInfo!= null)
                value.ErrorInfo.TryException = new Exception(value.ErrorInfo?.TryException?.Message);
        
            return value;
        }
        public static Returning<T> Optimize<T>(this Returning<T> value)
        {
            if (value.ErrorInfo != null)
                value.ErrorInfo.TryException = new Exception(value.ErrorInfo?.TryException?.Message);
        
            return value;
        }
    }
}
