using System;
using MgSoftDev.Controls.WPF.Notification;
using MgSoftDev.ReturningCore.Helper;

namespace MgSoftDev.ReturningCore.NotifyExtensions;

public static class NotifyExtension
{
    public static Returning<T> SendNotifyIfNotOk<T>(this Returning<T> value, string? messageIfError = null)
    {
        SendNotify(value, messageIfError);
        return value;
    }
    public static Returning SendNotifyIfNotOk(this Returning value, string? messageIfError = null)
    {
        SendNotify(value, messageIfError);
        return value;
    }

    static void SendNotify(Returning value, string? messageIfError)
    {
        try
        {
            messageIfError ??= "Ocurrió un error intente nuevamente";
            if (value.ResultType is ReturningBase.TypeResult.Unfinished or ReturningBase.TypeResult.ErrorAndUnfinished)
            {
                var uf = value.UnfinishedInfo;

                var title =  uf.Title.Format(uf.TitleFormatArgs);

                var message =  uf.Mensaje.Format(uf.MensajeFormatArgs);

                Notify.Show(title, message, uf.Type.ToToastType());
            }
            else if (value.ResultType == ReturningBase.TypeResult.Error)
            {
               Notify.ShowError(messageIfError,null);
            }
        }
        catch (Exception)
        {
            // ignored
        }
    }

    static Notify.NotificationType ToToastType(this UnfinishedInfo.NotifyType type)
    {
        return type switch
               {
                   UnfinishedInfo.NotifyType.Error      =>Notify.NotificationType.Error,
                   UnfinishedInfo.NotifyType.Information=>Notify.NotificationType.Information,
                   UnfinishedInfo.NotifyType.Success    =>Notify.NotificationType.Success,
                   UnfinishedInfo.NotifyType.Warning    =>Notify.NotificationType.Warning,
                   _                                    =>throw new ArgumentOutOfRangeException(nameof(type), type, null)
               };
    }

}
