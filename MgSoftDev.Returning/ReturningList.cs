using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MgSoftDev.Returning.Exceptions;
using MgSoftDev.Returning.Helper;
// ReSharper disable ExplicitCallerInfoArgument

namespace MgSoftDev.Returning;

public class ReturningList< T > : Returning
{
    #region Property

    public List<T> Value { get; internal set; }

    public bool OkNotNull=>Ok && Value != null;

    #endregion

    #region Methods

    public new ReturningList<T> Throw()
    {
        if (ResultType == TypeResult.Error) throw new ReturningErrorException(this);
        if (ResultType == TypeResult.Unfinished) throw new ReturningUnfinishedException(this);

        return this;
    }


    #region ReturningList<T> Try

    /// <summary>
    /// Execute a function[List[T]] and catch the exceptions, returning an ReturningList[T]
    /// </summary>
    /// <param name="methodFunc"></param>
    /// <param name="errorName"></param>
    /// <param name="errorCode"></param>
    /// <param name="memberName"></param>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    /// <returns></returns>
    public static ReturningList<T> Try(Func<List<T>> methodFunc, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, [ CallerMemberName ] string memberName = null,
                                            [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        try
        {
            var val = methodFunc.Invoke();

            return val;
        }
        catch ( ReturningUnfinishedException e )
        {
            return e.Result.ToReturningList<T>();
        }
        catch ( ReturningErrorException e )
        {
            return e.Result.ToReturningList<T>();
        }
        catch ( Exception e )
        {
            return new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber);
        }
    }

    /// <summary>
    /// Execute a function[Task[List[T]]] and catch the exceptions, returning an ReturningList[T]
    /// </summary>
    /// <param name="methodFunc"></param>
    /// <param name="saveLog"></param>
    /// <param name="errorName"></param>
    /// <param name="errorCode"></param>
    /// <param name="logName"></param>
    /// <param name="memberName"></param>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static ReturningListAsync<T> TryTask(Func<Task<List<T>>>      methodFunc,      bool saveLog = false, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError,
                                                     string                   logName    = "", [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null,
                                                     [ CallerLineNumber ] int lineNumber = 0)
    {
        return new ReturningListAsync<T>(()=>
        {
            try
            {
                try
                {
                    var invoke = methodFunc.Invoke();
                    invoke.Wait();

                    return Success(invoke.Result);
                }
                catch ( Exception e )
                {
                    throw e.InnerException??e;
                }
            }
            catch ( ReturningUnfinishedException e )
            {
                return e.Result.ToReturningList<T>();
            }
            catch ( ReturningErrorException e )
            {
                return saveLog
                           ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, null, logName)
                              .ToReturningList<T>()
                           : e.Result.ToReturningList<T>();
            }
            catch ( Exception e )
            {
                var error = new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber);
                if (saveLog) error.SaveLog(ReturningEnums.LogLevel.Error, null, logName);

                return error;
            }
        });
    }

    /// <summary>
    /// Execute a function[List[T]] in new thread and catch the exceptions, returning an ReturningList[T]
    /// </summary>
    /// <param name="methodFunc"></param>
    /// <param name="saveLog"></param>
    /// <param name="errorName"></param>
    /// <param name="errorCode"></param>
    /// <param name="logName"></param>
    /// <param name="memberName"></param>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ReturningListAsync<T> TryTask(Func<List<T>>            methodFunc,      bool saveLog = false, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError,
                                                     string                   logName    = "", [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null,
                                                     [ CallerLineNumber ] int lineNumber = 0)
    {
        return new ReturningListAsync<T>(()=>
        {
            try
            {
                var val = methodFunc.Invoke();

                return Success(val);
            }
            catch ( ReturningUnfinishedException e )
            {
                return e.Result.ToReturningList<T>();
            }
            catch ( ReturningErrorException e )
            {
                return saveLog
                           ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, null, logName)
                              .ToReturningList<T>()
                           : e.Result.ToReturningList<T>();
            }
            catch ( Exception e )
            {
                var error = new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber);
                if (saveLog) error.SaveLog(ReturningEnums.LogLevel.Error, null, logName);

                return error;
            }
        });
    }

    /// <summary>
    /// Execute a function[ReturningList[T]] and catch the exceptions, returning an ReturningList[T]
    /// </summary>
    /// <param name="methodFunc"></param>
    /// <param name="errorName"></param>
    /// <param name="errorCode"></param>
    /// <param name="memberName"></param>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ReturningList<T> Try(Func<ReturningList<T>>      methodFunc,        string                    errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError,
                                            [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath  = null,              [ CallerLineNumber ] int lineNumber = 0)
    {
        try
        {
            return methodFunc.Invoke();
        }
        catch ( ReturningUnfinishedException e )
        {
            return e.Result.ToReturningList<T>();
        }
        catch ( ReturningErrorException e )
        {
            return e.Result.ToReturningList<T>();
        }
        catch ( Exception e )
        {
            return new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber);
        }
    }

    /// <summary>
    /// Execute a function[Task[ReturningList[T]]] and catch the exceptions, returning an ReturningList[T]
    /// </summary>
    /// <param name="methodFunc"></param>
    /// <param name="saveLog"></param>
    /// <param name="errorName"></param>
    /// <param name="errorCode"></param>
    /// <param name="logName"></param>
    /// <param name="memberName"></param>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static ReturningListAsync<T> TryTask(Func<Task<ReturningList<T>>> methodFunc, bool saveLog = false, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError,
                                                     string                       logName    = "", [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null,
                                                     [ CallerLineNumber ] int     lineNumber = 0)
    {
        return new ReturningListAsync<T>(()=>
        {
            try
            {
                try
                {
                    var res = methodFunc.Invoke();
                    res.Wait();
                    if (saveLog) res.Result.SaveLog();

                    return res.Result;
                }
                catch ( Exception e )
                {
                    throw e.InnerException??e;
                }
            }
            catch ( ReturningUnfinishedException e )
            {
                return e.Result.ToReturningList<T>();
            }
            catch ( ReturningErrorException e )
            {
                return saveLog
                           ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, null, logName)
                              .ToReturningList<T>()
                           : e.Result.ToReturningList<T>();
            }
            catch ( Exception e )
            {
                var error = new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber);
                if (saveLog) error.SaveLog(ReturningEnums.LogLevel.Error, null, logName);

                return error;
            }
        });
    }

    /// <summary>
    /// Execute a function[ReturningList[T]] in new thread and catch the exceptions, returning an ReturningList[T]
    /// </summary>
    /// <param name="methodFunc"></param>
    /// <param name="saveLog"></param>
    /// <param name="errorName"></param>
    /// <param name="errorCode"></param>
    /// <param name="logName"></param>
    /// <param name="memberName"></param>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ReturningListAsync<T> TryTask(Func<ReturningList<T>>   methodFunc,      bool saveLog = false, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError,
                                                     string                   logName    = "", [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null,
                                                     [ CallerLineNumber ] int lineNumber = 0)
    {
        return new ReturningListAsync<T>(()=>
        {
            try
            {
                var res = methodFunc.Invoke();
                if (saveLog) res.SaveLog();

                return res;
            }
            catch ( ReturningUnfinishedException e )
            {
                return e.Result.ToReturningList<T>();
            }
            catch ( ReturningErrorException e )
            {
                return saveLog
                           ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, null, logName)
                              .ToReturningList<T>()
                           : e.Result.ToReturningList<T>();
            }
            catch ( Exception e )
            {
                var error = new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber);
                if (saveLog) error.SaveLog(ReturningEnums.LogLevel.Error, null, logName);

                return error;
            }
        });
    }

    #endregion

    #endregion

    #region Operators Overloading

    public static implicit operator List<T>(ReturningList<T> value)=>value.Value;


    public static implicit operator ReturningList<T>(Returning<List<T>> value)=>
        new()
        {
            ErrorInfo      = value.ErrorInfo,
            UnfinishedInfo = value.UnfinishedInfo,
            Value          = value.Value
        };

    public static implicit operator ReturningList<T>(List<T> value)=>
        new()
        {
            Value = value
        };

    public static implicit operator ReturningList<T>(ErrorInfo value)=>
        new()
        {
            ErrorInfo = value
        };

    public static implicit operator ReturningList<T>(UnfinishedInfo value)=>
        new()
        {
            UnfinishedInfo = value
        };

    #endregion


    public override string ToString()
    {
        return $"HasValue:{Value != null}\n"        +
               $"ResultType:{ResultType}\n"         +
               $"OK:{Ok}\n"                         +
               $"UnfinishedInfo:{UnfinishedInfo}\n" +
               $"ErrorInfo:{ErrorInfo}\n"           +
               $"IsLogStored:{IsLogStored}\n"       +
               $"LogException:{LogException}\n";
    }

    #region IDisposable

    public new void Dispose()
    {
        ErrorInfo      = null;
        UnfinishedInfo = null;
        LogException   = null;
        GC.Collect();
    }

    #endregion
}
