using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MgSoftDev.Returning.Exceptions;
using MgSoftDev.Returning.Helper;

// ReSharper disable ExplicitCallerInfoArgument

namespace MgSoftDev.Returning;

public partial class Returning : IReturning
{
    #region Property

    public UnfinishedInfo        UnfinishedInfo { get; set; }
    public ErrorInfo             ErrorInfo      { get; set; }
    public bool                  IsLogStored    { get; set; }
    public Exception             LogException   { get; set; }
    public bool                  Ok             =>ErrorInfo == null && UnfinishedInfo == null;
    public IReturning.TypeResult ResultType     =>Ok ? IReturning.TypeResult.Success : ( ErrorInfo != null ? IReturning.TypeResult.Error : IReturning.TypeResult.Unfinished );

    #endregion

    #region Methods

    /// <exception cref="ReturningErrorException">Condition.</exception>
    public Returning Throw()
    {
        if (ResultType == IReturning.TypeResult.Error) throw new ReturningErrorException(this);
        if (ResultType == IReturning.TypeResult.Unfinished) throw new ReturningUnfinishedException(this);

        return this;
    }

    #endregion

    #region Static Metodos

    public static Returning Success()=>new();

    public static Returning<T> Success< T >(T value)=>
        new()
        {
            Value = value
        };

    #region Try

    /// <summary>
    /// Execute a void method and catch the exceptions, returning an IReturning
    /// </summary>
    /// <param name="methodAction"></param>
    /// <param name="errorName"></param>
    /// <param name="errorCode"></param>
    /// <param name="memberName"></param>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    /// <returns></returns>
    public static Returning Try(Action methodAction, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, [ CallerMemberName ] string memberName = null,
                                [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        try
        {
            methodAction.Invoke();

            return Success();
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
            return Error(errorName, e, errorCode, memberName, filePath, lineNumber);
        }
    }

    /// <summary>
    /// Execute a Task method and catch the exceptions, returning an Returning
    /// </summary>
    /// <param name="methodAction"></param>
    /// <param name="saveLog"></param>
    /// <param name="errorName"></param>
    /// <param name="errorCode"></param>
    /// <param name="logName"></param>
    /// <param name="memberName"></param>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    /// <returns></returns>
    public static ReturningAsync TryTask(Func<Task> methodAction, bool saveLog = false, string errorName = "Unhandled error", string errorCode = "ErrorInfo.UnhandledError", string logName = "",
                                         [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        return new ReturningAsync(()=>
        {
            try
            {
                try
                {
                    var task = methodAction.Invoke();
                    task.Wait();

                    return Success();
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
                // throw e.InnerException;
                var error = Error(errorName, e, errorCode, memberName, filePath, lineNumber);
                if (saveLog) error.SaveLog(ReturningEnums.LogLevel.Error, null, logName);

                return error;
            }
        });
    }

    /// <summary>
    /// Execute a void method in new task and catch the exceptions, returning an Returning
    /// </summary>
    /// <param name="methodAction"></param>
    /// <param name="saveLog"></param>
    /// <param name="errorName"></param>
    /// <param name="errorCode"></param>
    /// <param name="logName"></param>
    /// <param name="memberName"></param>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    /// <returns></returns>
    public static ReturningAsync TryTask(Action methodAction, bool saveLog = false, string errorName = "Unhandled error", string errorCode = "ErrorInfo.UnhandledError", string logName = "",
                                         [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        return new ReturningAsync(()=>
        {
            try
            {
                methodAction?.Invoke();

                return Success();
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

                return ( Returning ) error;
            }
        });
    }


    /// <summary>
    /// Execute a Function that return Returning    and catch the exceptions, returning an Returning
    /// </summary>
    /// <param name="methodAction"></param>
    /// <param name="errorName"></param>
    /// <param name="errorCode"></param>
    /// <param name="memberName"></param>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    /// <returns></returns>
    public static Returning Try(Func<Returning> methodAction, string errorName = "Unhandled error", string errorCode = "ErrorInfo.UnhandledError", [ CallerMemberName ] string memberName = null,
                                [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        try
        {
            return methodAction.Invoke();
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

    /// <summary>
    /// Execute a Function that return Task[Returning]    and catch the exceptions, returning an Returning
    /// </summary>
    /// <param name="methodAction"></param>
    /// <param name="saveLog"></param>
    /// <param name="errorName"></param>
    /// <param name="errorCode"></param>
    /// <param name="logName"></param>
    /// <param name="memberName"></param>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    /// <returns></returns>
    public static ReturningAsync TryTask(Func<Task<Returning>> methodAction, bool saveLog = false, string errorName = "Unhandled error", string errorCode = "ErrorInfo.UnhandledError",
                                         string logName = "", [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        return new ReturningAsync(()=>
        {
            try
            {
                try
                {
                    var task = methodAction.Invoke();
                    task.Wait();
                    var res = task.Result;
                    if (saveLog) res.SaveLog();

                    return res;
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

    /// <summary>
    ///  Execute in new Thread a Function that return Returning    and catch the exceptions, returning an Returning
    /// </summary>
    /// <param name="methodAction"></param>
    /// <param name="saveLog"></param>
    /// <param name="errorName"></param>
    /// <param name="errorCode"></param>
    /// <param name="logName"></param>
    /// <param name="memberName"></param>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    /// <returns></returns>
    public static ReturningAsync TryTask(Func<Returning> methodAction, bool saveLog = false, string errorName = "Unhandled error", string errorCode = "ErrorInfo.UnhandledError", string logName = "",
                                         [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        return new ReturningAsync(()=>
        {
            try
            {
                var res = methodAction.Invoke();
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


    #region Error

    public static ErrorInfo Error(string errorMessage, Exception tryException = null, string errorCode = "", [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null,
                                  [ CallerLineNumber ] int lineNumber = 0)=>
        new(errorMessage, tryException, errorCode, memberName, filePath, lineNumber);


    public static ErrorInfo Error(string                    errorMessage,    object parameters, Exception tryException = null, string errorCode = "", [ CallerMemberName ] string memberName = null,
                                  [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)=>
        new(errorMessage, parameters, tryException, errorCode, memberName, filePath, lineNumber);

    #endregion

    #region Unfinished

    public static UnfinishedInfo Unfinished(string title, string mensaje, UnfinishedInfo.NotifyType notifyType = UnfinishedInfo.NotifyType.Information, string errorCode = null)=>
        new(title, mensaje, notifyType, errorCode);


    public static UnfinishedInfo Unfinished(string title, UnfinishedInfo.NotifyType notifyType = UnfinishedInfo.NotifyType.Information, string errorCode = null)=>new(title, notifyType, errorCode);

    public static UnfinishedInfo UnfinishedLocalization(string                    titleKey, string mensajeKey, object[] titleFormatArgs = null, object[] mensajeFormatArgs = null,
                                                        UnfinishedInfo.NotifyType notifyType = UnfinishedInfo.NotifyType.Information, string errorCode = null)=>
        UnfinishedInfo.FromLocalization(titleKey, mensajeKey, titleFormatArgs, mensajeFormatArgs, notifyType, errorCode);

    public static UnfinishedInfo UnfinishedLocalization(string titleKey, object[] titleFormatArgs = null, UnfinishedInfo.NotifyType notifyType = UnfinishedInfo.NotifyType.Information,
                                                        string errorCode = null)=>
        UnfinishedInfo.FromLocalization(titleKey, titleFormatArgs, notifyType, errorCode);

    #endregion

    #endregion

    #region Operators Overloading

    public static implicit operator Returning(ErrorInfo value)=>
        new()
        {
            ErrorInfo = value
        };

    public static implicit operator Returning(UnfinishedInfo value)=>
        new()
        {
            UnfinishedInfo = value
        };

    public static implicit operator Returning(ReturningErrorException      value)=>( Returning ) value.Result;
    public static implicit operator Returning(ReturningUnfinishedException value)=>( Returning ) value.Result;

    #endregion

    #region Overrides of Object

    public override string ToString()
    {
        return $"ResultType:{ResultType}\n" + $"OK:{Ok}\n" + $"UnfinishedInfo:{UnfinishedInfo}\n" + $"ErrorInfo:{ErrorInfo}\n" + $"IsLogStored:{IsLogStored}\n" + $"LogException:{LogException}\n";
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        ErrorInfo      = null;
        UnfinishedInfo = null;
        LogException   = null;
    }

    #endregion
}

public partial class Returning : IReturning
{
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
    public static Returning<T> Try<T>(Func<T> methodFunc, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, [ CallerMemberName ] string memberName = null,
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
    public static ReturningAsync<T> TryTask<T>(Func<Task<T>> methodFunc, bool saveLog = false, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, string logName = "",
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
    public static ReturningAsync<T> TryTask<T>(Func<T> methodFunc, bool saveLog = false, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, string logName = "",
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
    public static Returning<T> Try<T>(Func<Returning<T>> methodFunc, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError, [ CallerMemberName ] string memberName = null,
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
    public static ReturningAsync<T> TryTask<T>(Func<Task<Returning<T>>> methodFunc, bool saveLog = false, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError,
                                               string logName = "", [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        return new ReturningAsync<T>( ()=>
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
    public static ReturningAsync<T> TryTask<T>(Func<Returning<T>> methodFunc, bool saveLog = false, string errorName = "Unhandled error", string errorCode = ErrorInfo.UnhandledError,
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
}

public class Returning< T > : IReturning<T>
{
    public Returning() { }

    #region Property

    public T                     Value          { get; internal set; }
    public UnfinishedInfo        UnfinishedInfo { get; internal set; }
    public ErrorInfo             ErrorInfo      { get; internal set; }
    public bool                  IsLogStored    { get; set; }
    public Exception             LogException   { get; internal set; }
    public bool                  Ok             =>ErrorInfo == null && UnfinishedInfo == null;
    public IReturning.TypeResult ResultType     =>Ok ? IReturning.TypeResult.Success : ( ErrorInfo != null ? IReturning.TypeResult.Error : IReturning.TypeResult.Unfinished );

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
        return new ReturningAsync<T>( ()=>
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

public interface IReturning< out T > : IReturning
{
    T Value { get; }
}

public interface IReturning : IDisposable
{
    public enum TypeResult
    {
        Success,
        Error,
        Unfinished
    }

    UnfinishedInfo UnfinishedInfo { get; }
    ErrorInfo      ErrorInfo      { get; }
    bool           Ok             { get; }
    TypeResult     ResultType     { get; }
}
