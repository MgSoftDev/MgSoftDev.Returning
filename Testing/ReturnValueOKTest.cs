using MgSoftDev.ReturningCore;
using MgSoftDev.ReturningCore.Exceptions;
using MgSoftDev.ReturningCore.Helper;

namespace Testing;

public class ReturnValueOKTest
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
    public void TryGetReturningGenericTest()
    {
        var opt = 1;
        var res = TryGetReturningGeneric(opt);
        Assert.IsTrue(res.Value == opt);
        
        opt = 2;
        res = TryGetReturningGeneric(opt);
        Assert.IsTrue(res.Value == opt);
        
        opt = 3;
        res = TryGetReturningGeneric(opt);
        Assert.IsTrue(res.Value == opt);
        
        opt = 10;
        res = TryGetReturningGeneric(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 11;
        res = TryGetReturningGeneric(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 12;
        res = TryGetReturningGeneric(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 13;
        res = TryGetReturningGeneric(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 14;
        res = TryGetReturningGeneric(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 15;
        res = TryGetReturningGeneric(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 16;
        res = TryGetReturningGeneric(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 17;
        res = TryGetReturningGeneric(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 18;
        res = TryGetReturningGeneric(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 19;
        res = TryGetReturningGeneric(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        
    }


    public Returning<int> TryGetReturningGeneric(int opt)
    {
        return Returning<int>.Try(()=>
        {
            if (opt == 1) return 1;
            if (opt == 2) return Returning.Success(2);
            if (opt == 3) return Returning<int>.Try(()=>3);

            if (opt == 10) return Returning.Error(opt.ToString());
            if (opt == 11) return Returning.Unfinished(opt.ToString());
            if (opt == 12) return Returning<int>.Try(()=>Returning.Error(opt.ToString()));
            if (opt == 13) return Returning<int>.Try(()=>Returning.Unfinished(opt.ToString()));
            if (opt == 14) return ReturningList<int>.Try(()=>Returning.Error(opt.ToString()));
            if (opt == 15) return ReturningList<int>.Try(()=>Returning.Unfinished(opt.ToString()));
            if (opt == 16) return Returning.Try(()=>Returning.Error(opt.ToString()));
            if (opt == 17) return Returning.Try(()=>Returning.Unfinished(opt.ToString()));
            if (opt == 18) return Returning<List<string>>.Try(()=>Returning.Error(opt.ToString())).ToReturning<int>();
            if (opt == 19) return Returning<List<string>>.Try(()=>Returning.Unfinished(opt.ToString())).ToReturning<int>();

            return 0;
        });
    }

    [ Test ]
    public void TryGetReturningGenericListTest()
    {
        var opt = 1;
        var res = TryGetReturningGenericList(opt);
        Assert.IsTrue(res.Value.Count == opt);
        
        opt = 2;
        res = TryGetReturningGenericList(opt);
        Assert.IsTrue(res.Value.Count == opt);
        
        opt = 3;
        res = TryGetReturningGenericList(opt);
        Assert.IsTrue(res.Value.Count == opt);
        
        opt = 4;
        res = TryGetReturningGenericList(opt);
        Assert.IsTrue(res.Value.Count == opt);
        
        opt = 10;
        res = TryGetReturningGenericList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 11;
        res = TryGetReturningGenericList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 12;
        res = TryGetReturningGenericList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 13;
        res = TryGetReturningGenericList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 14;
        res = TryGetReturningGenericList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 15;
        res = TryGetReturningGenericList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 16;
        res = TryGetReturningGenericList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 17;
        res = TryGetReturningGenericList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 18;
        res = TryGetReturningGenericList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 19;
        res = TryGetReturningGenericList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
    }
    
    public Returning<List<int>> TryGetReturningGenericList(int opt)
    {
        return Returning<List<int>>.Try(()=>
        {
            if (opt == 1)
                return new List<int>()
                {
                    1
                };
            if (opt == 2)
                return Returning.Success(new List<int>()
                {
                    1,
                    2
                });
            if (opt == 3)
                return Returning<List<int>>.Try(()=>new List<int>()
                {
                    1,
                    2,
                    3
                });
            if (opt == 4)
                return ReturningList<int>.Try(()=>new List<int>()
                {
                    1,
                    2,
                    3,
                    4
                });

            if (opt == 10) return Returning.Error(opt.ToString());
            if (opt == 11) return Returning.Unfinished(opt.ToString());
            if (opt == 12) return Returning<int>.Try(()=>Returning.Error(opt.ToString())).ToReturningList<int>();
            if (opt == 13) return Returning<int>.Try(()=>Returning.Unfinished(opt.ToString())).ToReturningList<int>();
            if (opt == 14) return ReturningList<int>.Try(()=>Returning.Error(opt.ToString()));
            if (opt == 15) return ReturningList<int>.Try(()=>Returning.Unfinished(opt.ToString()));
            if (opt == 16) return Returning.Try(()=>Returning.Error(opt.ToString()));
            if (opt == 17) return Returning.Try(()=>Returning.Unfinished(opt.ToString()));
            if (opt == 18) return Returning<List<int>>.Try(()=>Returning.Error(opt.ToString()));
            if (opt == 19) return Returning<List<int>>.Try(()=>Returning.Unfinished(opt.ToString()));

            return new List<int>();
        });
    }

    [ Test ]
    public void TryGetReturningListTest()
    {
        var opt = 1;
        var res = TryGetReturningList(opt);
        Assert.IsTrue(res.Value.Count == opt);
        
        opt = 2;
        res = TryGetReturningList(opt);
        Assert.IsTrue(res.Value.Count == opt);
        
        opt = 3;
        res = TryGetReturningList(opt);
        Assert.IsTrue(res.Value.Count == opt);
        
        opt = 4;
        res = TryGetReturningList(opt);
        Assert.IsTrue(res.Value.Count == opt);
        
        opt = 10;
        res = TryGetReturningList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 11;
        res = TryGetReturningList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 12;
        res = TryGetReturningList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 13;
        res = TryGetReturningList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 14;
        res = TryGetReturningList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 15;
        res = TryGetReturningList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 16;
        res = TryGetReturningList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 17;
        res = TryGetReturningList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 18;
        res = TryGetReturningList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 19;
        res = TryGetReturningList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
    }
    public ReturningList<int> TryGetReturningList(int opt)
    {
        return ReturningList<int>.Try(()=>
        {
            if (opt == 1)
                return new List<int>()
                {
                    1
                };
            if (opt == 2)
                return Returning.Success(new List<int>()
                {
                    1,
                    2
                });
            if (opt == 3)
                return ReturningList<int>.Try(()=>new List<int>()
                {
                    1,
                    2,
                    3
                });
            if (opt == 4)
                return Returning<List<int>>.Try(()=>new List<int>()
                {
                    1,
                    2,
                    3,
                    4
                });

            if (opt == 10) return Returning.Error(opt.ToString());
            if (opt == 11) return Returning.Unfinished(opt.ToString());
            if (opt == 12) return Returning<int>.Try(()=>Returning.Error(opt.ToString()));
            if (opt == 13) return Returning<int>.Try(()=>Returning.Unfinished(opt.ToString()));
            if (opt == 14) return ReturningList<int>.Try(()=>Returning.Error(opt.ToString()));
            if (opt == 15) return ReturningList<int>.Try(()=>Returning.Unfinished(opt.ToString()));
            if (opt == 16) return Returning.Try(()=>Returning.Error(opt.ToString()));
            if (opt == 17) return Returning.Try(()=>Returning.Unfinished(opt.ToString()));
            if (opt == 18) return Returning<List<string>>.Try(()=>Returning.Error(opt.ToString())).ToReturningList<int>();
            if (opt == 19) return Returning<List<string>>.Try(()=>Returning.Unfinished(opt.ToString())).ToReturningList<int>();

            return new List<int>();
        });
    }

[ Test ]
    public async Task TaskTryGetReturningGenericTest()
    {
        var opt = 1;
        var res = await TaskTryGetReturningGeneric(opt);
        Assert.IsTrue(res.Value == opt);
        
        opt = 2;
        res = await TaskTryGetReturningGeneric(opt);
        Assert.IsTrue(res.Value == opt);
        
        
        opt = 3;
        res = await TaskTryGetReturningGeneric(opt);
        Assert.IsTrue(res.Value == opt);
        
        opt = 10;
        res = await TaskTryGetReturningGeneric(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 11;
        res = await TaskTryGetReturningGeneric(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 12;
        res = await TaskTryGetReturningGeneric(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 13;
        res = await TaskTryGetReturningGeneric(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 14;
        res = await TaskTryGetReturningGeneric(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 15;
        res = await TaskTryGetReturningGeneric(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 16;
        res = await TaskTryGetReturningGeneric(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 17;
        res = await TaskTryGetReturningGeneric(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        
    }

    public Task<Returning<int>> TaskTryGetReturningGeneric(int opt)
    {
        return Returning<int>.TryTask(async ()=>
        {
            if (opt == 1) return 1;
            if (opt == 2) return Returning.Success(2);
            if (opt == 3) return await Returning<int>.TryTask(()=>3);

            if (opt == 10) return Returning.Error(opt.ToString());
            if (opt == 11) return Returning.Unfinished(opt.ToString());
            if (opt == 12) return await Returning<int>.TryTask(()=>Returning.Error(opt.ToString()));
            if (opt == 13) return await Returning<int>.TryTask(()=>Returning.Unfinished(opt.ToString()));
            if (opt == 14) return await ReturningList<int>.TryTask(()=>Returning.Error(opt.ToString()));
            if (opt == 15) return await ReturningList<int>.TryTask(()=>Returning.Unfinished(opt.ToString()));
            if (opt == 16) return await Returning.TryTask(()=>Returning.Error(opt.ToString()));
            if (opt == 17) return await Returning.TryTask(()=>Returning.Unfinished(opt.ToString()));

            return 0;
        });
    }


    [ Test ]
    public async Task TaskTryGetReturningGenericListTest()
    {
        var opt = 1;
        var res = await TaskTryGetReturningGenericList(opt);
        Assert.IsTrue(res.Value.Count == opt);
        
        opt = 2;
        res = await TaskTryGetReturningGenericList(opt);
        Assert.IsTrue(res.Value.Count == opt);
        
        opt = 3;
        res = await TaskTryGetReturningGenericList(opt);
        Assert.IsTrue(res.Value.Count == opt);
        
        opt = 4;
        res = await TaskTryGetReturningGenericList(opt);
        Assert.IsTrue(res.Value.Count == opt);
        
        opt = 10;
        res = await TaskTryGetReturningGenericList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 11;
        res = await TaskTryGetReturningGenericList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 12;
        res = await TaskTryGetReturningGenericList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 13;
        res = await TaskTryGetReturningGenericList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 14;
        res = await TaskTryGetReturningGenericList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 15;
        res = await TaskTryGetReturningGenericList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 16;
        res = await TaskTryGetReturningGenericList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 17;
        res = await TaskTryGetReturningGenericList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 18;
        res = await TaskTryGetReturningGenericList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 19;
        res = await TaskTryGetReturningGenericList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
    }
    
    public Task<Returning<List<int>>> TaskTryGetReturningGenericList(int opt)
    {
        return Returning<List<int>>.TryTask(async ()=>
        {
            if (opt == 1)
                return new List<int>()
                {
                    1
                };
            if (opt == 2)
                return Returning.Success(new List<int>()
                {
                    1,
                    2
                });
            if (opt == 3)
                return await Returning<List<int>>.TryTask(()=>new List<int>()
                {
                    1,
                    2,
                    3
                });
            if (opt == 4)
                return ReturningList<int>.Try(()=>new List<int>()
                {
                    1,
                    2,
                    3,
                    4
                });

            if (opt == 10) return Returning.Error(opt.ToString());
            if (opt == 11) return Returning.Unfinished(opt.ToString());
            if (opt == 12) return Returning<int>.Try(()=>Returning.Error(opt.ToString())).ToReturningList<int>();
            if (opt == 13) return Returning<int>.Try(()=>Returning.Unfinished(opt.ToString())).ToReturningList<int>();
            if (opt == 14) return await ReturningList<int>.TryTask(()=>Returning.Error(opt.ToString()));
            if (opt == 15) return ReturningList<int>.Try(()=>Returning.Unfinished(opt.ToString()));
            if (opt == 16) return await Returning.TryTask(()=>Returning.Error(opt.ToString()));
            if (opt == 17) return Returning.Try(()=>Returning.Unfinished(opt.ToString()));
            if (opt == 18) return await Returning<List<int>>.TryTask(()=>Returning.Error(opt.ToString()));
            if (opt == 19) return Returning<List<int>>.Try(()=>Returning.Unfinished(opt.ToString()));

            return new List<int>();
        });
    }

    [ Test ]
    public async Task TaskTryGetReturningListTest()
    {
        var opt = 1;
        var res = await TaskTryGetReturningList(opt);
        Assert.IsTrue(res.Value.Count == opt);
        
        opt = 2;
        res = await TaskTryGetReturningList(opt);
        Assert.IsTrue(res.Value.Count == opt);
        
        opt = 3;
        res = await TaskTryGetReturningList(opt);
        Assert.IsTrue(res.Value.Count == opt);
        
        opt = 4;
        res = await TaskTryGetReturningList(opt);
        Assert.IsTrue(res.Value.Count == opt);
        
        opt = 10;
        res = await TaskTryGetReturningList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 11;
        res = await TaskTryGetReturningList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 12;
        res = await TaskTryGetReturningList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 13;
        res = await TaskTryGetReturningList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 14;
        res = await TaskTryGetReturningList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 15;
        res = await TaskTryGetReturningList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 16;
        res = await TaskTryGetReturningList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 17;
        res = await TaskTryGetReturningList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
        
        opt = 18;
        res = await TaskTryGetReturningList(opt);
        Assert.IsTrue(res.ErrorInfo.ErrorMessage == opt.ToString());
        
        opt = 19;
        res = await TaskTryGetReturningList(opt);
        Assert.IsTrue(res.UnfinishedInfo.Title == opt.ToString());
    }
    
    public Task<ReturningList<int>> TaskTryGetReturningList(int opt)
    {
        return ReturningList<int>.TryTask(async ()=>
        {
            if (opt == 1)
                return new List<int>()
                {
                    1
                };
            if (opt == 2)
                return Returning.Success(new List<int>()
                {
                    1,
                    2
                });
            if (opt == 3)
                return await ReturningList<int>.TryTask(()=>new List<int>()
                {
                    1,
                    2,
                    3
                });
            if (opt == 4)
                return Returning<List<int>>.Try(()=>new List<int>()
                {
                    1,
                    2,
                    3,
                    4
                });

            if (opt == 10) return Returning.Error(opt.ToString());
            if (opt == 11) return Returning.Unfinished(opt.ToString());
            if (opt == 12) return Returning<int>.Try(()=>Returning.Error(opt.ToString()));
            if (opt == 13) return await Returning<int>.TryTask(()=>Returning.Unfinished(opt.ToString()));
            if (opt == 14) return ReturningList<int>.Try(()=>Returning.Error(opt.ToString()));
            if (opt == 15) return await ReturningList<int>.TryTask(()=>Returning.Unfinished(opt.ToString()));
            if (opt == 16) return Returning.Try(()=>Returning.Error(opt.ToString()));
            if (opt == 17) return await Returning.TryTask(()=>Returning.Unfinished(opt.ToString()));
            if (opt == 18) return Returning<List<string>>.Try(()=>Returning.Error(opt.ToString())).ToReturningList<int>();
            if (opt == 19) return Returning<List<string>>.Try(()=>Returning.Unfinished(opt.ToString())).ToReturningList<int>();

            return new List<int>();
        });
    }
}
