using System;

namespace MgSoftDev.Returning.Interfaces;

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
    bool           IsLogStored    { get; }
    Exception      LogException   { get; set; }
}

public interface IReturning< out T > : IReturning
{
    T Value { get; }
}
