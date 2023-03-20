using System;
using System.Threading;
using System.Threading.Tasks;

namespace MgSoftDev.Returning;

public class ReturningAsync : Task<Returning>
{
    public ReturningAsync(Func<Returning> function): base(function)
    {
        Start();
    }
    public ReturningAsync(Func<Returning> function, CancellationToken cancellationToken) : base(function, cancellationToken)
    {
        Start();
    }
    public ReturningAsync(Func<Returning> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(function, cancellationToken,creationOptions)
    {
        Start();
    }
    public ReturningAsync(Func<Returning> function, TaskCreationOptions creationOptions) : base(function, creationOptions)
    {
        Start();
    }
}

public class ReturningAsync<T> : Task<Returning<T>>
{
    public ReturningAsync(Func<Returning<T>> function): base(function)
    {
        Start();
    }
    public ReturningAsync(Func<Returning<T>> function, CancellationToken cancellationToken) : base(function, cancellationToken)
    {
        Start();
    }
    public ReturningAsync(Func<Returning<T>> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(function, cancellationToken,creationOptions)
    {
        Start();
    }
    public ReturningAsync(Func<Returning<T>> function, TaskCreationOptions creationOptions) : base(function, creationOptions)
    {
        Start();
    }
}