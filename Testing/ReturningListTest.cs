using MgSoftDev.Returning;
using MgSoftDev.Returning.Exceptions;
using MgSoftDev.Returning.Helper;
using MgSoftDev.Returning.Interfaces;

namespace Testing;

public class ReturningListTest
{
    [ SetUp ]
    public void Setup() { }

    [ Test ]
    public void ReturnsCallsTest()
    {
        ReturningList<int>           result  = Returning.Error("Ocurrio un Error");
        ReturningList<int>           result2 = Returning.Unfinished("Valide los datos");
        ReturningList<int>           result3 = Returning.Success(new List<int>(){1,2,3});
        ReturningList<int>           result4 = new ErrorInfo("Error 2");
        ReturningList<int>           result5 = new UnfinishedInfo("Unfinished");
        ReturningErrorException      result8 = new ReturningErrorException(( Returning ) new ErrorInfo("Error 2"));
        ReturningUnfinishedException result9 = new ReturningUnfinishedException(( Returning ) new UnfinishedInfo("Unfinished"));
        Console.WriteLine("------------------------- From Error \n{0}", result);
        Console.WriteLine("------------------------- From Unfinished \n{0}", result2);
        Console.WriteLine("------------------------- From Success \n{0}", result3);
        Console.WriteLine("------------------------- ErrorInfo cast to IReturning \n{0}", result4);
        Console.WriteLine("------------------------- UnfinishedInfo cast to IReturning \n{0}", result5);
        Console.WriteLine("------------------------- From ReturningErrorException \n{0}", result8);
        Console.WriteLine("------------------------- From ReturningUnfinishedException \n{0}", result9);

        Assert.Pass();
    }

    [ Test ]
    public void Throw()
    {
       var res = Assert.Throws<ReturningErrorException>(()=>Returning.Error("Error de ejemplo")
                                                            .Throw(), "ReturningUnfinishedException");
       var res2=  Assert.Throws<ReturningUnfinishedException>(()=>Returning.Unfinished("Unfinished de ejemplo")
                                                                 .Throw(), "ReturningUnfinishedException");
       
       Console.WriteLine("------------------------- From Throw Error \n{0}", res.Error);
       Console.WriteLine("------------------------- From Throw Unfinished \n{0}", res2.Error);
       
        Assert.Pass();
    }

    
    [ Test ]
    public async Task SaveLog()
    {
        var res = Returning.Success(new List<int>(){1,2,3})
                           .SaveLog();
        var res2 = await Returning.Success(new List<int>(){1,2,3})
                            .SaveLogAsync();
        
        Assert.AreEqual(res.Value,new List<int>(){1,2,3});
        Assert.AreEqual(res2.Value,new List<int>(){1,2,3});
        
        
       Console.WriteLine("------------------------- Res1 \n{0}", res);
       Console.WriteLine("------------------------- Res2 \n{0}", res2);
       
        Assert.Pass();
    }

    

    
    
    [ Test ]
    public void Try1()
    {
        ReturningList<int> res = Returning.Try(()=>
        {
            Console.WriteLine("Some code...");
            return new List<int>(){1,2,3};
        });
        
        var res2 = Returning.Try(()=>
        {
            throw new Exception("Generic Exception");
            return new List<int>(){1,2,3};
        });
        
        var res3 = Returning.Try(()=>
        {
            Returning.Error("ReturningErrorException")
                     .Throw();
            return new List<int>(){1,2,3};
        });
        var res4 = Returning.Try(()=>
        {
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
            return new List<int>(){1,2,3};
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
            return new List<int>(){1,2,3};
        });
        
        Console.WriteLine("End Call");
        
        var res2 = await Returning.TryTask(async ()=>
        {
            await Task.Delay(1000);
            throw new Exception("Generic Exception");
            return new List<int>(){1,2,3};
        });
        
        var res3 = await Returning.TryTask(async ()=>
        {
            await Task.Delay(1000);
            Returning.Error("ReturningErrorException")
                     .Throw();
            return new List<int>(){1,2,3};
        });
        var res4 = await Returning.TryTask(async ()=>
        {
            await Task.Delay(1000);
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
            return new List<int>(){1,2,3};
            
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
        Console.WriteLine("Start Call");
        var res = await Returning.TryTask( ()=>
        {
            return new List<int>(){1,2,3};
        });
        
        Console.WriteLine("End Call");
        
        var res2 = await Returning.TryTask( ()=>
        {
            throw new Exception("Generic Exception");
            return new List<int>(){1,2,3};
        });
        
        var res3 = await Returning.TryTask( ()=>
        {
            Returning.Error("ReturningErrorException")
                     .Throw();
            return new List<int>(){1,2,3};
        });
        var res4 = await Returning.TryTask( ()=>
        {
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
            return new List<int>(){1,2,3};
            
        });
        
        Console.WriteLine(res4.ToString());
        Assert.AreEqual(res.Ok, true);
        Assert.AreEqual(res2.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res3.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, IReturning.TypeResult.Unfinished);
    }
    
    [ Test ]
    public void Try4()
    {
        ReturningList<int> res = Returning.Try(()=>
        {
            Console.WriteLine("Some code...");
            return Returning.Success(new List<int>(){1,2,3});
        });
        
        var res2 = Returning.Try(()=>
        {
            throw new Exception("Generic Exception");
            return Returning.Success(new List<int>(){1,2,3});
        });
        
        var res3 = Returning.Try(()=>
        {
            Returning.Error("ReturningErrorException")
                     .Throw();
            return Returning.Success(new List<int>(){1,2,3});
        });
        var res4 = Returning.Try(()=>
        {
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
            return Returning.Success(new List<int>(){1,2,3});
        });
        
        Console.WriteLine(res.ToString());
        Assert.AreEqual(res.Ok, true);
        Assert.AreEqual(res2.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res3.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, IReturning.TypeResult.Unfinished);
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
            return Returning.Success(new List<int>(){1,2,3});
        });
        
        Console.WriteLine("End Call");
        
        var res2 = await Returning.TryTask(async ()=>
        {
            await Task.Delay(1000);
            throw new Exception("Generic Exception");
            return Returning.Success(new List<int>(){1,2,3});
        });
        
        var res3 = await Returning.TryTask(async ()=>
        {
            await Task.Delay(1000);
            Returning.Error("ReturningErrorException")
                     .Throw();
            return Returning.Success(new List<int>(){1,2,3});
        });
        var res4 = await Returning.TryTask(async ()=>
        {
            await Task.Delay(1000);
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
            return Returning.Success(new List<int>(){1,2,3});
            
        });
        
    
        Console.WriteLine(res4.ToString());
        Assert.AreEqual(res.Ok, true);
        Assert.AreEqual(res2.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res3.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, IReturning.TypeResult.Unfinished);
    }
    
    
    [ Test ]
    public async Task Try6()
    {
        var res = await Returning.TryTask(()=>
        {
            Console.WriteLine("Some code...");
            return Returning.Success(new List<int>(){1,2,3});
        });
        
        var res2 = await  Returning.TryTask(()=>
        {
            throw new Exception("Generic Exception");
            return Returning.Success(new List<int>(){1,2,3});
        });
        
        var res3 = await  Returning.TryTask(()=>
        {
            Returning.Error("ReturningErrorException")
                     .Throw();
            return Returning.Success(new List<int>(){1,2,3});
        });
        var res4 = await  Returning.TryTask(()=>
        {
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
            return Returning.Success(new List<int>(){1,2,3});
        });
        
       
        Console.WriteLine(res.ToString());
        Assert.AreEqual(res.Ok, true);
        Assert.AreEqual(res2.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res3.ResultType, IReturning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, IReturning.TypeResult.Unfinished);
    }
}
