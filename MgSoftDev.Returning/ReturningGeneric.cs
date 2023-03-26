using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MgSoftDev.Returning.Exceptions;
using MgSoftDev.Returning.Helper;
// ReSharper disable ExplicitCallerInfoArgument

namespace MgSoftDev.Returning;


public class Returning< T > : Returning
{
    #region Property

    public T    Value    { get; internal set; }
    public bool OkNotNull=>Ok && Value != null;
    #endregion

    #region Methods

    public new Returning<T> Throw()
    {
        if (ResultType == TypeResult.Error) throw new ReturningErrorException(this);
        if (ResultType == TypeResult.Unfinished) throw new ReturningUnfinishedException(this);

        return this;
    }
   
    
    #region Returning<T> Try

    /// <summary>
    /// Execute a function[T] and catch the exceptions, returning an Returning[T]
    /// </summary>
    /// <param name="methodFunc"></param>
    /// <param name="errorName"></param>
    /// <param name="errorCode"></param>
    /// <param name="memberName"></param>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    /// <returns></returns>
    public static Returning<T> Try(Func<T> methodFunc, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, [ CallerMemberName ] string memberName = null,
                                        [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        try
        {
            var val = methodFunc.Invoke();

            return val;
        }
        catch ( ReturningUnfinishedException e )
        {
            return e.Result.ToReturning<T>();
        }
        catch ( ReturningErrorException e )
        {
            return  e.Result.ToReturning<T>();
        }
        catch ( Exception e )
        {
            return new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber);
        }
    }

    /// <summary>
    /// Execute a function[Task[T]] and catch the exceptions, returning an Returning[T]
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
    public static ReturningAsync<T> TryTask(Func<Task<T>> methodFunc, bool saveLog = false, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, string logName = "",
                                                 [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        return new ReturningAsync<T>(()=>
        {
            try
            {
                try
                {
                    var invoke = methodFunc.Invoke();
                    invoke.Wait();

                    return Returning.Success(invoke.Result);
                }
                catch ( Exception e )
                {
                    throw e.InnerException??e;
                }
            }
            catch ( ReturningUnfinishedException e )
            {
                return e.Result.ToReturning<T>();
            }
            catch ( ReturningErrorException e )
            {
                return saveLog ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, null, logName).ToReturning<T>() : e.Result.ToReturning<T>();
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
    /// Execute a function[T] in new thread and catch the exceptions, returning an Returning[T]
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
    public static ReturningAsync<T> TryTask(Func<T> methodFunc, bool saveLog = false, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, string logName = "",
                                                 [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        return new ReturningAsync<T>(()=>
        {
            try
            {
                var val = methodFunc.Invoke();

                return Returning.Success(val);
            }
            catch ( ReturningUnfinishedException e )
            {
                return  e.Result.ToReturning<T>();
            }
            catch ( ReturningErrorException e )
            {
                return saveLog ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, null, logName).ToReturning<T>() : e.Result.ToReturning<T>();
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
    /// Execute a function[Returning[T]]  and catch the exceptions, returning an Returning[T]
    /// </summary>
    /// <param name="methodFunc"></param>
    /// <param name="errorName"></param>
    /// <param name="errorCode"></param>
    /// <param name="memberName"></param>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Returning<T> Try(Func<Returning<T>> methodFunc, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, [ CallerMemberName ] string memberName = null,
                                        [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        try
        {
            return methodFunc.Invoke();
        }
        catch ( ReturningUnfinishedException e )
        {
            return  e.Result.ToReturning<T>();
        }
        catch ( ReturningErrorException e )
        {
            return e.Result.ToReturning<T>();
        }
        catch ( Exception e )
        {
            return new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber);
        }
    }

    /// <summary>
    /// Execute a function[Task[Returning[T]]] and catch the exceptions, returning an Returning[T]
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
    public static ReturningAsync<T> TryTask(Func<Task<Returning<T>>> methodFunc, bool saveLog = false, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError,
                                                 string logName = "", [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        return new ReturningAsync<T>(()=>
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
                return e.Result.ToReturning<T>();
            }
            catch ( ReturningErrorException e )
            {
                return saveLog ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, null, logName).ToReturning<T>() : e.Result.ToReturning<T>();
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
    /// Execute a function[Returning[T]] in new thread and catch the exceptions, returning an Returning[T]
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
    public static ReturningAsync<T> TryTask(Func<Returning<T>> methodFunc, bool saveLog = false, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError,
                                                 string logName = "", [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        return new ReturningAsync<T>(()=>
        {
            try
            {
                var res = methodFunc.Invoke();
                if (saveLog) res.SaveLog();

                return res;
            }
            catch ( ReturningUnfinishedException e )
            {
                return e.Result.ToReturning<T>();
            }
            catch ( ReturningErrorException e )
            {
                return saveLog ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, null, logName).ToReturning<T>() : e.Result.ToReturning<T>();
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

    public static implicit operator T(Returning<T> value)=>value.Value;

    

    public static implicit operator Returning<T>(T value)=>
        new()
        {
            Value = value
        };

    public static implicit operator Returning<T>(ErrorInfo value)=>
        new()
        {
            ErrorInfo = value
        };

    public static implicit operator Returning<T>(UnfinishedInfo value)=>
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