using MgSoftDev.Returning;
using MgSoftDev.Returning.Exceptions;
using MgSoftDev.Returning.Helper;

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
        Returning                    result10 = Returning.Success(15);
        Returning                    result11 = Returning.Success(new List<string>());
        Console.WriteLine("------------------------- From Error \n{0}", result);
        Console.WriteLine("------------------------- From Unfinished \n{0}", result2);
        Console.WriteLine("------------------------- From Success \n{0}", result3);
        Console.WriteLine("------------------------- ErrorInfo cast to Returning \n{0}", result4);
        Console.WriteLine("------------------------- UnfinishedInfo cast to Returning \n{0}", result5);
        Console.WriteLine("------------------------- From ReturningErrorException \n{0}", result8);
        Console.WriteLine("------------------------- From ReturningUnfinishedException \n{0}", result9);
        Console.WriteLine("------------------------- From Returning<T> \n{0}", result10);
        Console.WriteLine("------------------------- From ReturningList<T> \n{0}", result11);

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
        Assert.AreEqual(res2.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res3.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, Returning.TypeResult.Unfinished);
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
        Assert.AreEqual(res2.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res3.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, Returning.TypeResult.Unfinished);
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
        Assert.AreEqual(res3.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, Returning.TypeResult.Unfinished);
    }
    
    [ Test ]
    public void Try4()
    {
        Returning res = Returning.Try(()=>
        {
            Console.WriteLine("Some code...");
            return Returning.Success();
        });
        
        Returning res2 = Returning.Try(()=>
        {
            throw new Exception("Generic Exception");
            return Returning.Success();
        });
        
        Returning res3 = Returning.Try(()=>
        {
            Returning.Error("ReturningErrorException")
                     .Throw();
            return Returning.Success();
        });
        Returning res4 = Returning.Try(()=>
        {
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
            return Returning.Success();
        });
        
        Returning res5 = Returning.Try(()=>
        {
            return Returning.Error("ReturningErrorException");
        });
        Returning res6 = Returning.Try(()=>
        {
            return Returning.Unfinished("ReturningUnfinishedException");
        });
        
        Console.WriteLine(res.ToString());
        Assert.AreEqual(res.Ok, true);
        Assert.AreEqual(res2.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res3.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, Returning.TypeResult.Unfinished);
        Assert.AreEqual(res5.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res6.ResultType, Returning.TypeResult.Unfinished);
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
        Assert.AreEqual(res2.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res3.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, Returning.TypeResult.Unfinished);
        Assert.AreEqual(res5.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res6.ResultType, Returning.TypeResult.Unfinished);
    }
    
    
    [ Test ]
    public async Task Try6()
    {
        Returning res = await Returning.TryTask(()=>
        {
            Console.WriteLine("Some code...");
            return Returning.Success();
        });
        
        Returning res2 = await  Returning.TryTask(()=>
        {
            throw new Exception("Generic Exception");
            return Returning.Success();
        });
        
        Returning res3 = await  Returning.TryTask(()=>
        {
            Returning.Error("ReturningErrorException")
                     .Throw();
            return Returning.Success();
        });
        Returning res4 = await  Returning.TryTask(()=>
        {
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
            return Returning.Success();
        });
        
        Returning res5 =  await Returning.TryTask(()=>Returning.Error("ReturningErrorException"));
        Returning res6 =  await Returning.TryTask(()=>Returning.Unfinished("ReturningUnfinishedException"));
        
        Console.WriteLine(res.ToString());
        Assert.AreEqual(res.Ok, true);
        Assert.AreEqual(res2.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res3.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, Returning.TypeResult.Unfinished);
        Assert.AreEqual(res5.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res6.ResultType, Returning.TypeResult.Unfinished);
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

    [ Test ]
    public void ReturnReturning()
    {
        var opt = 3;
        Returning result = Returning.Try(()=>
        {
             if (opt == 1) return Returning.Error("asas");
            if (opt == 2) return Returning.Unfinished("asas");
            if (opt == 3) return Returning.Success(10);
             if (opt == 4) return Returning.Success();
             if (opt == 5) return Returning.Success(new List<string>());
            
            var task = ReturningTask();
            task.Wait();
            if (opt == 6) return task.Result ;
            
            var task2 = ReturningTask2();
            task2.Wait();
           if (opt == 7) return task2.Result ;
            
             var taskList = ReturningListTask();
             taskList.Wait();
             if (opt == 8) return taskList.Result ;

            return Returning.Success();
        });
    }
    [ Test ]
    public void ReturnReturningGeneric()
    {
        var opt = 3;
        Returning<int> result = Returning<int>.Try(()=>
        {
             if (opt == 1) return Returning.Error("asas");
             if (opt == 2) return Returning.Unfinished("asas");
             if (opt == 3) return Returning.Success(10);
             if (opt == 4) return Returning.Success().ToReturning<int>();
            // if (opt == 5) return 10;

            var task = ReturningTask();
            task.Wait();
            if (opt == 6) return task.Result.ToReturning<int>() ;
            
            var task2 = ReturningTask2();
            task2.Wait();
            if (opt == 7) return task2.Result ;
            
            var taskList = ReturningListTask();
            taskList.Wait();
            if (opt == 8) return taskList.Result.ToReturning<int>() ;

            return Returning.Success(10);
        });
    }
    
    ReturningAsync ReturningTask()
    {
        return Returning.TryTask(()=>
        {
            return Returning.Success();
        });
    }
    ReturningAsync<int> ReturningTask2()
    {
        return Returning<int>.TryTask(()=>
        {
            return Returning.Success(10);
        });
    }
    ReturningListAsync<int> ReturningListTask()
    {
        return ReturningList<int>.TryTask(()=>
        {
            return Returning.Success(new List<int>(){1,2,3});
        });
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
