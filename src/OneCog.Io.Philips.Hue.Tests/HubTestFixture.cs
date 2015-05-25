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
                                    new Dto.Indexed<Dto.Light> { Index = 1, Item = new Dto.Light { Name = "Item1", State = new Dto.LightState { On = true, Hue = 255 }}},
                                    new Dto.Indexed<Dto.Light> { Index = 2, Item = new Dto.Light { Name = "Item2", State = new Dto.LightState { On = true, Hue = 128 }}},
                                    new Dto.Indexed<Dto.Light> { Index = 3, Item = new Dto.Light { Name = "Item3", State = new Dto.LightState { On = true, Hue = 64 }}}
                                }  
                            )
                        }
                    );

                    Hub hub = new Hub(api, () => A.Fake<IInteraction>());

                    await hub.Connect(CancellationToken.None);

                    List<ILight> actual = new List<ILight>();
                    
                    hub.Lighting.Subscribe(actual.Add);

                    Assert.That(actual.Count, Is.EqualTo(3));
                }
            }
        }
    }
}
