using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue.Tests
{
    [TestFixture]
    public class HubTestFixture
    {
        public class Lighting
        {
            public class Should
            {
                [Test]
                public async Task ReturnCurrentLightingStateOfEachLightOnSubscription()
                {
                    IApi api = A.Fake<IApi>();
                    A.CallTo(() => api.Connect(A<Func<IInteraction>>.Ignored, A<CancellationToken>.Ignored))
                     .Returns(new Dto.State
                        {
                            Lights = new Dto.Index<Dto.Light>(
                                new Dto.Indexed<Dto.Light>[] { 
                                    new Dto.Indexed<Dto.Light> { Index = 1, Item = new Dto.Light { Name = "Item1", State = new Dto.LightState { On = true, ColorMode = "xy", Xy = new [] { 0.3972, 0.4564 } } } },
                                    new Dto.Indexed<Dto.Light> { Index = 2, Item = new Dto.Light { Name = "Item2", State = new Dto.LightState { On = true, ColorMode = "xy", Xy = new [] { 0.5425, 0.4196 } } } },
                                    new Dto.Indexed<Dto.Light> { Index = 3, Item = new Dto.Light { Name = "Item3", State = new Dto.LightState { On = true, ColorMode = "xy", Xy = new [] { 0.41, 0.51721 } } } }
                                }  
                            )
                        }
                    );

                    Hub hub = new Hub(api, () => A.Fake<IInteraction>());

                    await hub.Connect(CancellationToken.None);

                    List<Light.ISource> actual = new List<Light.ISource>();
                    
                    hub.Lighting.Subscribe(actual.Add);

                    Assert.That(actual.Count, Is.EqualTo(3));
                }
            }
        }
    }
}
