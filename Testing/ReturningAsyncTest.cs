using MgSoftDev.ReturningCore;
using MgSoftDev.ReturningCore.Exceptions;
using MgSoftDev.ReturningCore.Helper;

namespace Testing;

public class ReturningAsyncTest
{
    [ SetUp ]
    public void Setup() { }

    [ Test ]
    public async Task ReturnsCallsTest()
    {
        var res = new ReturningAsync(()=>Returning.Try(()=>
        {
            Thread.Sleep(5000);
            return Returning.Success();
        }));
        
        await res;
        
        Assert.IsTrue(res.Result.Ok);
    }

}
