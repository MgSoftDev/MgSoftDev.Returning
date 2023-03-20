using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MgSoftDev.Returning.Exceptions;
using MgSoftDev.Returning.Helper;
using MgSoftDev.Returning.Interfaces;

namespace MgSoftDev.Returning;


public class ReturningList<T> : IReturning<List<T>>
{
    public ReturningList() { }
    
    #region Property

    public List<T>                     Value          { get; internal set; }
    public UnfinishedInfo        UnfinishedInfo { get; internal set; }
    public ErrorInfo             ErrorInfo      { get; internal set; }
    public bool                  IsLogStored    { get; set; }
    public Exception             LogException   { get; set; }
    public bool                  Ok             =>ErrorInfo == null && UnfinishedInfo == null;
    public IReturning.TypeResult ResultType     =>Ok ? IReturning.TypeResult.Success : ( ErrorInfo != null ? IReturning.TypeResult.Error : IReturning.TypeResult.Unfinished );
    public bool                  OkNotNull      =>Ok && Value != null;
    #endregion

    #region Methods

    public ReturningList<T> Throw()
    {
        if (ResultType == IReturning.TypeResult.Error) throw new ReturningErrorException(this);
        if (ResultType == IReturning.TypeResult.Unfinished) throw new ReturningUnfinishedException(this);

        return this;
    }

    

    #endregion

    #region Operators Overloading

    public static implicit operator List<T>(ReturningList<T> value)=>value.Value;

    public static implicit operator ReturningList<T>(Returning value)=>
        new()
        {
            ErrorInfo      = value.ErrorInfo,
            UnfinishedInfo = value.UnfinishedInfo
        };

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

    public void Dispose()
    {
        ErrorInfo      = null;
        UnfinishedInfo = null;
        LogException   = null;
        GC.Collect();
    }

    #endregion
}