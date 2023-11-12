using System;

namespace MgSoftDev.ReturningCore.Helper
{
    public static class Extension
    {
        public static Returning<T> ToReturning<T>(this ReturningBase value)=>
        new ()
        {
            ErrorInfo      = value.ErrorInfo,
            UnfinishedInfo = value.UnfinishedInfo
        };
        public static ReturningList<T> ToReturningList<T>(this ReturningBase value)=>
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
            OptimizeInternal(value);
            return value;
        }
        public static Returning<T> Optimize<T>(this Returning<T> value)
        {
            OptimizeInternal(value);
            return value;
        }
        public static ReturningList<T> Optimize<T>(this ReturningList<T> value)
        {
            OptimizeInternal(value);
            return value;
        }
        public static Returning Anonymous(this Returning value)
        {
            AnonymousInternal(value);
            return value;
        }
        public static Returning<T> Anonymous<T>(this Returning<T> value)
        {
            AnonymousInternal(value);
            return value;
        }
        public static ReturningList<T> Anonymous<T>(this ReturningList<T> value)
        {
            AnonymousInternal(value);
            return value;
        }


        private static void AnonymousInternal(Returning value)
        {
            if (value.ErrorInfo != null)
            {
                value.ErrorInfo.MemberName   = null;
                value.ErrorInfo.FilePath     = null;
                value.ErrorInfo.LineNumber   = 0;
                value.ErrorInfo.Parameters   = null;
                value.ErrorInfo.TryException = null;
            }

            if (value.UnfinishedInfo != null)
            {
                value.UnfinishedInfo.Parameters = null;
            }
            
            value.LogException = null;
        }
        private static void OptimizeInternal(Returning value)
        {
            if (value.ErrorInfo != null)
            {
                value.ErrorInfo.Parameters   = null;
                if(value.ErrorInfo.TryException!= null)
                {
                    value.ErrorInfo.TryException.StackTrace = null;
                    value.ErrorInfo.TryException.InnerException = null;
                }
                
            }

            if (value.UnfinishedInfo != null)
            {
                value.UnfinishedInfo.Parameters = null;
            }
            
            value.LogException = null;
        }
    }
}
