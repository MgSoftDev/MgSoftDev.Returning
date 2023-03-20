using MgSoftDev.Returning;
using MgSoftDev.Returning.Exceptions;
using MgSoftDev.Returning.Interfaces;

namespace Testing;

public class ReturningTest
{
    [ SetUp ]
    public void Setup() { }

    [ Test ]
    public void ReturnsCallsTest()
    {
        Returning                    result  = Returning.Error("Ocurrio un Error");
        Returning                    result2 = Returning.Unfinished("Valide los datos");
        Returning                    result3 = Returning.Success();
        Returning                    result4 = new ErrorInfo("Error 2");
        Returning                    result5 = new UnfinishedInfo("Unfinished");
        ReturningErrorException      result8 = new ReturningErrorException(( Returning ) new ErrorInfo("Error 2"));
        ReturningUnfinishedException result9 = new ReturningUnfinishedException(( Returning ) new UnfinishedInfo("Unfinished"));
        Console.WriteLine("------------------------- From Error \n{0}", result);
        Console.WriteLine("------------------------- From Unfinished \n{0}", result2);
        Console.WriteLine("------------------------- From Success \n{0}", result3);
        Console.WriteLine("------------------------- ErrorInfo cast to Returning \n{0}", result4);
        Console.WriteLine("------------------------- UnfinishedInfo cast to Returning \n{0}", result5);
        Console.WriteLine("------------------------- From ReturningErrorException \n{0}", result8);
        Console.WriteLine("------------------------- From ReturningUnfinishedException \n{0}", result9);

        Assert.Pass();
    }

    [ Test ]
    public void Throw()
    {
        Assert.Throws<ReturningErrorException>(()=>Returning.Error("Error de ejemplo")
                                                            .Throw(), "ReturningUnfinishedException");
        Assert.Throws<ReturningUnfinishedException>(()=>Returning.Unfinished("Unfinished de ejemplo")
                                                                 .Throw(), "ReturningUnfinishedException");
        Assert.Pass();
    }

    [ Test ]
    public void Try1()
    {
        var res = Returning.Try(()=>
        {
            Console.WriteLine("Some code...");
        });
        
        var res2 = Returning.Try(()=>
        {
            throw new Exception("Generic Exception");
        });
        
        var res3 = Returning.Try(()=>
        {
            Returning.Error("ReturningErrorException")
                     .Throw();
        });
        var res4 = Returning.Try(()=>
        {
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
        });
        
        Console.WriteLine(res.ToString());
        Assert.AreEqual(res.Ok, true);
        Assert.AreEqual(res2.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res3.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, IReturning.TypeResult.Unfinished);
    }
    
    
    [ Test ]
    public async Task Try2()
    {
        Console.WriteLine("Start Call");
        var res = await Returning.TryTask( async()=>
        {
            Console.WriteLine("Start Some code...");
            await Task.Delay(2000);
            Console.WriteLine("End Some code...");
        });
        
        Console.WriteLine("End Call");
        
        var res2 = await Returning.TryTask(async ()=>
        {
            await Task.Delay(1000);
            throw new Exception("Generic Exception");
        });
        
        var res3 = await Returning.TryTask(async ()=>
        {
            await Task.Delay(1000);
            Returning.Error("ReturningErrorException")
                     .Throw();
        });
        var res4 = await Returning.TryTask(async ()=>
        {
            await Task.Delay(1000);
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
            
        });
        
        Console.WriteLine(res4.ToString());
        Assert.AreEqual(res.Ok, true);
        Assert.AreEqual(res2.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res3.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, IReturning.TypeResult.Unfinished);
    }
    
    [ Test ]
    public async Task Try3()
    {
        var res = await Returning.TryTask(()=>
        {
            Console.WriteLine("Some code...");
        });
        
        
        var res3 =await  Returning.TryTask(()=>
        {
            Returning.Error("ReturningErrorException")
                     .Throw();
        });
        var res4 =await  Returning.TryTask(()=>
        {
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
        });
        
        Console.WriteLine(res.ToString());
        Assert.AreEqual(res.Ok, true);
        Assert.AreEqual(res3.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, IReturning.TypeResult.Unfinished);
    }
    
    [ Test ]
    public void Try4()
    {
        var res = Returning.Try(()=>
        {
            Console.WriteLine("Some code...");
            return Returning.Success();
        });
        
        var res2 = Returning.Try(()=>
        {
            throw new Exception("Generic Exception");
            return Returning.Success();
        });
        
        var res3 = Returning.Try(()=>
        {
            Returning.Error("ReturningErrorException")
                     .Throw();
            return Returning.Success();
        });
        var res4 = Returning.Try(()=>
        {
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
            return Returning.Success();
        });
        
        var res5 = Returning.Try(()=>
        {
            return Returning.Error("ReturningErrorException");
        });
        var res6 = Returning.Try(()=>
        {
            return Returning.Unfinished("ReturningUnfinishedException");
        });
        
        Console.WriteLine(res.ToString());
        Assert.AreEqual(res.Ok, true);
        Assert.AreEqual(res2.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res3.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, IReturning.TypeResult.Unfinished);
        Assert.AreEqual(res5.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res6.ResultType, IReturning.TypeResult.Unfinished);
    }
    
    
    
    [ Test ]
    public async Task Try5()
    {
        Console.WriteLine("Start Call");
        var res = await Returning.TryTask( async()=>
        {
            Console.WriteLine("Start Some code...");
            await Task.Delay(2000);
            Console.WriteLine("End Some code...");
            return Returning.Success();
        });
        
        Console.WriteLine("End Call");
        
        var res2 = await Returning.TryTask(async ()=>
        {
            await Task.Delay(1000);
            throw new Exception("Generic Exception");
            return Returning.Success();
        });
        
        var res3 = await Returning.TryTask(async ()=>
        {
            await Task.Delay(1000);
            Returning.Error("ReturningErrorException")
                     .Throw();
            return Returning.Success();
        });
        var res4 = await Returning.TryTask(async ()=>
        {
            await Task.Delay(1000);
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
            return Returning.Success();
            
        });
        
        var res5 = await Returning.TryTask(async ()=>
        {
            await Task.Delay(1000);
            return Returning.Error("ReturningErrorException");
        });
        var res6 = await Returning.TryTask(async ()=>
        {
            await Task.Delay(1000);

            return Returning.Unfinished("ReturningUnfinishedException");

        });
        Console.WriteLine(res4.ToString());
        Assert.AreEqual(res.Ok, true);
        Assert.AreEqual(res2.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res3.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, IReturning.TypeResult.Unfinished);
        Assert.AreEqual(res5.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res6.ResultType, IReturning.TypeResult.Unfinished);
    }
    
    
    [ Test ]
    public async Task Try6()
    {
        var res = await Returning.TryTask(()=>
        {
            Console.WriteLine("Some code...");
            return Returning.Success();
        });
        
        var res2 = await  Returning.TryTask(()=>
        {
            throw new Exception("Generic Exception");
            return Returning.Success();
        });
        
        var res3 = await  Returning.TryTask(()=>
        {
            Returning.Error("ReturningErrorException")
                     .Throw();
            return Returning.Success();
        });
        var res4 = await  Returning.TryTask(()=>
        {
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
            return Returning.Success();
        });
        
        var res5 =  await Returning.TryTask(()=>Returning.Error("ReturningErrorException"));
        var res6 =  await Returning.TryTask(()=>Returning.Unfinished("ReturningUnfinishedException"));
        
        Console.WriteLine(res.ToString());
        Assert.AreEqual(res.Ok, true);
        Assert.AreEqual(res2.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res3.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, IReturning.TypeResult.Unfinished);
        Assert.AreEqual(res5.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res6.ResultType, IReturning.TypeResult.Unfinished);
    }

    [ Test ]
    public void GetDataFromMethod()
    {
        var res = GetData();
        
        Assert.AreEqual(res.Ok, true);
    }
    
    [ Test ]
    public async Task GetDataFromMethodAsync()
    {
        var res = await GetDataAsync();
        
        Assert.AreEqual(res.Ok, true);
    }

    private Returning GetData()
    {
        return Returning.Try(()=>
        {
            return Returning.Success();
        });
    }
    private ReturningAsync GetDataAsync()
    {
        return Returning.TryTask(()=>
        {
            return Returning.Success();
        });
    }

}
