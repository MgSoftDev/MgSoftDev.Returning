using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MgSoftDev.ReturningCore.Exceptions;
using MgSoftDev.ReturningCore.Helper;

// ReSharper disable ExplicitCallerInfoArgument

namespace MgSoftDev.ReturningCore;

public partial class Returning 
{
    public enum TypeResult
    {
        Success,
        Error,
        Unfinished
    }
    #region Property

    public UnfinishedInfo        UnfinishedInfo { get;  set; }
    public ErrorInfo             ErrorInfo      { get;  set; }
    public bool                  IsLogStored    { get;  set; }
    public ErrorInfoException             LogException   { get;  set; }
    public bool                  Ok             =>ErrorInfo == null && UnfinishedInfo == null;
    public TypeResult ResultType     =>Ok ? TypeResult.Success : ( ErrorInfo != null ? TypeResult.Error : TypeResult.Unfinished );

    #endregion

    #region Methods

    /// <exception cref="ReturningErrorException">Condition.</exception>
    public Returning Throw()
    {
        if (ResultType == TypeResult.Error) throw new ReturningErrorException(this);
        if (ResultType == TypeResult.Unfinished) throw new ReturningUnfinishedException(this);

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

    public static ReturningList<T> Success< T >(List<T> value)=>
        new()
        {
            Value = value
        };

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

    public static implicit operator Returning(ReturningErrorException      value)=>value.Result;
    public static implicit operator Returning(ReturningUnfinishedException value)=>value.Result;

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

public partial class Returning 
{
    
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
            methodAction?.Invoke();
    
            return Success();
        }
        catch ( ReturningUnfinishedException e )
        {
            return e.Result;
        }
        catch ( ReturningErrorException e )
        {
            return e.Result;
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
    public static Task<Returning> TryTask(Func<Task> methodAction, bool saveLog = false, string errorName = "Unhandled error", string errorCode = "ErrorInfo.UnhandledError", string logName = "",
                                          [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        return Task.Run<Returning>(async ()=>
        {
            try
            {
                try
                {
                    await methodAction.Invoke();
                    return Success();
                }
                catch ( Exception e )
                {
                    throw e.InnerException??e;
                }
            }
            catch ( ReturningUnfinishedException e )
            {
                return e.Result;
            }
            catch ( ReturningErrorException e )
            {
                return saveLog ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, null, logName) : e.Result;
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
    public static Task<Returning> TryTask(Action methodAction, bool saveLog = false, string errorName = "Unhandled error", string errorCode = "ErrorInfo.UnhandledError", string logName = "",
                                          [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        return Task.Run<Returning>(()=>
        {
            try
            {
                methodAction.Invoke();

                return Success();
            }
            catch ( ReturningUnfinishedException e )
            {
                return e.Result;
            }
            catch ( ReturningErrorException e )
            {
                return saveLog ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, null, logName) : e.Result;
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
            return e.Result;
        }
        catch ( ReturningErrorException e )
        {
            return e.Result;
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
    public static Task<Returning> TryTask(Func<Task<Returning>> methodAction, bool saveLog = false, string errorName = "Unhandled error", string errorCode = "ErrorInfo.UnhandledError",
                                          string logName = "", [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        return Task.Run<Returning>(async ()=>
        {
            try
            {
                try
                {
                    var res = await methodAction.Invoke();
                    if (saveLog) res.SaveLog();

                    return res;
                }
                catch ( Exception e )
                {
                    throw e.InnerException??e;
                }
            }
            catch ( ReturningUnfinishedException e )
            {
                return e.Result;
            }
            catch ( ReturningErrorException e )
            {
                return saveLog ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, null, logName) : e.Result;
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
    public static Task<Returning> TryTask(Func<Returning> methodAction, bool saveLog = false, string errorName = "Unhandled error", string errorCode = "ErrorInfo.UnhandledError", string logName = "",
                                          [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        return Task.Run<Returning>(()=>
        {
            try
            {
                var res = methodAction.Invoke();
                if (saveLog) res.SaveLog();

                return res;
            }
            catch ( ReturningUnfinishedException e )
            {
                return e.Result;
            }
            catch ( ReturningErrorException e )
            {
                return saveLog ? e.Result.SaveLog(ReturningEnums.LogLevel.Error, null, logName) : e.Result;
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
