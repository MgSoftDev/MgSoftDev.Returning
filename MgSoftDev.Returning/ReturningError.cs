using System;
using System.Runtime.CompilerServices;

namespace MgSoftDev.ReturningCore;

public class ReturningError
{
    #region Ctor
    
    public ReturningError(ErrorInfo error)
    {
        Error = error;
    }
    public ReturningError(UnfinishedInfo unfinished)
    {
        Unfinished = unfinished;
    }
    public ReturningError(ErrorInfo error, UnfinishedInfo unfinished)
    {
        Error      = error;
        Unfinished = unfinished;
    }
    

    #endregion

    #region Statics

    #region ErrorInfo

    public static ReturningError ErrorInfo(string errorMessage, Exception tryException = null, string errorCode = "", [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null,
                                           [ CallerLineNumber ] int lineNumber = 0)
    {
        return new ReturningError(new ErrorInfo(errorMessage, tryException, errorCode, memberName, filePath, lineNumber));
    }

    public static ReturningError ErrorInfo(string                    errorMessage,    object parameters, Exception tryException = null, string errorCode = "", [ CallerMemberName ] string memberName = null,
                                           [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        return new ReturningError(new ErrorInfo(errorMessage, parameters, tryException, errorCode, memberName, filePath, lineNumber));
    }

    #endregion

    #region UnfinishedInfo

    public static ReturningError UnfinishedInfo(string title, string mensaje, UnfinishedInfo.NotifyType notifyType = ReturningCore.UnfinishedInfo.NotifyType.Information, string errorCode = null)
    {
        return new ReturningError(new UnfinishedInfo(title, mensaje, notifyType, errorCode));
    }
    public static ReturningError UnfinishedInfo(string title, UnfinishedInfo.NotifyType notifyType = ReturningCore.UnfinishedInfo.NotifyType.Information, string errorCode = null)
    {
        return new ReturningError(new UnfinishedInfo(title, notifyType, errorCode));
    }

    public static ReturningError FromLocalization(string titleKey, string mensajeKey, object[] titleFormatArgs = null,object[] mensajeFormatArgs = null, UnfinishedInfo.NotifyType notifyType = ReturningCore.UnfinishedInfo.NotifyType.Information, string errorCode = null)
    {
        return new ReturningError(ReturningCore.UnfinishedInfo.FromLocalization(titleKey, mensajeKey, titleFormatArgs, mensajeFormatArgs, notifyType, errorCode));
    }
    public static ReturningError FromLocalization(string titleKey, object[] titleFormatArgs = null, UnfinishedInfo.NotifyType notifyType = ReturningCore.UnfinishedInfo.NotifyType.Information, string errorCode = null)
    {
        return new ReturningError(ReturningCore.UnfinishedInfo.FromLocalization(titleKey, titleFormatArgs, notifyType, errorCode));
    }

    #endregion

    
    #region ErrorAndUnfinishedInfo

    public static ReturningError ErrorAndUnfinishedInfo(string title, string mensaje, Exception tryException = null,object parameters=null, UnfinishedInfo.NotifyType notifyType = ReturningCore.UnfinishedInfo.NotifyType.Information, string errorCode = null, [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null,
                                                        [ CallerLineNumber ] int lineNumber = 0)
    {
        return new ReturningError(new ErrorInfo(title+" \t " + mensaje,parameters, tryException, errorCode, memberName, filePath, lineNumber),  new UnfinishedInfo(title, mensaje, notifyType, errorCode));
    }
    public static ReturningError ErrorAndUnfinishedInfo(string title, Exception tryException = null,object parameters=null, UnfinishedInfo.NotifyType notifyType = ReturningCore.UnfinishedInfo.NotifyType.Information, string errorCode = null, [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null,
                                                        [ CallerLineNumber ] int lineNumber = 0)
    {
        return new ReturningError(new ErrorInfo(title , tryException, errorCode, memberName, filePath, lineNumber),new UnfinishedInfo(title, notifyType, errorCode));
    }

 
    #endregion
    #endregion
    
    public enum ErrorTypes
    {
        Error,
        Unfinished,
        ErrorAndUnfinished
    }
    
    public ErrorInfo      Error      { get; set; }
    public UnfinishedInfo Unfinished { get; set; }
    public ErrorTypes     ErrorType       => Error != null && Unfinished != null ? ErrorTypes.ErrorAndUnfinished : Error != null ? ErrorTypes.Error : ErrorTypes.Unfinished;
    

}
