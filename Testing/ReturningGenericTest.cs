using MgSoftDev.ReturningCore;
using MgSoftDev.ReturningCore.Exceptions;
using MgSoftDev.ReturningCore.Helper;

namespace Testing;

public class ReturningGenericTest
{
    [ SetUp ]
    public void Setup() { }

    [ Test ]
    public void ReturnsCallsTest()
    {
        Returning<string>            result   = Returning.Error("Ocurrio un Error");
        Returning<int>               result2  = Returning.Unfinished("Valide los datos");
        Returning<int>               result3  = Returning.Success(10);
        Returning<int>               result10 = Returning.Success().ToReturning<int>();
        Returning<int>               result11 = Returning.Success(new List<string>()).ToReturning<int>();
        Returning<int>               result4  = new ErrorInfo("Error 2");
        Returning<int>               result5  = new UnfinishedInfo("Unfinished");
        ReturningErrorException      result8  = new ReturningErrorException(( Returning ) new ErrorInfo("Error 2"));
        ReturningUnfinishedException result9  = new ReturningUnfinishedException(( Returning ) new UnfinishedInfo("Unfinished"));
        Console.WriteLine("------------------------- From Error \n{0}", result);
        Console.WriteLine("------------------------- From Unfinished \n{0}", result2);
        Console.WriteLine("------------------------- From Success \n{0}", result3);
        Console.WriteLine("------------------------- ErrorInfo cast to IReturning \n{0}", result4);
        Console.WriteLine("------------------------- UnfinishedInfo cast to IReturning \n{0}", result5);
        Console.WriteLine("------------------------- From ReturningErrorException \n{0}", result8);
        Console.WriteLine("------------------------- From ReturningUnfinishedException \n{0}", result9);
        Console.WriteLine("------------------------- From Returning<T> \n{0}", result10);
        Console.WriteLine("------------------------- From ReturningList<T> \n{0}", result11);

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
        var res = Returning.Success(10)
                           .SaveLog();
        var res2 = await Returning.Success(10)
                            .SaveLogAsync();
        
        Assert.AreEqual(res.Value,10);
        Assert.AreEqual(res2.Value,10);
        
        
       Console.WriteLine("------------------------- Res1 \n{0}", res);
       Console.WriteLine("------------------------- Res2 \n{0}", res2);
       
        Assert.Pass();
    }

    public Returning TryGetReturningOnly()
    {
        return Returning.Try(()=>
        {
            if (DateTime.Now.Day == 1) return Returning.Error("asd");

            var res = GetReturning();

            if (!res.Ok) return res;

            var str1 = GetReturningInt();

            if (!str1.Ok) return str1;

            var strList1 = GetReturningLISTInt();

            if (!strList1.Ok) return strList1;

            var str = GetReturningString();

            if (!str.Ok) return str.ToReturning<int>();

            var strList = GetReturningLISTString();

            if (!strList.Ok) return strList.ToReturning<int>();

            return Returning.Success();
        });
    } 
    public Returning<int> TryGetReturningGeneric()
    {
        return Returning<int>.Try(()=>
        {
            if (DateTime.Now.Day == 1) return Returning.Error("asd");
            if (DateTime.Now.Day == 2) return Returning.Success(2);

            var res = GetReturning();

            if (!res.Ok) return res;

            var str1 = GetReturningInt();

            if (!str1.Ok) return str1;

            var strList1 = GetReturningLISTInt();

            if (!strList1.Ok) return strList1;

            var str = GetReturningString();

            if (!str.Ok) return str.ToReturning<int>();

            var strList = GetReturningLISTString();

            if (!strList.Ok) return strList.ToReturning<int>();

            
            return 5;
        });
    } 
    public ReturningList<int> TryGetReturningList()
    {
        return ReturningList<int>.Try(()=>
        {
            if (DateTime.Now.Day == 1) return Returning.Error("asd");

            var res = GetReturning();

            if (!res.Ok) return res;

            var str1 = GetReturningInt();

            if (!str1.Ok) return str1;

            var strList1 = GetReturningLISTInt();

            if (!strList1.Ok) return strList1;

            var str = GetReturningString();

            if (!str.Ok) return str.ToReturning<int>();

            var strList = GetReturningLISTString();

            if (!strList.Ok) return strList.ToReturning<int>();

            return new List<int>();
        });
    } 
    
     public Task<Returning> TaskTryGetReturningOnly()
    {
        return Returning.TryTask(()=>
        {
            if (DateTime.Now.Day == 1) return Returning.Error("asd");

            var res = GetReturning();

            if (!res.Ok) return res;

            var str1 = GetReturningInt();

            if (!str1.Ok) return str1;

            var strList1 = GetReturningLISTInt();

            if (!strList1.Ok) return strList1;

            var str = GetReturningString();

            if (!str.Ok) return str.ToReturning<int>();

            var strList = GetReturningLISTString();

            if (!strList.Ok) return strList.ToReturning<int>();

            return Returning.Success();
        });
    } 
    public Task<Returning<int>> TaskTryGetReturningGeneric()
    {
        return Returning<int>.TryTask(()=>
        {
            if (DateTime.Now.Day == 1) return Returning.Error("asd");
            if (DateTime.Now.Day == 2) return Returning.Success(2);

            var res = GetReturning();

            if (!res.Ok) return res;

            var str1 = GetReturningInt();

            if (!str1.Ok) return str1;

            var strList1 = GetReturningLISTInt();

            if (!strList1.Ok) return strList1;

            var str = GetReturningString();

            if (!str.Ok) return str.ToReturning<int>();

            var strList = GetReturningLISTString();

            if (!strList.Ok) return strList.ToReturning<int>();

            
            return 5;
        });
    } 
    public Task<ReturningList<int>> TaskTryGetReturningList()
    {
        return ReturningList<int>.TryTask(()=>
        {
            if (DateTime.Now.Day == 1) return Returning.Error("asd");

            var res = GetReturning();

            if (!res.Ok) return res;

            var str1 = GetReturningInt();

            if (!str1.Ok) return str1;

            var strList1 = GetReturningLISTInt();

            if (!strList1.Ok) return strList1;

            var str = GetReturningString();

            if (!str.Ok) return str.ToReturning<int>();

            var strList = GetReturningLISTString();

            if (!strList.Ok) return strList.ToReturning<int>();

            return new List<int>();
        });
    } 
    
    public Returning GetReturningOnly()
    {
        if (DateTime.Now.Day == 1) return Returning.Error("asd");
    
        var res = GetReturning();
        if (!res.Ok) return res;

        var str1 = GetReturningInt();
        if (!str1.Ok) return str1;
        
        var strList1 = GetReturningLISTInt();
        if (!strList1.Ok) return strList1;
        
        var str = GetReturningString();
        if (!str.Ok) return str.ToReturning<int>();
        
        var strList = GetReturningLISTString();
        if (!strList.Ok) return strList.ToReturning<int>();
        
        return Returning.Success();
    } 
    public Returning<int> GetReturningGeneric()
    {
        if (DateTime.Now.Day == 1) return Returning.Error("asd");
    
        var res = GetReturning();
        if (!res.Ok) return res;

        var str1 = GetReturningInt();
        if (!str1.Ok) return str1;
        
        var strList1 = GetReturningLISTInt();
        if (!strList1.Ok) return strList1;
        
        var str = GetReturningString();
        if (!str.Ok) return str.ToReturning<int>();
        
        var strList = GetReturningLISTString();
        if (!strList.Ok) return strList.ToReturning<int>();
        
        return 5;
    } 
    public ReturningList<int> GetReturningList()
    {
        if (DateTime.Now.Day == 1) return Returning.Error("asd");
    
        var res = GetReturning();
        if (!res.Ok) return res;

        var str1 = GetReturningInt();
        if (!str1.Ok) return str1;
        
        var strList1 = GetReturningLISTInt();
        if (!strList1.Ok) return strList1;
        
        var str = GetReturningString();
        if (!str.Ok) return str.ToReturning<int>();
        
        var strList = GetReturningLISTString();
        if (!strList.Ok) return strList.ToReturning<int>();
        
        return new List<int>();
    } 
    public Returning GetReturning()
    {
        
        return Returning.Success();
        
    }

    public Returning<string> GetReturningString()
    {
        
        return Returning.Success("asd");
        
    }
    
    public ReturningList<string> GetReturningLISTString()
    {
        
        return Returning.Success(new List<string>());
        
    }
    
    
    public Returning<int> GetReturningInt()
    {
        
        return Returning.Success(1);
        
    }
    
    public ReturningList<int> GetReturningLISTInt()
    {
        
        return Returning.Success(new List<int>());
        
    }

    
    
    [ Test ]
    public void Try1()
    {
        Returning<int> res = Returning<int>.Try(()=>
        {
            Console.WriteLine("Some code...");
            return 10;
        });
        
        Returning<int> res2 = Returning<int>.Try(()=>
        {
            throw new Exception("Generic Exception");
            return 10;
        });
        
        Returning<int> res3 = Returning<int>.Try(()=>
        {
            Returning.Error("ReturningErrorException")
                     .Throw();
            return 10;
        });
        Returning<int> res4 = Returning<int>.Try(()=>
        {
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
            return 10;
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
        var res = await Returning<int>.TryTask( async()=>
        {
            Console.WriteLine("Start Some code...");
            await Task.Delay(2000);
            Console.WriteLine("End Some code...");
            return 10;
        });
        
        Console.WriteLine("End Call");
        
        Returning<int> res2 = await Returning<int>.TryTask(async ()=>
        {
            await Task.Delay(1000);
            throw new Exception("Generic Exception");
            return 10;
        });
        
        Returning<int> res3 = await Returning<int>.TryTask(async ()=>
        {
            await Task.Delay(1000);
            Returning.Error("ReturningErrorException")
                     .Throw();
            return 10;
        });
        Returning<int> res4 = await Returning<int>.TryTask(async ()=>
        {
            await Task.Delay(1000);
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
            return 10;
            
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
        Console.WriteLine("Start Call");
        Returning<int> res = await Returning<int>.TryTask( ()=>
        {
            return 10;
        });
        
        Console.WriteLine("End Call");
        
        Returning<int> res2 = await Returning<int>.TryTask( ()=>
        {
            throw new Exception("Generic Exception");
            return 10;
        });
        
        Returning<int> res3 = await Returning<int>.TryTask( ()=>
        {
            Returning.Error("ReturningErrorException")
                     .Throw();
            return 10;
        });
        Returning<int> res4 = await Returning<int>.TryTask( ()=>
        {
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
            return 10;
            
        });
        
        Console.WriteLine(res4.ToString());
        Assert.AreEqual(res.Ok, true);
        Assert.AreEqual(res2.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res3.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, Returning.TypeResult.Unfinished);
    }
    
    [ Test ]
    public void Try4()
    {
        Returning<int> res = Returning<int>.Try(()=>
        {
            Console.WriteLine("Some code...");
            return Returning.Success(10);
        });
        
        Returning<int> res2 = Returning<int>.Try(()=>
        {
            throw new Exception("Generic Exception");
            return Returning.Success(10);
        });
        
        Returning<int> res3 = Returning<int>.Try(()=>
        {
            Returning.Error("ReturningErrorException")
                     .Throw();
            return Returning.Success(10);
        });
        Returning<int> res4 = Returning<int>.Try(()=>
        {
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
            return Returning.Success(10);
        });
        
        Console.WriteLine(res.ToString());
        Assert.AreEqual(res.Ok, true);
        Assert.AreEqual(res2.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res3.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, Returning.TypeResult.Unfinished);
    }

    
    [ Test ]
    public async Task Try5()
    {
        Console.WriteLine("Start Call");
        Returning<int> res = await Returning<int>.TryTask( async()=>
        {
            Console.WriteLine("Start Some code...");
            await Task.Delay(2000);
            Console.WriteLine("End Some code...");
            return Returning.Success(10);
        });
        
        Console.WriteLine("End Call");
        
        Returning<int> res2 = await Returning<int>.TryTask(async ()=>
        {
            await Task.Delay(1000);
            throw new Exception("Generic Exception");
            return Returning.Success(10);
        });
        
        Returning<int> res3 = await Returning<int>.TryTask(async ()=>
        {
            await Task.Delay(1000);
            Returning.Error("ReturningErrorException")
                     .Throw();
            return Returning.Success(10);
        });
        Returning<int> res4 = await Returning<int>.TryTask(async ()=>
        {
            await Task.Delay(1000);
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
            return Returning.Success(10);
            
        });
        
    
        Console.WriteLine(res4.ToString());
        Assert.AreEqual(res.Ok, true);
        Assert.AreEqual(res2.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res3.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, Returning.TypeResult.Unfinished);
    }
    
    
    [ Test ]
    public async Task Try6()
    {
        Returning<int> res = await Returning<int>.TryTask(()=>
        {
            Console.WriteLine("Some code...");
            return Returning.Success(10);
        });
        
        Returning<int> res2 = await  Returning<int>.TryTask(()=>
        {
            throw new Exception("Generic Exception");
            return Returning.Success(10);
        });
        
        Returning<int> res3 = await  Returning<int>.TryTask(()=>
        {
            Returning.Error("ReturningErrorException")
                     .Throw();
            return Returning.Success(10);
        });
        Returning<int> res4 = await  Returning<int>.TryTask(()=>
        {
            Returning.Unfinished("ReturningUnfinishedException")
                     .Throw();
            return Returning.Success(10);
        });
        
       
        Console.WriteLine(res.ToString());
        Assert.AreEqual(res.Ok, true);
        Assert.AreEqual(res2.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res3.ResultType, Returning.TypeResult.Error);
        Assert.AreEqual(res4.ResultType, Returning.TypeResult.Unfinished);
    }
}
