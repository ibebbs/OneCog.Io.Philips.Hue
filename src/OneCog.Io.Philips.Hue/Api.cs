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
        Task<bool> Connect(Func<IInteraction> pressLinkButton, CancellationToken cancellationToken);

        Task<Dto.IState> GetState();

        Lights.IApi Lights { get; }
    }

    public class Api : IApi
    {
        private readonly IClient _client;
        private readonly string _user;
        private readonly string _deviceType;

        public Api(Lights.IApi lights, IClient client, string user, string deviceType)
        {
            _client = client;
            _user = user;
            _deviceType = deviceType;

            Lights = lights;
        }

        public async Task<bool> Connect(Func<IInteraction> pressLinkButton, CancellationToken cancellationToken)
        {
            bool connected = false;

            while (!connected && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await GetState();
                    connected = true;
                }
                catch (UnauthorizedAccessException)
                {
                    // Do nothing
                }

                if (!connected)
                {
                    bool createdUser = await CreateUser(pressLinkButton);
                }
            }

            return connected;
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

        public async Task<Dto.IState> GetState()
        {
            var result = await _client.Get(new Uri("/api/{user}", UriKind.Relative), new Dictionary<string, string> { { "user", _user } });

            return Dto.State.FromJson(result);
        }

        public Lights.IApi Lights { get; private set; }
    }
}
