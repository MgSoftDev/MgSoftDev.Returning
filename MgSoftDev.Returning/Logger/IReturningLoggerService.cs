using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MgSoftDev.Returning.Helper;
using MgSoftDev.Returning.Interfaces;

namespace MgSoftDev.Returning.Logger
{
    public interface IReturningLoggerService
    {
        object EventSource { get; set; }
        
        bool       SaveLog(IReturning      returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, object eventSource =null,  string logName = null);
        Task<bool> SaveLogAsync(IReturning returning, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, object eventSource = null, string logName = null);



        bool       SaveLog(UnfinishedInfo      unfinished, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, object eventSource = null, string logName = null);
        Task<bool> SaveLogAsync(UnfinishedInfo unfinished, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, object eventSource = null, string logName = null);

        bool       SaveLog(ErrorInfo      errorInfo, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, object eventSource = null, string logName = null);
        Task<bool> SaveLogAsync(ErrorInfo errorInfo, ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, object eventSource = null, string logName = null);


        bool SaveLog           (string errorMessage, object parameters, Exception tryException = null, string errorCode = "", ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, object eventSource = null, string logName = null, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);
        Task<bool> SaveLogAsync(string errorMessage, object parameters, Exception tryException = null, string errorCode = "", ReturningEnums.LogLevel logLevel = ReturningEnums.LogLevel.Error, object eventSource = null, string logName = null, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);
    }
}
