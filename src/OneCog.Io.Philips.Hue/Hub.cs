using Colourful;
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

        private readonly Subject<Light.ISource> _lighting;
        private readonly IConnectableObservable<Light.ISource> _lightingObservable;
        private IDisposable _lightingSubscription;

        public Hub(IApi api, Func<IInteraction> pressLinkButton)
        {
            _api = api;
            _pressLinkButton = pressLinkButton;

            _lighting = new Subject<Light.ISource>();
            _lightingObservable = _lighting.GroupLatest(light => light.Id);
        }

        public Hub(IClient client, string userName, string deviceType, Func<IInteraction> pressLinkButton)
            : this(new Api(client, userName, deviceType), pressLinkButton) { }

        public Hub(Uri hubAddress, string userName, string deviceType, Func<IInteraction> pressLinkButton)
            : this(new Client(hubAddress), userName, deviceType, pressLinkButton)
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
                .Select(indexedLight => new Light.Bulb { Id = (uint)indexedLight.Index, Color = Colourful.RGBColor.FromRGB8bit(0, 0, 0) });

            foreach (Light.ISource light in lights)
            {
                _lighting.OnNext(light);
            }
        }

        public Task SetLight(uint index, IColorVector color)
        {
            _lighting.OnNext(new Light.Bulb { Id = index, Color = color });

            return Task.FromResult<object>(null);
        }

        public IObservable<Light.ISource> Lighting
        {
            get { return _lightingObservable; }
        }
    }
}
