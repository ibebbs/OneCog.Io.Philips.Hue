using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue
{
    public static class ReactiveExtensions
    {
        private class Grouped<TKey, TItem> : IConnectableObservable<TItem>
        {
            private ConcurrentDictionary<TKey, TItem> _dictionary = new ConcurrentDictionary<TKey, TItem>();
            
            private IConnectableObservable<TItem> _connectable;

            public Grouped(IObservable<TItem> source, Func<TItem, TKey> keySelector)
            {
                _connectable = source
                    .Select(item => new { Key = keySelector(item), Item = item })
                    .Select(kvp => _dictionary.AddOrUpdate(kvp.Key, kvp.Item, (k, i) => kvp.Item))
                    .Publish();

            }

            public IDisposable Connect()
            {
                return _connectable.Connect();
            }

            public IDisposable Subscribe(IObserver<TItem> observer)
            {
                return _connectable.StartWith(_dictionary.Values).Subscribe(observer);
            }
        }

        public static IConnectableObservable<TItem> GroupLatest<TKey, TItem>(this IObservable<TItem> source, Func<TItem, TKey> keySelector)
        {
            return new Grouped<TKey, TItem>(source, keySelector);
        }

        public static IObservable<IReadOnlyDictionary<uint, Light.ISource>> ByIndex(this IObservable<Light.ISource> source)
        {
            return source.Scan(
                new Dictionary<uint, Light.ISource>(),
                (grouped, light) =>
                {
                    grouped[light.Id] = light;
                    return grouped;
                }
            );
        }
    }
}
