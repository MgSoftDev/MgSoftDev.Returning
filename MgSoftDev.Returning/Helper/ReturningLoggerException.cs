using System.Reflection;
using System.Threading.Tasks;
using MgSoftDev.ReturningCore.Logger;

namespace MgSoftDev.ReturningCore.Helper
{
    public static class ReturningLoggerException
    {
        public static Returning SaveLog(this Returning returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, object eventSource = null, string logName = null)
        {
            var obj = returning;

            if (obj == null || obj.IsLogStored || ReturningLogger.LoggerService == null) return obj;

            logName = logName ??
                      Assembly.GetCallingAssembly()
                              .GetName()
                              .Name;
            if (ReturningLogger.LoggerService.SaveLog(obj, logLevel, eventSource, logName)) obj.IsLogStored = true;

            return obj;
        }
        public static async Task<Returning> SaveLogAsync(this Returning returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, object eventSource = null, string logName = null)
        {
            var obj = returning;

            if (obj == null || obj.IsLogStored || ReturningLogger.LoggerService == null) return obj;


            logName = logName ??
                      Assembly.GetCallingAssembly()
                              .GetName()
                              .Name;
            if (await ReturningLogger.LoggerService.SaveLogAsync(obj, logLevel, eventSource, logName)) obj.IsLogStored = true;

            return obj;
        }
        
        public static Returning<T> SaveLog<T>(this Returning<T> returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, object eventSource = null, string logName = null)
        {
            if (returning == null || returning.IsLogStored || ReturningLogger.LoggerService == null) return returning;
            logName = logName ?? Assembly.GetCallingAssembly().GetName().Name;
            ((Returning)returning).SaveLog(logLevel, eventSource, logName);

            return returning;

        }
        public static async Task<Returning<T>> SaveLogAsync< T >(this Returning<T> returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, object eventSource = null,
                                                                 string            logName = null)
        {
            if (returning == null || returning.IsLogStored || ReturningLogger.LoggerService == null) return returning;
        
            logName = logName ??
                      Assembly.GetCallingAssembly()
                              .GetName()
                              .Name;
            await ((Returning)returning).SaveLogAsync(logLevel, eventSource, logName);

            return returning;

        }
        
        
        public static ReturningList<T> SaveLog<T>(this ReturningList<T> returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, object eventSource = null, string logName = null)
        {
            if (returning == null || returning.IsLogStored || ReturningLogger.LoggerService == null) return returning;
            logName = logName ?? Assembly.GetCallingAssembly().GetName().Name;
            ((Returning)returning).SaveLog(logLevel, eventSource, logName);

            return returning;

        }
        public static async Task<ReturningList<T>> SaveLogAsync< T >(this ReturningList<T> returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, object eventSource = null,
                                                                     string                logName = null)
        {
            if (returning == null || returning.IsLogStored || ReturningLogger.LoggerService == null) return returning;
        
            logName = logName ??
                      Assembly.GetCallingAssembly()
                              .GetName()
                              .Name;
             await ( (Returning)returning ).SaveLogAsync(logLevel, eventSource, logName);

             return returning;

        }
        
        
        public static ErrorInfo SaveLog(this ErrorInfo error, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, object eventSource = null, string logName = null)
        {
            if (error == null || ReturningLogger.LoggerService == null) return error;

            logName = logName ??
                      Assembly.GetCallingAssembly()
                              .GetName()
                              .Name;
            ReturningLogger.LoggerService.SaveLog(error, logLevel, eventSource, logName);

            return error;
        }
        public static async Task<ErrorInfo> SaveLogAsync(this ErrorInfo error, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, object eventSource = null, string logName = null)
        {
            if (error == null || ReturningLogger.LoggerService == null) return error;

            logName = logName ??
                      Assembly.GetCallingAssembly()
                              .GetName()
                              .Name;
            await ReturningLogger.LoggerService.SaveLogAsync(error, logLevel, eventSource, logName);

            return error;
        }
        
        public static UnfinishedInfo SaveLog(this UnfinishedInfo unfinished, object eventSource = null, string logName = null)
        {
            if (unfinished == null || ReturningLogger.LoggerService == null) return unfinished;

            logName = logName ??
                      Assembly.GetCallingAssembly()
                              .GetName()
                              .Name;


            ReturningLogger.LoggerService.SaveLog(unfinished, unfinished.Type.ToLogLevel(), eventSource, logName);

            return unfinished;
        }
        public static async Task<UnfinishedInfo> SaveLogAsync(this UnfinishedInfo unfinished, object eventSource = null, string logName = null)
        {
            if (unfinished == null || ReturningLogger.LoggerService == null) return unfinished;

            logName = logName ??
                      Assembly.GetCallingAssembly()
                              .GetName()
                              .Name;

            await ReturningLogger.LoggerService.SaveLogAsync(unfinished, unfinished.Type.ToLogLevel(), eventSource, logName);

            return unfinished;
        }
        
        
    }
}
