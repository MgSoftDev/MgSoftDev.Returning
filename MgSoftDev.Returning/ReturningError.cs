using System;
using System.Runtime.CompilerServices;
using MgSoftDev.ReturningCore.Exceptions;

namespace MgSoftDev.ReturningCore;

public class ReturningError
{
    public ReturningError() { }

    internal ErrorInfo      Error      { get; set; }
    internal UnfinishedInfo Unfinished { get; set; }
    
    #region Fluent Api

    public ReturningError SetUnfinished(string title, string mensaje, UnfinishedInfo.NotifyType notifyType = ReturningCore.UnfinishedInfo.NotifyType.Information, string errorCode = null)
    {
        Unfinished = new UnfinishedInfo(title, mensaje, notifyType, errorCode);

        return this;
    }

    public ReturningError SetUnfinished(string title, UnfinishedInfo.NotifyType notifyType = ReturningCore.UnfinishedInfo.NotifyType.Information, string errorCode = null)
    {
        Unfinished = new UnfinishedInfo(title, notifyType, errorCode);

        return this;
    }


    public ReturningError SetLocalization(string                    titleKey, string mensajeKey, object[] titleFormatArgs = null, object[] mensajeFormatArgs = null,
                                          UnfinishedInfo.NotifyType notifyType = ReturningCore.UnfinishedInfo.NotifyType.Information, string errorCode = null)
    {
        Unfinished = ReturningCore.UnfinishedInfo.FromLocalization(titleKey, mensajeKey, titleFormatArgs, mensajeFormatArgs, notifyType, errorCode);

        return this;
    }

    public ReturningError SetLocalization(string titleKey, object[] titleFormatArgs = null, UnfinishedInfo.NotifyType notifyType = ReturningCore.UnfinishedInfo.NotifyType.Information,
                                          string errorCode = null)
    {
        Unfinished = ReturningCore.UnfinishedInfo.FromLocalization(titleKey, titleFormatArgs, notifyType, errorCode);

        return this;
    }


    public ReturningError SetUnfinished()
    {
        Unfinished = new UnfinishedInfo(Error?.ErrorMessage, null, ReturningCore.UnfinishedInfo.NotifyType.Error, Error?.ErrorCode);

        return this;
    }

    public ReturningError SetError([ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        Error = new ErrorInfo(Unfinished?.Title + "\t" + Unfinished?.Mensaje, null, Unfinished?.ErrorCode, memberName, filePath, lineNumber);

        return this;
    }

    public ReturningError SetError(string errorMessage, Exception tryException = null, string errorCode = "", [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null,
                                   [ CallerLineNumber ] int lineNumber = 0)
    {
        Error = new ErrorInfo(errorMessage, tryException, errorCode, memberName, filePath, lineNumber);

        return this;
    }

    public ReturningError SetError(string                    errorMessage,    object parameters, Exception tryException = null, string errorCode = "", [ CallerMemberName ] string memberName = null,
                                   [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
    {
        Error = new ErrorInfo(errorMessage, parameters, tryException, errorCode, memberName, filePath, lineNumber);

        return this;
    }

    #endregion

    
    public Returning Throw()
    {
        if (Error!= null) throw new ReturningErrorException(this);
        if (Unfinished != null) throw new ReturningUnfinishedException(this);

        return this;
    }
}
