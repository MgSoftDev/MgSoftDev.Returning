using System;
using System.Runtime.CompilerServices;
using MgSoftDev.ReturningCore.Exceptions;

namespace MgSoftDev.ReturningCore
{
    internal class ErrorInfo2
    {
        public const string UnhandledError = "UnhandledError";
        public enum TypeError
        {
            Error,
            Message,
            ErrorAndMessage
        }
        
        public enum NotifyType
        {
            Information = 1,
            Success     = 2,
            Warning     = 3,
            Error       = 4
        }
        public string     Title             { get; set; }
        public string     Mensaje           { get; set; }
        public bool       UseLocalization   { get; set; }
        public string[]   TitleFormatArgs   { get; set; }
        public string[]   MensajeFormatArgs { get; set; }
        public NotifyType Type              { get; set; }
        
        public string             ErrorMessage      { get; set; }
        public string             ErrorCode         { get; set; }
        public string             ErrorMemberName   { get; set; }
        public string             ErrorFilePath     { get; set; }
        public int                ErrorLineNumber   { get; set; }
        public ErrorInfoException ErrorTryException { get; set; }
        public object             ErrorParameters   { get; set; }

        public ErrorInfo2() { }

        public ErrorInfo2(string errorMessage, Exception tryException = null, string errorCode = "", [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null,
                         [ CallerLineNumber ] int lineNumber = 0)
        {
            Title             = errorMessage;
            Mensaje           = null;
            UseLocalization   = false;
            TitleFormatArgs   = null;
            MensajeFormatArgs = null;
            Type              = NotifyType.Error;
            ErrorMessage      = errorMessage;
            ErrorCode         = errorCode;
            ErrorMemberName   = memberName;
            ErrorFilePath     = filePath;
            ErrorLineNumber   = lineNumber;
            ErrorTryException = new ErrorInfoException(tryException);
            ErrorParameters   = null;
        }

        public ErrorInfo2(string                    errorMessage,    object parameters, Exception tryException = null, string errorCode = "", [ CallerMemberName ] string memberName = null,
                         [ CallerFilePath ] string filePath = null, [ CallerLineNumber ] int lineNumber = 0)
        {
            Title             = errorMessage;
            Mensaje           = null;
            UseLocalization   = false;
            TitleFormatArgs   = null;
            MensajeFormatArgs = null;
            Type              = NotifyType.Error;
            ErrorMessage      = errorMessage;
            ErrorCode         = errorCode;
            ErrorMemberName   = memberName;
            ErrorFilePath     = filePath;
            ErrorLineNumber   = lineNumber;
            ErrorTryException = new ErrorInfoException(tryException);
            ErrorParameters   = parameters;
        }
        
        public static ErrorInfo2 UiMessage(string title, string mensaje, NotifyType notifyType = NotifyType.Information, string errorCode = null, [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null,
                                           [ CallerLineNumber ] int lineNumber = 0)
        {
            return new ErrorInfo2()
            {
                Title           = title,
                Mensaje         = mensaje,
                UseLocalization = false,
                TitleFormatArgs   = null,
                MensajeFormatArgs = null,
                Type              = notifyType,
                ErrorMessage = title + "\t" +mensaje,
                ErrorCode    = errorCode,
                ErrorMemberName   = memberName,
                ErrorFilePath     = filePath,
                ErrorLineNumber   = lineNumber,
                ErrorTryException = null,
                ErrorParameters   = null
            };
        }
        public static ErrorInfo2 UiMessage(string title, NotifyType notifyType = NotifyType.Information, string errorCode = null, [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null,
                                           [ CallerLineNumber ] int lineNumber = 0)
        {
            return new ErrorInfo2()
            {
                Title             = title,
                Mensaje           = null,
                UseLocalization   = false,
                TitleFormatArgs   = null,
                MensajeFormatArgs = null,
                Type              = notifyType,
                ErrorMessage      = title ,
                ErrorCode         = errorCode,
                ErrorMemberName   = memberName,
                ErrorFilePath     = filePath,
                ErrorLineNumber   = lineNumber,
                ErrorTryException = null,
                ErrorParameters   = null
            };
        }
        
        
        
        public static ErrorInfo2 UiMessageLocalization(string titleKey, string mensajeKey, string[] titleFormatArgs = null,string[] mensajeFormatArgs = null, NotifyType notifyType = NotifyType.Information, string errorCode = null, [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null,
                                                       [ CallerLineNumber ] int lineNumber = 0)
        {
            return new ErrorInfo2()
            {
                Title             = titleKey,
                Mensaje           = mensajeKey,
                UseLocalization   = true,
                TitleFormatArgs   = titleFormatArgs,
                MensajeFormatArgs = mensajeFormatArgs,
                Type              = notifyType,
                ErrorMessage      = titleKey + "\t" +mensajeKey,
                ErrorCode         = errorCode,
                ErrorMemberName   = memberName,
                ErrorFilePath     = filePath,
                ErrorLineNumber   = lineNumber,
                ErrorTryException = null,
                ErrorParameters   = null
            };
        }
        public static ErrorInfo2 UiMessageLocalization(string titleKey,  string[] titleFormatArgs = null, NotifyType notifyType = NotifyType.Information, string errorCode = null, [ CallerMemberName ] string memberName = null, [ CallerFilePath ] string filePath = null,
                                                       [ CallerLineNumber ] int lineNumber = 0)
        {
            return new ErrorInfo2()
            {
                Title             = titleKey,
                Mensaje           = null,
                UseLocalization   = true,
                TitleFormatArgs   = titleFormatArgs,
                MensajeFormatArgs = null,
                Type              = notifyType,
                ErrorMessage      = titleKey ,
                ErrorCode         = errorCode,
                ErrorMemberName   = memberName,
                ErrorFilePath     = filePath,
                ErrorLineNumber   = lineNumber,
                ErrorTryException = null,
                ErrorParameters   = null
            };
        }
        
        
        
      //  public void Throw() { throw new ReturningErrorException(this); }


        #region Overrides of Object

        public override string ToString() { return $"ErrorMessage:{ErrorMessage}\n"         +
                                                   $"TryException:{ErrorTryException}\n"    +
                                                   $"ErrorCode:{ErrorCode}\n"               +
                                                   $"Parameters{ErrorParameters != null}\n" +
                                                   $"MemberName:{ErrorMemberName}\n"        +
                                                   $"LineNumber:{ErrorLineNumber}\n"        + 
                                                   $"FilePath{ErrorFilePath}"; 
        }

        #endregion
    }

}
