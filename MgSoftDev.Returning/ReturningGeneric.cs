using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MgSoftDev.Returning.Exceptions;
using MgSoftDev.Returning.Helper;
using MgSoftDev.Returning.Interfaces;

namespace MgSoftDev.Returning;


public class Returning< T > : IReturning<T>
{
    public Returning() { }

    #region Property

    public T                     Value          { get; internal set; }
    public UnfinishedInfo        UnfinishedInfo { get; internal set; }
    public ErrorInfo             ErrorInfo      { get; internal set; }
    public bool                  IsLogStored    { get; set; }
    public Exception             LogException   { get; set; }
    public bool                  Ok             =>ErrorInfo == null && UnfinishedInfo == null;
    public IReturning.TypeResult ResultType     =>Ok ? IReturning.TypeResult.Success : ( ErrorInfo != null ? IReturning.TypeResult.Error : IReturning.TypeResult.Unfinished );
    public bool                  OkNotNull      =>Ok && Value != null;
    #endregion

    #region Methods

    public Returning<T> Throw()
    {
        if (ResultType == IReturning.TypeResult.Error) throw new ReturningErrorException(this);
        if (ResultType == IReturning.TypeResult.Unfinished) throw new ReturningUnfinishedException(this);

        return this;
    }

    #region Returning<T> Try

    /// <summary>
    /// Execute a function<T> and catch the exceptions, returning an Returning<T>
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
            return ( Returning ) e.Result;
        }
        catch ( ReturningErrorException e )
        {
            return ( Returning ) e.Result;
        }
        catch ( Exception e )
        {
            return new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber);
        }
    }


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
                    throw e.InnerException;
                }
            }
            catch ( ReturningUnfinishedException e )
            {
                return ( Returning ) e.Result;
            }
            catch ( ReturningErrorException e )
            {
                return saveLog ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, null, logName) : ( Returning ) e.Result;
            }
            catch ( Exception e )
            {
                var error = new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber);
                if (saveLog) error.SaveLog(ReturningEnums.LogLevel.Error, null, logName);

                return error;
            }
        });
    }


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
                return ( Returning ) e.Result;
            }
            catch ( ReturningErrorException e )
            {
                return saveLog ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, null, logName) : ( Returning ) e.Result;
            }
            catch ( Exception e )
            {
                var error = new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber);
                if (saveLog) error.SaveLog(ReturningEnums.LogLevel.Error, null, logName);

                return error;
            }
        });
    }


    public static Returning<T> Try(Func<Returning<T>> methodFunc, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, [ CallerMemberName ] string memberName = null,
                                   [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        try
        {
            return methodFunc.Invoke();
        }
        catch ( ReturningUnfinishedException e )
        {
            return ( Returning ) e.Result;
        }
        catch ( ReturningErrorException e )
        {
            return ( Returning ) e.Result;
        }
        catch ( Exception e )
        {
            return new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber);
        }
    }

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
                    throw e.InnerException;
                }
            }
            catch ( ReturningUnfinishedException e )
            {
                return ( Returning ) e.Result;
            }
            catch ( ReturningErrorException e )
            {
                return saveLog ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, null, logName) : ( Returning ) e.Result;
            }
            catch ( Exception e )
            {
                var error = new ErrorInfo(errorName, e, errorCode, memberName, filePath, lineNumber);
                if (saveLog) error.SaveLog(ReturningEnums.LogLevel.Error, null, logName);

                return error;
            }
        });
    }

    public static ReturningAsync<T> TryTask(Func<Returning<T>> methodFunc, bool saveLog = false, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, string logName = "",
                                            [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
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
                return ( Returning ) e.Result;
            }
            catch ( ReturningErrorException e )
            {
                return saveLog ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, null, logName) : ( Returning ) e.Result;
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

    public static implicit operator Returning<T>(Returning value)=>
        new()
        {
            ErrorInfo      = value.ErrorInfo,
            UnfinishedInfo = value.UnfinishedInfo
        };

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

    public void Dispose()
    {
        ErrorInfo      = null;
        UnfinishedInfo = null;
        LogException   = null;
        GC.Collect();
    }

    #endregion
}