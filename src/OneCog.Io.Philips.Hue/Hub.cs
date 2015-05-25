using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue
{
    public class Hub : IDisposable
    {
        private readonly IApi _api;
        private readonly Func<IInteraction> _pressLinkButton;

        private readonly Subject<ILight> _lighting;
        private readonly IConnectableObservable<ILight> _lightingObservable;
        private IDisposable _lightingSubscription;

        public Hub(IApi api, Func<IInteraction> pressLinkButton)
        {
            _api = api;
            _pressLinkButton = pressLinkButton;

            _lighting = new Subject<ILight>();
            _lightingObservable = _lighting.GroupLatest(light => light.Id);
        }

        public Hub(Uri hubAddress, string userName, string deviceType, Func<IInteraction> pressLinkButton)
            : this(new Api(new Lights.Api(), new Client(hubAddress), userName, deviceType), pressLinkButton)
        {
        }

        public void Dispose()
        {
            if (_lightingSubscription != null)
            {
                _lightingSubscription.Dispose();
                _lightingSubscription = null;
            }
        }
        
        public async Task Connect(CancellationToken cancellationToken)
        {
            _lightingSubscription = _lightingObservable.Connect();

            Dto.IState state = await _api.Connect(_pressLinkButton, cancellationToken);

            var lights = state.Lights
                .Select(indexedLight => new Light { Id = (uint)indexedLight.Index, Color = Colourful.RGBColor.FromRGB8bit(0, 0, 0) });

            foreach (ILight light in lights)
            {
                _lighting.OnNext(light);
            }
        }

        public Task SetLight(ILight light)
        {
            _lighting.OnNext(light);

            return Task.FromResult<object>(null);
        }

        public IObservable<ILight> Lighting
        {
            get { return _lightingObservable; }
        }
    }
}
