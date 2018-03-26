using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace RxSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to terminate");

            #region Create & Return

            //Observable.Create<int>(obs =>
            //    {
            //        obs.OnNext(10);
            //        obs.OnNext(20);
            //        //obs.OnError(new Exception("Some Error"));
            //        obs.OnCompleted();
            //        return Disposable.Empty;
            //    }).Dump<int>("Observable Create"); 

            //var result = 0;

            //Observable.Return(5 /result).Dump("Observable Return");

            #endregion

            #region Unfold

            //Observable.Range(1, 5).Dump("Observable Range");

            //Observable.Interval(TimeSpan.FromSeconds(2)).Dump("Observable Interval"); 

            //Observable.Generate(0,
            //                    x => x < 5,     // Condition
            //                    x => x + 1,     // iterator
            //                    x => x * x)     // result selector
            //                    .Dump("Observable Generate");

            #endregion

            #region Time Sequence related

            // Buffer
            //var source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(10);
            //source.Buffer(3)
            //    .Subscribe(buffer =>
            //    {
            //        Console.WriteLine("--Buffered values");
            //        foreach (var value in buffer)
            //        {

            //            Console.WriteLine(value);
            //        }
            //    }, () => Console.WriteLine("Completed"));



            // Window
            //var windowIdx = 0;
            //var source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(10);
            //source.Window(3)

            //.Subscribe(window =>
            //{

            //    var id = windowIdx++;
            //    Console.WriteLine("--Starting new window");
            //    var windowName = "Window" + windowIdx;
            //    window.Subscribe(

            //    value => Console.WriteLine("{0} : {1}", windowName, value),
            //    ex => Console.WriteLine("{0} : {1}", windowName, ex),
            //    () => Console.WriteLine("{0} Completed", windowName));
            //},
            //() => Console.WriteLine("Completed"));

            #endregion

            Console.ReadKey();
        }

    }

    public static class ObservableExtentions
    {
        public static void Dump<T>(this IObservable<T> source, string name)
        {
            source.Subscribe(
            i => Console.WriteLine("{0}-->{1}", name, i),
            ex => Console.WriteLine("{0} failed-->{1}", name, ex.Message),
            () => Console.WriteLine("{0} completed", name));
        }
    }

}
