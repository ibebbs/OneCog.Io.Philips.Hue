using OneCog.Io.Philips.Hue.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue
{
    public interface IApi
    {
        /// <summary>
        /// Connects to the Hue hub
        /// </summary>
        /// <param name="pressLinkButton"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="CancellationRequestedException">If cancellation token is used to cancel the operation</exception>
        Task<Dto.IState> Connect(Func<IInteraction> pressLinkButton, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the current <see cref="Dto.IState"/> 
        /// </summary>
        /// <returns></returns>
        Task<Dto.IState> GetState();

        /// <summary>
        /// Sets the state of the <see cref="Light.Source"/>
        /// </summary>
        /// <param name="light"></param>
        /// <returns></returns>
        Task<Light.ISource> Set(Light.ISource light);
    }

    public class Api : IApi
    {
        private readonly IClient _client;
        private readonly string _user;
        private readonly string _deviceType;

        public Api(IClient client, string user, string deviceType)
        {
            _client = client;
            _user = user;
            _deviceType = deviceType;
        }

        private async Task<bool> CreateUser(Func<IInteraction> pressLinkButton)
        {
            IInteraction interaction = pressLinkButton();

            await interaction.Completion;

            User user = new User(_user, _deviceType);

            string content = Serializer.Json.Serializer(user);

            bool result = await _client.Post(new Uri("/api", UriKind.Relative), content);

            return result;
        }

        public async Task<IState> Connect(Func<IInteraction> pressLinkButton, CancellationToken cancellationToken)
        {
            IState state = null;

            while (state == null)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    state = await GetState();
                }
                catch (UnauthorizedAccessException)
                {
                    // Do nothing
                }

                if (state == null)
                {
                    bool createdUser = await CreateUser(pressLinkButton);
                }
            }

            return state;
        }

        public async Task<Dto.IState> GetState()
        {
            var result = await _client.Get(new Uri("/api/{user}", UriKind.Relative), new Dictionary<string, string> { { "user", _user } });

            return Dto.State.FromJson(result);
        }

        public async Task<Light.ISource> Set(Light.ISource light)
        {
            var state = light.Color.ToLightState();

            string content = Serializer.Json.Serializer(state);

            var result = await _client.Put(
                new Uri("/api/<username>/lights/<id>/state", UriKind.Relative),
                new Dictionary<string, string> { { "username", _user }, { "id", light.Id.ToString() } },
                content
            );

            // TODO: Return new instance of light modified with the successful state changes
            return light;
        }
    }
}
