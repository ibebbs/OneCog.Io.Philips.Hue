using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue
{
    public interface ICacheEntry<TKey, TValue> : IConnectableObservable<TValue> { TKey Key { get; } } 

    public static class Ex 
    { 
        public static IConnectableObservable<ICacheEntry<TKey, TValue>> Cache<TKey, TValue> (this IObservable<TValue> source, Func<TValue, TKey> keySelector)
        {
            var query = from i in source 
                        group i by keySelector(i) into grp
                        select new CacheEntry<TKey, TValue>(grp.Key, grp.Replay(1)); 
            
            return Observable.Create<ICacheEntry<TKey, TValue>>(
                o => 
                { 
                    var disposable = new CompositeDisposable(); 
                    var subsription = query
                        .Do(ce => disposable.Add(ce.Connect()))
                        .Select(ce => (ICacheEntry<TKey, TValue>) ce) // remove on .net 4.0 
                        .Subscribe(o);
                    
                    return disposable; 
                }
             ).Replay();
        }
        
        private class CacheEntry<TKey, TValue> : ICacheEntry<TKey, TValue> 
        { 
            private readonly TKey _key; 
            private readonly IConnectableObservable<TValue> _underlying; 
            
            public TKey Key 
            { 
                get { return _key; } 
            }
            
            public CacheEntry(TKey key, IConnectableObservable<TValue> underlying) 
            { 
                _key = key; 
                _underlying = underlying; 
            } 
            
            public IDisposable Connect() 
            {
                return _underlying.Connect(); 
            } 
            
            public IDisposable Subscribe(IObserver<TValue> observer) 
            { 
                return _underlying.Subscribe(observer); 
            }
        }
    }
    
     class Update 
     { 
         public int Id { get; set; } 
         public string Message { get; set; } 
     }

     public class Program
     {
         static void Main(string[] args)
         {
             var source = new Subject<Update>();
             var cache = source.Cache(u => u.Id);
             cache.Connect(); // make cache hot 

             var query = from entry in cache
                         from update in entry
                         select update;

             source.OnNext(new Update { Id = 1, Message = "should never see this" });
             source.OnNext(new Update { Id = 1, Message = "should see this" });

             query.Subscribe(u => Console.WriteLine(u.Id + " " + u.Message));

             source.OnNext(new Update { Id = 2, Message = "should see this" });
             source.OnNext(new Update { Id = 1, Message = "should see this" });
         }
     }
}
