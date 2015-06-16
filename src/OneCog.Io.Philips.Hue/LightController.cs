using Colourful;
using Colourful.Conversion;
using Colourful.Difference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace OneCog.Io.Philips.Hue
{
    public class LightController : ISubject<Light.ISource>
    {
        private static readonly ColourfulConverter _converter = new ColourfulConverter();
        private static readonly CIE76ColorDifference _comparer = new CIE76ColorDifference();

        private readonly IScheduler _scheduler;
        private readonly Subject<Light.ISource> _input;
        private readonly Subject<Light.ISource> _output;

        private IDisposable _subscription;

        public LightController(IScheduler scheduler = null)
        {
            _scheduler = scheduler ?? TaskPoolScheduler.Default;

            _input = new Subject<Light.ISource>();
            _output = new Subject<Light.ISource>();
        }

        private Tuple<Light.ISource, Light.ISource, double> GetDifferences(Light.ISource desired, Light.ISource current)
        {
            LabColor desiredColor = _converter.ToLab(desired.Color);
            LabColor currentColor = _converter.ToLab(current.Color);

            double difference = _comparer.ComputeDifference(desiredColor, currentColor);

            return Tuple.Create(desired, current, difference);
        }

        private Tuple<Light.ISource, Light.ISource> GetLightValues(KeyValuePair<uint, Light.ISource> desired, IEnumerable<KeyValuePair<uint, Light.ISource>> current)
        {
            Light.ISource actual = current.Select(kvp => kvp.Value).FirstOrValue(new Light.Bulb { Id = desired.Value.Id, Color = new RGBColor(0.0, 0.0, 0.0) });

            return Tuple.Create(desired.Value, actual);
        } 

        private IEnumerable<Tuple<Light.ISource, Light.ISource, double>> GetDifferences(IReadOnlyDictionary<uint, Light.ISource> desired, IReadOnlyDictionary<uint, Light.ISource> current)
        {
            return Enumerable
                .GroupJoin(desired, current, item => item.Key, item => item.Key, GetLightValues)
                .Select(tuple => GetDifferences(tuple.Item1, tuple.Item2))
                .Where(tuple => tuple.Item3 != 0.0)
                .OrderBy(tuple => Math.Abs(tuple.Item3));
        }

        public IDisposable Connect(IEnumerable<Light.ISource> initialState)
        {
            return Observable
                .CombineLatest(_input.ByIndex(), _output.ByIndex().StartWith(initialState.ToDictionary(light => light.Id)), GetDifferences)
                .SelectMany(differences => differences.Select(tuple => tuple.Item1).Take(1))
                .Sample(TimeSpan.FromMilliseconds(10), _scheduler)
                .Subscribe(_output);
        }

        public void OnCompleted()
        {
            if (_subscription != null)
            {
                _output.OnCompleted();

                _subscription.Dispose();
                _subscription = null;
            }
        }

        public void OnError(Exception error)
        {
            if (_subscription != null)
            {
                _output.OnError(error);

                _subscription.Dispose();
                _subscription = null;
            }
        }

        public void OnNext(Light.ISource value)
        {
            _input.OnNext(value);
        }

        public IDisposable Subscribe(IObserver<Light.ISource> observer)
        {
            return _output.Subscribe(observer);
        }
    }
}
