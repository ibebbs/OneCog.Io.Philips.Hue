using NUnit.Framework;
using OneCog.Io.Philips.Hue.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue.Tests
{
    [TestFixture]
    public class DtoTestFixture
    {
        [Test]
        public void CanDeserializeState()
        {
            State state = Serializer.Json.Deserialize<State>(Resources.ConfigurationResponse);

            Assert.That(state, Is.Not.Null);
        }

        [Test]
        public void CanDeserializeStateLights()
        {
            State state = Serializer.Json.Deserialize<State>(Resources.ConfigurationResponse);

            Assert.That(state, Is.Not.Null);
            Assert.That(state.Lights, Is.Not.Null);
            Assert.That(state.Lights.Count, Is.EqualTo(2));

            Dto.Light light = state.Lights[2].Item;
            Assert.That(light, Is.Not.Null);
            Assert.That(light.State, Is.Not.Null);
            Assert.That(light.State.On, Is.True);
            Assert.That(light.State.Brightness, Is.EqualTo(254));
            Assert.That(light.State.Hue, Is.EqualTo(33536));
            Assert.That(light.State.Saturation, Is.EqualTo(144));
            Assert.That(light.State.Xy, Is.EqualTo(new float[] { 0.3460F, 0.3568F }));
            Assert.That(light.State.Ct, Is.EqualTo(201));
            Assert.That(light.State.Alert, Is.EqualTo("none"));
            Assert.That(light.State.Effect, Is.EqualTo("none"));
            Assert.That(light.State.ColorMode, Is.EqualTo("hs"));
            Assert.That(light.State.Reachable, Is.EqualTo(true));

            Assert.That(light.Type, Is.EqualTo("Extended color light"));
            Assert.That(light.Name, Is.EqualTo("Hue Lamp 2"));
            Assert.That(light.ModelId, Is.EqualTo("LCT001"));
            Assert.That(light.FirmwareVersion, Is.EqualTo("65003148"));
        }

        [Test]
        public void CanDeserializeStateChangeResponse()
        {
            StateChangeResponse response = Serializer.Json.Deserialize<StateChangeResponse>(Resources.SetLightResponse);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Changes, Is.Not.Null);
            Assert.That(response.Changes.Length, Is.EqualTo(3));
            Assert.That(response.Changes[0].Success, Is.True);
            Assert.That(response.Changes[0].Key, Is.EqualTo("/lights/1/state/bri"));
            Assert.That(response.Changes[0].Value, Is.EqualTo(200));
        }
    }
}
