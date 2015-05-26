using FakeItEasy;
using FakeItEasy.Core;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue.Tests
{
    [TestFixture]
    public class ApiTestFixture
    {
        private const string UserName = "TestUser";
        private const string DeviceType = "TestDevice";

        private static Task RunTest(Func<IClient, Api, Task> test)
        {
            IClient client = A.Fake<IClient>();
            A.CallTo(() => client.Get(A<Uri>.Ignored)).Throws(new InvalidOperationException("Resource Not Found"));
            A.CallTo(() => client.Post(A<Uri>.Ignored, A<string>.Ignored)).Throws(new InvalidOperationException("Resource Not Found"));

            Api subject = new Api(client, UserName, DeviceType);

            return test(client, subject);
        }

        private static bool JsonEquals(string x, string y)
        {
            JToken expected = JToken.Parse(x);
            JToken actual = JToken.Parse(y);

            return JToken.DeepEquals(expected, actual);
        }

        public class Connect
        {
            public class Should
            {
                [Test]
                public async Task AttemptToGetStateWhenCalled()
                {
                    await RunTest(
                        async (client, subject) =>
                        {
                            Func<IInteraction> pressLinkButton = () => A.Fake<IInteraction>();

                            A.CallTo(
                                () => client.Get(
                                    A<Uri>.That.Matches(
                                        uri => string.Equals(uri.OriginalString, "/api/{user}", StringComparison.InvariantCultureIgnoreCase)
                                    ),
                                    A<IReadOnlyDictionary<string, string>>.That.Matches(
                                        dictionary => dictionary["user"] == UserName
                                    )
                                )
                            ).Returns(Resources.ConfigurationResponse);

                            Dto.IState state = await subject.Connect(pressLinkButton, CancellationToken.None);

                            A.CallTo(
                                () => client.Get(
                                    A<Uri>.That.Matches(
                                        uri => string.Equals(uri.OriginalString, "/api/{user}", StringComparison.InvariantCultureIgnoreCase)
                                    ),
                                    A<IReadOnlyDictionary<string, string>>.That.Matches(
                                        dictionary => dictionary["user"] == UserName
                                    )
                                )
                            ).MustHaveHappened(Repeated.Exactly.Once);
                        }
                    );
                }

                [Test]
                public async Task AttemptInvokePressLinkButtonIfNotAuthorisedToGetState()
                {
                    await RunTest(
                        async (client, subject) =>
                        {
                            bool userCreated = false;

                            IInteraction interaction = A.Fake<IInteraction>();
                            A.CallTo(() => interaction.Completion).Returns(Task.FromResult<object>(null));

                            Func<IInteraction> pressLinkButton = () => interaction;

                            Func<IFakeObjectCall, Task<string>> handler =
                                args =>
                                {
                                    if (userCreated)
                                    {
                                        return Task.FromResult<string>("{ \"state\": \"test\" }");
                                    }
                                    else
                                    {
                                        throw new UnauthorizedAccessException();
                                    }
                                };

                            A.CallTo(
                                () => client.Get(
                                    A<Uri>.That.Matches(uri => uri == new Uri("/api/{user}", UriKind.Relative)),
                                    A<IReadOnlyDictionary<string, string>>.That.Matches(
                                        dictionary => dictionary["user"] == UserName
                                    )
                                )
                            ).ReturnsLazily(handler);

                            A.CallTo(() => client.Post(A<Uri>.Ignored, A<string>.Ignored)).Invokes(() => userCreated = true).Returns(true);

                            Dto.IState state = await subject.Connect(pressLinkButton, CancellationToken.None);

                            A.CallTo(() => interaction.Completion).MustHaveHappened(Repeated.Exactly.Once);
                        }
                    );
                }

                [Test]
                public async Task NotAttemptInvokePressLinkButtonIfAuthorisedToGetState()
                {
                    await RunTest(
                        async (client, subject) =>
                        {
                            IInteraction interaction = A.Fake<IInteraction>();
                            A.CallTo(() => interaction.Completion).Returns(Task.FromResult<object>(null));

                            Func<IInteraction> pressLinkButton = () => interaction;

                            A.CallTo(
                                () => client.Get(
                                    A<Uri>.That.Matches(uri => uri == new Uri("/api/{user}", UriKind.Relative)),
                                    A<IReadOnlyDictionary<string, string>>.That.Matches(
                                        dictionary => dictionary["user"] == UserName
                                    )
                                )
                            ).Returns("{ \"state\": \"test\" }");

                            Dto.IState state = await subject.Connect(pressLinkButton, CancellationToken.None);

                            A.CallTo(() => interaction.Completion).MustNotHaveHappened();
                        }
                    );
                }

                [Test]
                public async Task AttemptCreateUserAfterLinkButtonPressed()
                {
                    await RunTest(
                        async (client, subject) =>
                        {
                            bool userCreated = false;

                            IInteraction interaction = A.Fake<IInteraction>();
                            A.CallTo(() => interaction.Completion).Returns(Task.FromResult<object>(null));

                            Func<IInteraction> pressLinkButton = () => interaction;

                            Func<IFakeObjectCall, Task<string>> handler =
                                args =>
                                {
                                    if (userCreated)
                                    {
                                        return Task.FromResult<string>("{ \"state\": \"test\" }");
                                    }
                                    else
                                    {
                                        throw new UnauthorizedAccessException();
                                    }
                                };

                            A.CallTo(
                                () => client.Get(
                                    A<Uri>.That.Matches(uri => uri == new Uri("/api/{user}", UriKind.Relative)),
                                    A<IReadOnlyDictionary<string, string>>.That.Matches(
                                        dictionary => dictionary["user"] == UserName
                                    )
                                )
                            ).ReturnsLazily(handler);

                            A.CallTo(
                                () => client.Post(
                                    A<Uri>.That.Matches(uri => uri == new Uri("/api", UriKind.Relative)),
                                    A<string>.That.Matches(content => JsonEquals("{\"devicetype\": \"TestDevice\", \"username\": \"TestUser\"}", content))
                                )
                            ).Invokes(() => userCreated = true).Returns(true);

                            Dto.IState state = await subject.Connect(pressLinkButton, CancellationToken.None);

                            A.CallTo(
                                () => client.Post(
                                    A<Uri>.That.Matches(uri => uri == new Uri("/api", UriKind.Relative)),
                                    A<string>.That.Matches(content => JsonEquals("{\"devicetype\": \"TestDevice\", \"username\": \"TestUser\"}", content))
                                )
                            ).MustHaveHappened(Repeated.Exactly.Once);
                        }
                    );
                }
            }
        }
    }
}
